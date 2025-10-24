using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO.User
{
    public class UserCreateDTO
    {
        [Required]
        [MaxLength(30), MinLength(3)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Surname { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(4)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(4)]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; } = null!;

        [Required]
        public string Birthday { get; set; } = null!;
    }
}
