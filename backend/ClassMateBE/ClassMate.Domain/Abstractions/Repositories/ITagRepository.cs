using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface ITagRepository : IBaseRepository<TagDto>
    {
        Task<List<TagDto>> GetTagsByUser(Guid userId); 
        Task<bool> AnyNotesAndToDosAsync(Guid tagID);

    }
}
