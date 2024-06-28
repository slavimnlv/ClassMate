using ClassMate.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassMate.API.Models
{
    public record ShareModel
    (
        [Required]
        Guid ResourceID,

        [Required]
        string Email,

        [Required]
        AccessRoles Role

    );
}
