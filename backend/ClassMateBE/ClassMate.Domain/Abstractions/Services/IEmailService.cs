using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Services
{
    public interface IEmailService
    {
        void SendSharedNotification(string email, string from, string to, string link);
    }
}
