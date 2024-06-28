using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class ToDoFilterDto
    {
        public Guid? TagId { get; set; }
        public string? Title { get; set; }
        public bool HideDone {  get; set; }
        public OwnershipFilter Ownership {  get; set; }

    }
}
