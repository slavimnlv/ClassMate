using AutoMapper;
using ClassMate.Data.Entities;
using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClassMate.Data.Repositories
{
    public class UsersNotesRepository : BaseRepository<UsersNotesDto, UsersNotes>, IUsersNotesRepository
    {
        public UsersNotesRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<ResponsePaginatedDto<ResponseNoteDto>> GetAllByUser(Guid userId, int offset, int limit, NoteFilterDto filter)
        {
            var query = entities.AsQueryable();

            query = query.Where(x=> x.UserID == userId);

            if(!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => x.Note!.Title!.ToLower().Contains(filter.Title.ToLower()));
            }

            if(filter.TagId.HasValue)
            {
                query = query.Where(x => x.Note!.Tags!.Any(tag => tag.ID == filter.TagId));

            }

            switch (filter.Ownership)
            {
                case OwnershipFilter.Mine:
                    query = query.Where(x => x.Role == AccessRoles.Owner);
                    break;
                case OwnershipFilter.Shared:
                    query = query.Where(x => x.Role != AccessRoles.Owner);
                    break;
                default:
                    break;
            }

            if (filter.Newest)
            {
                query = query.OrderByDescending(x => x.Note!.CreateDate);
            }
            else
            {
                query = query.OrderBy(x => x.Note!.CreateDate);
            }


            var count = await query.CountAsync();

            query = query.Skip(offset).Take(limit).Include(x => x.Note);

            var result = await _mapper.ProjectTo<ResponseNoteDto>(query).ToListAsync();

            return new ResponsePaginatedDto<ResponseNoteDto>
            {
                Content = result,
                Size = count
            };
        }

        public async Task<UsersNotesDto?> GetByUserAndNote(Guid userId, Guid noteId)
        {
            return await _mapper.ProjectTo<UsersNotesDto>(entities.Where(x => x.UserID == userId && x.NoteID == noteId).Include(x => x.Note).ThenInclude(x => x!.Tags).Include(x => x.User)).AsNoTracking().FirstOrDefaultAsync();

        }

        public async Task<UsersNotesDto?> GetNoteWithOwner(Guid noteID)
        {
            return await _mapper.ProjectTo<UsersNotesDto>(entities.Where(x => x.NoteID == noteID && x.Role == Domain.Enums.AccessRoles.Owner).Include(x => x.User)).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
