using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO.User
{
    public class UserAuthDTO
    {
        [Required]
        [MaxLength(30), MinLength(3)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(4)]
        public string Password { get; set; } = null!;
    }
}
