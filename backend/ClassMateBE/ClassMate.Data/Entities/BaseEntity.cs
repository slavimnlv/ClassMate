using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Entities
{
    public abstract class BaseEntity
    {
        public Guid ID { get; set; }
    }
}
