using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class ResponseClassDto : BaseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public int? WeekRepetition { get; set; }
        public DateTime? RepeatUntil { get; set; }
        public DateTime EndDate { get; set; }
        public AccessRoles Role { get; set; }
    }
}
