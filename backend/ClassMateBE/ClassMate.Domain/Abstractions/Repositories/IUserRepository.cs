using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface IUserRepository : IBaseRepository<UserDto>
    {
        Task<bool> IsEmailTakenAsync(string email);
        Task<UserDto?> GetByEmailAsync(string email);
    }
}
