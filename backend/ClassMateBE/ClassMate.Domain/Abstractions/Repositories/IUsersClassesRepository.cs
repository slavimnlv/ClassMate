using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface IUsersClassesRepository : IBaseRepository<UsersClassesDto>
    {
        Task<List<UsersClassesDto>> GetAllByUser(Guid userId);
        Task<UsersClassesDto?> GetByUserAndClass(Guid userId, Guid classId);
    }
}
