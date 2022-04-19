using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValidatorAPI.DomainModels;

namespace ValidatorAPI.Data
{
    public static class DBInitializer
    {
        
        public static void Initizalizer(ValidatorContext context)
        {
            context.Database.EnsureCreated();
            //hasher klaarzetten
            PasswordHasher<IdentityUser> password = new PasswordHasher<IdentityUser>();

            if (context.ValidatorUsers.Any())
            {
                return;
            }



            context.Companies.Add(new Company() { Bedrijfsnaam = "Robonext" });
            context.Companies.Add(new Company { Bedrijfsnaam = "SnD" });


            context.SaveChangesAsync();

            ///super admin
            IdentityUser superAdmin = new IdentityUser();
            superAdmin.Email = "gilles.gui@robonext.eu";
            superAdmin.NormalizedEmail = superAdmin.Email.ToUpper();
            superAdmin.UserName = superAdmin.Email;
            superAdmin.NormalizedUserName = superAdmin.UserName.ToUpper();
            superAdmin.EmailConfirmed = true;
            superAdmin.PhoneNumber = "0456789123";

            superAdmin.PasswordHash = password.HashPassword(superAdmin, "RobonextTest1!");

            context.ValidatorUsers.Add(new ValidatorUser() { Naam = "Gui Gilles", Account = superAdmin, AccountId = superAdmin.Id, BedrijfId = 1});

            context.SaveChangesAsync();

            DbSet<IdentityUserRole<string>> roles = context.UserRoles;
            IdentityRole role = context.Roles.FirstOrDefault(x => x.Name == "Super-Admin");
            if (!roles.Any(us => us.UserId == superAdmin.Id && us.RoleId == role.Id))
            {
                roles.Add(new IdentityUserRole<string>() { UserId = superAdmin.Id, RoleId = role.Id });
            }

            context.SaveChanges();

            IdentityUser admin = new IdentityUser();
            admin.Email = "guigilles@gmail.com";
            admin.NormalizedEmail = admin.Email.ToUpper();
            admin.UserName = admin.Email;
            admin.NormalizedUserName = admin.UserName.ToUpper();
            admin.EmailConfirmed = true;
            admin.PhoneNumber = "0498765432";
            admin.PasswordHash = password.HashPassword(admin, "SndTest1!");
            context.ValidatorUsers.Add(new ValidatorUser() { Naam = "Gui Gilles", Account = admin, AccountId = admin.Id, BedrijfId = 2});

            context.SaveChanges();

            role = context.Roles.FirstOrDefault(x => x.Name == "Admin");
            if (!roles.Any(us => us.UserId == admin.Id && us.RoleId == role.Id))
            {
                roles.Add(new IdentityUserRole<string>() { UserId = admin.Id, RoleId = role.Id });
            }
            context.SaveChanges();
        }
    }
}
