namespace Wallspace.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Wallspace.Extensions;
    using Wallspace.Models;

    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<WallspaceUser> _userManager;

        public AccountController(UserManager<WallspaceUser> userManager)
        {
            _userManager = userManager;
        }

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
    }
}