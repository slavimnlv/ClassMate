using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using ClassMate.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Services
{
    public class UserService : BaseService<UserDto>, IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository,
            IPasswordService passwordService,
            IJwtService jwtService) : base(userRepository)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }


        public async Task CreateUserAsync(UserDto userDto)
        {
            bool emailIsFree = await VerifyEmailAsync(userDto.Email!);
            if (emailIsFree)
            {
                _passwordService.HashPassword(userDto.Password!, out string hash, out string salt);
                userDto.Password = hash;
                userDto.Salt = salt;
                await CreateAsync(userDto);
            }
            else
                throw new AppException("Email already in use!");
        }

        public async Task<bool> VerifyEmailAsync(string email)
        {
            var taken = await _userRepository.IsEmailTakenAsync(email);
            return !taken;
        }

        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            var dto = await _userRepository.GetByEmailAsync(email);
            if (dto != null)
            {
                if (_passwordService.VerifyHashedPassword(password, dto.Password!, dto.Salt!))
                {
                    string token = _jwtService.GenerateJwtToken(dto.ID);

                    return new LoginResponseDto{ Token = token};
                }
            }

            throw new AppException("Invalid credentials!");
        }

    }
}
