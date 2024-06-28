using AutoMapper;
using Azure;
using ClassMate.Data.Entities;
using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Repositories
{
    public class ToDoRepository : BaseRepository<ToDoDto, ToDo>, IToDoRepository
    {
        public ToDoRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<ToDoDto> CreateToDoAsync(ToDoDto dto)
        {

            dto.ID = Guid.NewGuid();
            var entity = _mapper.Map<ToDo>(dto);

            if(entity.Tags != null)
            {
                foreach(var tag in entity.Tags)
                {
                    if(tag.ID != Guid.Empty)
                    {
                        _dbContext.Tags.Attach(tag);
                    }
                }
            }

            await entities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ToDoDto>(entity);

        }

        public async Task<ToDoDto> UpdateToDoAsync(List<TagDto> userTags, List<string> tags, Guid userId, ToDoDto dto)
        {

            List<TagDto> newTags = new List<TagDto>();

            foreach (var tagName in tags)
            {
                var tag = userTags.FirstOrDefault(tag => tag.Title == tagName);

                if (tag == null)
                {
                    tag = new TagDto
                    {
                        UserID = userId,
                        Title = tagName
                    };
                }

                newTags.Add(tag);
            }

            dto.Tags = newTags;

            var entity = _mapper.Map<ToDo>(dto);

            var fromDB = await entities.Include(e => e.Tags).FirstOrDefaultAsync(e => e.ID == entity.ID);


            if (entity.Tags != null && fromDB!.Tags != null)
            {
                foreach (Tag tag in entity.Tags)
                {
                    if (!fromDB.Tags.Any(t => t.ID == tag.ID))
                    {
                        fromDB.Tags.Add(tag);
                    }

                }

                var tagDtoIds = new HashSet<Guid>(newTags.Select(dto => dto.ID));

                fromDB.Tags.RemoveAll(tag => !tagDtoIds.Contains(tag.ID));

            }
            else
            {
                fromDB!.Tags = entity.Tags;
            }

            fromDB.Description = entity.Description;
            fromDB.Deadline = entity.Deadline;
            fromDB.Title = entity.Title;
            fromDB.Done = entity.Done;

            _dbContext.Set<ToDo>().Update(fromDB);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ToDoDto>(fromDB);


        }
    }
}
