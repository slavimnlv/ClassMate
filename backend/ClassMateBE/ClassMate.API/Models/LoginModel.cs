using System.ComponentModel.DataAnnotations;

namespace ClassMate.API.Models
{
    public record LoginModel
    (
        [Required] string Email, 
        [Required] string Password
    );
    
}
