using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ValidatorAPI.Entities;
using ValidatorAPI.Helpers;

namespace ValidatorAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<AppSettings> appSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<object> Authenticate([FromBody] ApiUser apiUser)
        {
            //de apiUser probeert in te loggen
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(
                apiUser.Username, apiUser.Password, false, false);

            if (signInResult.Succeeded)
            {
                IdentityUser user = _userManager.Users.SingleOrDefault(r => r.UserName == apiUser.Username);
                var roles = await _userManager.GetRolesAsync(user);

                //controleren of de identity toegang heeft tot bepaalde rechten in de api
                if (roles.Contains("Gebruiker"))
                {
                    return BadRequest(new { message = "Deze user is niet een admin, enkel admins mogen in deze API rechten hebben" });
                }
                //token instellen als de user een admin of super-admin is
                apiUser.Token = GenerateJwtToken(apiUser.Username, user).ToString();
          
                apiUser.Roles = roles.ToArray();
                
                return apiUser;
            }

            return BadRequest(new { message = "Username or password is incorrect" });
        }

        [HttpPost("checkToken")]
        public async Task<bool> CheckToken(ValidateToken token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (token.Token is string tok)
            {
                var decodedValue = handler.ReadJwtToken(tok);
                var k = decodedValue.ValidTo > DateTime.Now;
                return k;
            }

            return false;
          
          
        }





        private string GenerateJwtToken(string email, IdentityUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }


}
