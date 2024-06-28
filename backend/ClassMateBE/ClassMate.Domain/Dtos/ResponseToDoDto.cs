using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class ResponseToDoDto : BaseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool Done { get; set; } = false;
        public AccessRoles Role { get; set; }

    }
}
