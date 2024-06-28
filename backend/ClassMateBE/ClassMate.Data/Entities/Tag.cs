using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Entities
{
    public class Tag : BaseEntity
    {
        public required string Title { get; set; }
        public Guid UserID { get; set; }
        public User? User { get; set; }
        public List<Note>? Notes { get; set; }
        public List <ToDo>? ToDos { get; set; }

    }
}
