using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Dtos
{
    public class ResponsePaginatedDto<T>
    {
        public List<T>? Content { get; set; }
        public int Size { get; set; }
    }
}
