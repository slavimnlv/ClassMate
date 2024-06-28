using ClassMate.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassMate.API.Models
{
    public record SyncModel
    (
        [Required]
        Guid ResourceId,

        [Required]
        Platforms Platform

    );
}
