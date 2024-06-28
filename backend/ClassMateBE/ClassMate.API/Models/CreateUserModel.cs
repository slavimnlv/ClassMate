using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ClassMate.API.Models
{
    public record CreateUserModel 
    (
        [Required]
        [StringLength(50, MinimumLength = 2)]
        string Name,

        [Required]
        [EmailAddress]
        [StringLength(100)]
        string Email,

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")]
        string Password
    );
    
}
