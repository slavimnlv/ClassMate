using AutoMapper;
using ClassMate.Data.Entities;
using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Repositories
{
    public class UserRepository : BaseRepository<UserDto, User>, IUserRepository
    {
        public UserRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            return await _mapper.ProjectTo<UserDto>(entities.Where(x => x.Email == email)).FirstOrDefaultAsync();
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await entities.Where(x => x.Email == email).AnyAsync();
        }
    }
}
