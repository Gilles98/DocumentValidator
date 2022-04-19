using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidatorAPI.Entities
{
    public class ApiUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string[] Roles { get; set; }
    }
}
