using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface IToDoRepository : IBaseRepository<ToDoDto>
    {
        Task<ToDoDto> CreateToDoAsync(ToDoDto dto);
        Task<ToDoDto> UpdateToDoAsync(List<TagDto> userTags, List<string> tags, Guid userId, ToDoDto dto);



    }
}
