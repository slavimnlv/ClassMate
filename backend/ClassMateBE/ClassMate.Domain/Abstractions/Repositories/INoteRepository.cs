using ClassMate.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface INoteRepository : IBaseRepository<NoteDto>
    {
        Task<NoteDto> CreateNoteAsync(NoteDto dto);
        Task<NoteDto> UpdateNoteAsync(List<TagDto> userTags, List<string> tags, Guid userId, NoteDto dto);
    }
}
