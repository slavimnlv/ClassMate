using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; } 
        public  required string Email { get; set; }
        public  required string PasswordHash { get; set; }
        public  required string PasswordSalt { get; set; }

    }
}
