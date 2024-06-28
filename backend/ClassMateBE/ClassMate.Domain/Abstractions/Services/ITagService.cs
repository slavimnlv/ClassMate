using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Services
{
    public interface ITagService : IBaseService<TagDto>
    {
        Task<List<TagDto>> GetAllTagsAsync();
    }
}
