using System.ComponentModel.DataAnnotations;

namespace ClassMate.API.Models
{
    public record UpdateToDoModel
    (
        [Required]
        Guid ID,

        [Required]
        [StringLength(100)]
        string Title,

        [StringLength(500)]
        string Description,

        [Required]
        DateTime Deadline,

        Guid? ToDoID,

        bool Done

    );
}
