using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class ToDoDto : BaseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool Done { get; set; } = false;
        public List<TagDto> Tags { get; set; } = new();
    }
}
