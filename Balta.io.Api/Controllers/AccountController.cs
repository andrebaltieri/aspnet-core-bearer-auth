using Balta.io.Api.Security;
using Balta.io.Domain.Commands.UserCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Balta.io.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly JsonSerializerSettings _serializerSettings;

        public AccountController(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromForm] AuthenticateUserCommand applicationUser)
        {
            var identity = await GetClaimsIdentity(applicationUser);
            if (identity == null)
                return BadRequest("Usuário ou senha inválidos");        

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Username),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("Balta.io")
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims.AsEnumerable(),
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                username = applicationUser.Username,
                otherData = "Valor custom"
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("O período deve ser maior que zero", nameof(JwtIssuerOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private static Task<ClaimsIdentity> GetClaimsIdentity(AuthenticateUserCommand user)
        {
            if (user.Username == "andrebaltieri" &&
                user.Password == "andrebaltieri")
            {
                return Task.FromResult(new ClaimsIdentity(
                    new GenericIdentity(user.Username, "Token"),
                    new[]
                    {
                        new Claim("Balta.io", "User")
                    }));
            }

            if (user.Username == "batman" &&
                user.Password == "batman")
            {
                return Task.FromResult(new ClaimsIdentity(
                    new GenericIdentity(user.Username, "Token"),
                    new[]
                    {
                        new Claim("Balta.io", "Admin"),
                        new Claim("Balta.io", "User")
                    }));
            }

            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
