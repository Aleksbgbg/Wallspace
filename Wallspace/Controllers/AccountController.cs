namespace Wallspace.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Wallspace.Extensions;
    using Wallspace.Infrastructure.Jwt;
    using Wallspace.Models;
    using Wallspace.Services;

    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<WallspaceUser> _userManager;

        private readonly IJwtService _jwtService;

        private readonly JwtIssuerOptions _jwtOptions;

        public AccountController(UserManager<WallspaceUser> userManager, IJwtService jwtService, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _jwtOptions = jwtOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(RegistrationCredentials registrationCredentials)
        {
            if (ModelState.IsValid)
            {
                WallspaceUser newUser = new WallspaceUser
                {
                    UserName = registrationCredentials.Username,
                    Email = registrationCredentials.Email,
                    PhoneNumber = registrationCredentials.PhoneNumber
                };

                IdentityResult createResult = await _userManager.CreateAsync(newUser, registrationCredentials.Password);

                if (createResult.Succeeded)
                {
                    return Ok();
                }

                ModelState.AddIdentityErrors(createResult);
            }

            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginCredentials loginCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WallspaceUser wallspaceUser = await _userManager.FindByNameAsync(loginCredentials.Username);

            if (wallspaceUser == null)
            {
                ModelState.AddModelError("login_failure", "Invalid username.");
                return BadRequest(ModelState);
            }

            if (await _userManager.CheckPasswordAsync(wallspaceUser, loginCredentials.Password))
            {
                return new JsonResult(new
                {
                    Id = wallspaceUser.Id,
                    AuthToken = _jwtService.WriteJwt(wallspaceUser),
                    ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
                });
            }

            ModelState.AddModelError("login_failure", "Invalid password.");
            return BadRequest(ModelState);
        }
    }
}