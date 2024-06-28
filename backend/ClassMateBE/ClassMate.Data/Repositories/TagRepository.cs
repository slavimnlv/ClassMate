using AutoMapper;
using ClassMate.Data.Entities;
using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Repositories
{
    public class TagRepository : BaseRepository<TagDto, Tag>, ITagRepository
    {
        public TagRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<TagDto>> GetTagsByUser(Guid userId)
        {
            return await _mapper.ProjectTo<TagDto>(entities.Where(x => x.UserID == userId)).AsNoTracking().ToListAsync();
        }

        public async Task<bool> AnyNotesAndToDosAsync(Guid tagID)
        {
            var tag = await entities
            .Where(t => t.ID == tagID)
            .FirstOrDefaultAsync();

            if (!tag!.Notes.IsNullOrEmpty() || !tag.ToDos.IsNullOrEmpty())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
