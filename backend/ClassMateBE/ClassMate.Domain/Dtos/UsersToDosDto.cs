using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class UsersToDosDto : BaseDto
    {
        public Guid ToDoID { get; set; }
        public ToDoDto? ToDo { get; set; }
        public Guid UserID { get; set; }
        public UserDto? User { get; set; }
        public Guid? CalendarEventId { get; set; }
        public CalendarEventDto? CalendarEvent { get; set; }
        public AccessRoles Role { get; set; }
    }
}
