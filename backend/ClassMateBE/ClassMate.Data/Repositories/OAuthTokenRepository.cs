using AutoMapper;
using ClassMate.Data.Entities;
using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Repositories
{
    public class OAuthTokenRepository : BaseRepository<OAuthTokenDto, OAuthToken>, IOAuthTokenRepository
    {
        public OAuthTokenRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<OAuthTokenDto?> GetByUserIdAndPlatformAsync(Guid userId, Platforms platform)
        {
            return await _mapper.ProjectTo<OAuthTokenDto?>(entities.Where(x => x.UserId == userId && x.Platform == platform)).FirstOrDefaultAsync();

        }
    }
}
