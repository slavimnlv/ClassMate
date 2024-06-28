using ClassMate.Domain.Enums;

namespace ClassMate.API.Models.Filters
{
    public record NoteFilter 
    (
        Guid? TagId,
        bool Newest,
        string? Title,
        OwnershipFilter Ownership = OwnershipFilter.All
    );
      
}
