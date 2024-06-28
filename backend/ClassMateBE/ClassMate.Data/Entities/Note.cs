using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Entities
{
    public class Note : BaseEntity
    {
        public required string Title { get; set; }
        public string? Content { get; set; }
        public required DateTime CreateDate { get; set; }
        public List<Tag>? Tags { get; set; }

    }
}
