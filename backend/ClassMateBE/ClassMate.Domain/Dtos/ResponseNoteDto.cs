using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class ResponseNoteDto : BaseDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public AccessRoles Role { get; set; }

    }
}
