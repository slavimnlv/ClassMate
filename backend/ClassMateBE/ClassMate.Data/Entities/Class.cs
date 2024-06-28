using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Entities
{
    public class Class : BaseEntity
    {
        public required string Title { get; set; }
        public  string? Description { get; set; }
        public required DateTime StartDate { get; set; }
        public int? WeekRepetition { get; set; }
        public DateTime? RepeatUntil { get; set; }
        public required DateTime EndDate { get; set; }
    }
}
