using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ValidatorAPI.DomainModels
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string Bedrijfsnaam { get; set; }


        public ICollection<ValidatorUser> Users { get; set; }
    }
}
