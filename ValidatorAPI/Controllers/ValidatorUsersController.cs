using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValidatorAPI.Data;
using ValidatorAPI.DomainModels;

namespace ValidatorAPI.Controllers
{
    [Route("/api")]
    [ApiController]
    public class ValidatorUsersController : ControllerBase
    {


        private ValidatorContext _context = new ValidatorContext();
        private readonly UserManager<IdentityUser> _userManager;
        PasswordHasher<IdentityUser> _password = new PasswordHasher<IdentityUser>();
        public ValidatorUsersController(ValidatorContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ValidatorUser>> GetAll()
        {
            return await _context.ValidatorUsers.ToListAsync();
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("companies")]
        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ValidatorUser> GetById(int id)
        {
            ValidatorUser user = await _context.ValidatorUsers.FindAsync(id);
            user.Account = await _userManager.FindByIdAsync(user.AccountId);
            user.Account.PasswordHash = "hidden";
            return user;
        }



        [HttpGet("roles")]
        public async Task<IEnumerable<string>> GetAllRoles()
        {
            return await _context.Roles.Select(x => x.Name).Where(y => y != "Super-Admin").ToListAsync();
        }


        [HttpGet("user_emails")]
        public async Task<IEnumerable<string>> GetAllEmails()
        {

            return await _context.Users.Select(x => x.UserName).ToListAsync();
        }
        [HttpGet("user_emailByCompany")]
        public async Task<string> GetEmailByAccountId(string accId)
        {
            var result = await _context.Users.Where(x => x.Id == accId).FirstOrDefaultAsync();
            return result.UserName;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("user/create")]

        public async Task<ValidatorUser> PostNewAdmin([FromBody]NewUser user)
        {

            ///controles aanmaken voor welk type identityUser het moet toevoegen
            ///newUser later nog uitbreiden, mogleijk validatorUser ook

            //identity aanmaken
            IdentityUser admin = new IdentityUser();
            admin.Email = user.Email ;
            admin.NormalizedEmail = admin.Email.ToUpper();
            admin.UserName = admin.Email;
            admin.NormalizedUserName = admin.UserName.ToUpper();
            admin.EmailConfirmed = true;
            admin.PhoneNumber = user.Phone.ToString();
            admin.PasswordHash = _password.HashPassword(admin, user.Password);

            //validator aanmaken aanmaken
            ValidatorUser vUser = new ValidatorUser() { Naam = user.Name, Account = admin, AccountId = admin.Id };

            await _context.ValidatorUsers.AddAsync(vUser);

            await _context.SaveChangesAsync();
            DbSet<IdentityUserRole<string>> roles = _context.UserRoles;
            IdentityRole role = _context.Roles.FirstOrDefault(x => x.Name == "Admin");

            //rol toevoegen
            if (!roles.Any(us => us.UserId == admin.Id && us.RoleId == role.Id))
            {
                roles.Add(new IdentityUserRole<string>() { UserId = admin.Id, RoleId = role.Id });
            }
            await _context.SaveChangesAsync();
            return vUser;
        }

        /*     [HttpPost("files")]
             public async Task<File> PostFile([FromBody] File file)
             {

             }  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        */

    

        [HttpDelete("user/delete/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ValidatorUser>> DeleteUser(int id)
        {

            //uitbreiden met controles

            //user ophalen
            ValidatorUser user = await _context.ValidatorUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //identity ophalen
            var identityUser = await _userManager.FindByIdAsync(user.AccountId);

            //user en identity verwijderen
            await _userManager.DeleteAsync(identityUser);
            _context.Set<ValidatorUser>().Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
