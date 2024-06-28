using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Settings
{
    public class GoogleSettings
    {
        public string? ClientID { get; set; }
        public string? AuthUri { get; set; }
        public string? TokenUri { get; set; }
        public string? ClientSecret { get; set; }
        public string? RedirectUri { get; set; }
    }
}
