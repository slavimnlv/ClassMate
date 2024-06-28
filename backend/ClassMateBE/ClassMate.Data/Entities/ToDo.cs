using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Entities
{
    public class ToDo : BaseEntity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required DateTime Deadline { get; set; }
        public bool Done { get; set; } = false;
        public List<Tag>? Tags { get; set; }

    }
}
