using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Services
{
    public interface IUserService : IBaseService<UserDto>
    {
        Task<bool> VerifyEmailAsync(string email);
        Task CreateUserAsync(UserDto userDto);
        Task<LoginResponseDto> LoginAsync(string email, string password);
    }
}
