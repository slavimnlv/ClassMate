using ClassMate.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ClassMate.API.Models
{
    public record CreateNoteModel
    (
        [Required]
        [StringLength(100)]
        string Title,

        [StringLength(2000)]
        string Content
    );
}
