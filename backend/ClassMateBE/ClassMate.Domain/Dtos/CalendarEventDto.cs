using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class CalendarEventDto : BaseDto
    {
        public Platforms Platform { get; set; }
        public string PlatformEventId { get; set; }
        public Guid UserId { get; set; }
    }
}
