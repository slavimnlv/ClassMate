using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface IUsersToDosRepository : IBaseRepository<UsersToDosDto>
    {
        Task<ResponsePaginatedDto<ResponseToDoDto>> GetAllByUser(Guid userId, int offset, int limit, ToDoFilterDto filter);
        Task<UsersToDosDto?> GetByUserAndToDo(Guid userId, Guid todoId);
        Task<UsersToDosDto?> GetNextToDo(Guid userId);
        Task<UsersToDosDto?> GetToDoWithOwner(Guid todoID);
    }
}
