namespace Wallspace.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    using Microsoft.Extensions.Options;

    using Wallspace.Infrastructure.Jwt;
    using Wallspace.Models;

    public class JwtService : IJwtService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtService(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string WriteJwt(WallspaceUser wallspaceUser)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, wallspaceUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, IssueDateSinceUnixEpoch().ToString()),
                new Claim("Id", wallspaceUser.Id)
            };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(_jwtOptions.Issuer,
                                                                     _jwtOptions.Audience,
                                                                     claims,
                                                                     _jwtOptions.NotBefore,
                                                                     _jwtOptions.Expiration,
                                                                     _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private long IssueDateSinceUnixEpoch()
        {
            return (long)Math.Round((_jwtOptions.IssuedAt.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }
    }
}