using ClassMate.Domain.Dtos;
using ClassMate.Domain.Enums;

namespace ClassMate.Domain.Abstractions.Services
{
    public interface INoteService : IBaseService<NoteDto>
    {
        Task CreateNoteAsync(NoteDto dto);
        Task DeleteNoteAsync(Guid id);
        Task<ResponsePaginatedDto<ResponseNoteDto>> GetAllNotesAsync(int pageNumber, int pageSize,NoteFilterDto filter);
        Task<UsersNotesDto> GetNoteAsync(Guid id);
        Task ShareNoteAsync(Guid resourceID, string email, AccessRoles role);
        Task UpdateNoteAsync(NoteDto dto);
    }
}
