using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Services
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
        Task CreateToDoAsync(ToDoDto dto);
        Task DeleteToDoAsync(Guid id);
        Task<ResponsePaginatedDto<ResponseToDoDto>> GetAllToDosAsync(int pageNumber, int pageSize, ToDoFilterDto filter);
        Task<UsersToDosDto> GetToDoAsync(Guid id);
        Task ShareToDoAsync(Guid resourceID, string email, AccessRoles role);
        Task UpdateToDoAsync(ToDoDto dto);
        Task SyncToDoAsync(Guid resourceId);
        Task<UsersToDosDto?> GetNextToDo();

    }
}
