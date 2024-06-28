﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Settings
{
    public class JwtSettings
    {
        public string? Issuer { get; set; }
        public string? Key { get; set; }
        public string? Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
    }
}
