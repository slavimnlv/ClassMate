using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Services
{
    public interface IPasswordService
    {
        void HashPassword(string password, out string hash, out string salt);
        bool VerifyHashedPassword(string providedPassword, string hashedPassword, string saltString);
    }
}
