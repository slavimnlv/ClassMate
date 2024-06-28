﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Settings
{
    public class EmailSettings
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }
        public string? Host {  get; set; }
    }
}
