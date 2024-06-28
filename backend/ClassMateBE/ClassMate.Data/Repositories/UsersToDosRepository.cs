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
    public class UsersToDosRepository : BaseRepository<UsersToDosDto, UsersToDos>, IUsersToDosRepository
    {
        public UsersToDosRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<ResponsePaginatedDto<ResponseToDoDto>> GetAllByUser(Guid userId, int offset, int limit, ToDoFilterDto filter)
        {
            var query = entities.AsQueryable();

            query = query.Where(x => x.UserID == userId);

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => x.ToDo!.Title!.ToLower().Contains(filter.Title.ToLower()));
            }

            if (filter.TagId.HasValue)
            {
                query = query.Where(x => x.ToDo!.Tags!.Any(tag => tag.ID == filter.TagId));

            }

            if (filter.HideDone)
            {
                query = query.Where(x => x.ToDo!.Done == false);
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

            query = query.OrderBy(x => x.ToDo!.Deadline);

            var count = await query.CountAsync();

            query = query.Skip(offset).Take(limit).Include(x => x.ToDo);

            var result = await _mapper.ProjectTo<ResponseToDoDto>(query).ToListAsync();

            return new ResponsePaginatedDto<ResponseToDoDto>
            {
                Content = result,
                Size = count
            };
        }

        public async Task<UsersToDosDto?> GetByUserAndToDo(Guid userId, Guid todoId)
        {
            return await _mapper.ProjectTo<UsersToDosDto>(entities.Where(x => x.UserID == userId && x.ToDoID == todoId).Include(x => x.ToDo).ThenInclude(x => x!.Tags).Include(x => x.User)).AsNoTracking().FirstOrDefaultAsync();

        }

        public async Task<UsersToDosDto?> GetToDoWithOwner(Guid todoID)
        {
            return await _mapper.ProjectTo<UsersToDosDto>(entities.Where(x => x.ToDoID == todoID && x.Role == Domain.Enums.AccessRoles.Owner).Include(x => x.User)).FirstOrDefaultAsync();
        }

        public async Task<UsersToDosDto?> GetNextToDo(Guid userId)
        {
            var now = DateTime.Now;

            var nextToDo = await _mapper.ProjectTo<UsersToDosDto>(entities
                .Include(ut => ut.ToDo)
                .Where(ut => ut.UserID == userId && ut.ToDo != null && !ut.ToDo.Done)
                .OrderBy(ut => ut.ToDo!.Deadline))
                .FirstOrDefaultAsync();

            if(nextToDo == null) {
                return null;
            }

            return nextToDo;
        }
    }
}
