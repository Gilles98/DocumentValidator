using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValidatorAPI.DomainModels;

namespace ValidatorAPI.Data
{
    public class ValidatorContext : IdentityDbContext
    {
        public DbSet<ValidatorUser> ValidatorUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public ValidatorContext(DbContextOptions<ValidatorContext> options): base(options) {
        
           
        }

        public ValidatorContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ValidatorUser>().ToTable("ValidatorUser");
            modelBuilder.Entity<Company>().ToTable("Company");

            base.OnModelCreating(modelBuilder);
        }
    }
}
