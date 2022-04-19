using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ValidatorAPI.DomainModels
{
    public class ValidatorUser
    {
        [Key]
        public int Id { get; set; }

        public string Naam { get; set; }

        [ForeignKey("Account")]
        public string AccountId { get; set; }
        
        [ForeignKey("Bedrijf")]
        public int BedrijfId { get; set; }


        public Company Bedrijf { get; set; }
        public IdentityUser Account { get; set; }
       



    }
}
