using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Entities
{
    public class UsersNotes : BaseEntity
    {
        public Guid NoteID { get; set; }
        public Note? Note { get; set; }
        public Guid UserID { get; set; }
        public User? User { get; set; }
        public AccessRoles Role { get; set; }
    }
}
