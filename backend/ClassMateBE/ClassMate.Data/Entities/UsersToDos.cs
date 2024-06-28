using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Entities
{
    public class UsersToDos : BaseEntity
    {
        public Guid ToDoID { get; set; }
        public ToDo? ToDo { get; set; }
        public Guid UserID { get; set; }
        public User? User { get; set; }
        public Guid? CalendarEventId { get; set; }
        public CalendarEvent? CalendarEvent { get; set; }
        public AccessRoles Role { get; set; }
    }
}
