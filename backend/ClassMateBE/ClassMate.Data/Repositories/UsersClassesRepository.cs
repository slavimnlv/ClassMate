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
    public class UsersClassesRepository : BaseRepository<UsersClassesDto, UsersClasses>, IUsersClassesRepository
    {
        public UsersClassesRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<UsersClassesDto>> GetAllByUser(Guid userId)
        {
            return await _mapper.ProjectTo<UsersClassesDto>(entities.Where(x => x.UserID == userId).Include(x => x.Class).Include(x => x.User)).ToListAsync();
        }

        public async Task<UsersClassesDto?> GetByUserAndClass(Guid userId, Guid classId)
        {
            return await _mapper.ProjectTo<UsersClassesDto>(entities.Where(x => x.UserID == userId && x.ClassId == classId).Include(x => x.Class).Include(x => x.User)).AsNoTracking().FirstOrDefaultAsync();

        }
    }
}
