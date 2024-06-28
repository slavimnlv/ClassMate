using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class TagDto : BaseDto
    {
        public string? Title { get; set; }
        public Guid UserID { get; set; }

    }
}
