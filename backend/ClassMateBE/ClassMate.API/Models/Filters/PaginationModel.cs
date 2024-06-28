using System.ComponentModel.DataAnnotations;

namespace ClassMate.API.Models.Filters
{
    public record PaginationModel
    (
        int PageNumber = 1,
        int PageSize = 8
    );
}
