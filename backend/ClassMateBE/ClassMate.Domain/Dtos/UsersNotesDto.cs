using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class UsersNotesDto : BaseDto
    {
        public Guid NoteID { get; set; }
        public NoteDto? Note { get; set; }
        public Guid UserID { get; set; }
        public UserDto? User { get; set; }
        public AccessRoles Role { get; set; }
    }
}
