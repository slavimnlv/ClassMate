using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Services
{
    public interface IClassService : IBaseService<ClassDto>
    {
        Task CreateClassAsync(ClassDto dto);
        Task DeleteClassAsync(Guid id);
        Task<List<UsersClassesDto>> GetAllAsync();
        Task<UsersClassesDto> GetClassAsync(Guid id);
        Task ShareClassAsync(Guid resourceID, string email, AccessRoles role);
        Task UpdateClassAsync(ClassDto dto);
        Task SyncClassAsync(Guid resourceId);
        Task<UsersClassesDto?> GetNextClass();
    }
}
