using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class NoteFilterDto
    {
        public Guid? TagId { get; set; }
        public bool Newest { get; set; } = true;
        public string? Title {  get; set; }
        public OwnershipFilter Ownership {  get; set; }

    }
}
