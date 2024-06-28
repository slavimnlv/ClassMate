using System.ComponentModel.DataAnnotations;

namespace ClassMate.API.Models
{
    public record UpdateNoteModel
    (
        [Required]
        Guid ID,

        [Required]
        [StringLength(100)]
        string Title,

        [StringLength(2000)]
        string Content
    );
}
