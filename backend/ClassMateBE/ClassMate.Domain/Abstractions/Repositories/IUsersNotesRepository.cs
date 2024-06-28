using ClassMate.Domain.Dtos;

namespace ClassMate.Domain.Abstractions.Repositories
{
    public interface IUsersNotesRepository : IBaseRepository<UsersNotesDto>
    {
        Task<ResponsePaginatedDto<ResponseNoteDto>> GetAllByUser(Guid userId,int offset, int limit, NoteFilterDto filter);
        Task<UsersNotesDto?> GetByUserAndNote(Guid userId, Guid noteId);
        Task<UsersNotesDto?> GetNoteWithOwner(Guid noteID);
    }
}
