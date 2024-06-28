using ClassMate.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassMate.API.Models
{
    public record UpdateClassModel
    (
        [Required]
        Guid ID,

        [Required]
        [StringLength(100)]
        string Title,

        [StringLength(500)]
        string Description,

        [Required]
        DateTime StartDate,

        int? WeekRepetition,

        DateTime? RepeatUntil,

        [Required]
        DateTime EndDate
    );
}
