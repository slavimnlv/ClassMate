using ClassMate.Domain.Enums;

namespace ClassMate.API.Models.Filters
{
    public record ToDoFilter
    (
        Guid? TagId,
        string? Title,
        bool HideDone = false,
        OwnershipFilter Ownership = OwnershipFilter.All
    );
}
