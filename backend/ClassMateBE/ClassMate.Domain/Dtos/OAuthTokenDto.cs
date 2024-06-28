﻿using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class OAuthTokenDto : BaseDto
    {
        public Guid UserId { get; set; }
        public Platforms Platform { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime IssuedUtc { get; set; }
        public long? ExpiresInSeconds { get; set; }
    }
}
