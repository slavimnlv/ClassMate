using AutoMapper;
using ClassMate.Data.Entities;
using ClassMate.Domain.Abstractions.Repositories;
using ClassMate.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data.Repositories
{
    public class NoteRepository : BaseRepository<NoteDto, Note>, INoteRepository    
    {
        public NoteRepository(ClassMateDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<NoteDto> CreateNoteAsync(NoteDto dto)
        {

            dto.ID = Guid.NewGuid();
            var entity = _mapper.Map<Note>(dto);

            if (entity.Tags != null)
            {
                foreach (var tag in entity.Tags)
                {
                    if (tag.ID != Guid.Empty)
                    {
                        _dbContext.Tags.Attach(tag);
                    }
                }
            }

            await entities.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<NoteDto>(entity);

        }

        public async Task<NoteDto> UpdateNoteAsync(List<TagDto> userTags, List<string> tags, Guid userId, NoteDto dto)
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

            var entity = _mapper.Map<Note>(dto);

            var fromDB = await entities.Include(e => e.Tags).FirstOrDefaultAsync(e => e.ID == entity.ID);


            if(entity.Tags != null && fromDB!.Tags != null)
            {
                foreach (Tag tag in entity.Tags)
                {
                    if(!fromDB.Tags.Any(t => t.ID == tag.ID))
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

            fromDB.CreateDate = entity.CreateDate;
            fromDB.Content = entity.Content;
            fromDB.Title = entity.Title;


            _dbContext.Set<Note>().Update(fromDB);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<NoteDto>(fromDB);

        }

    }
}
