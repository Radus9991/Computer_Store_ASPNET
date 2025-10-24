using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO.Computer
{
    public class ComputerCreateDTO
    {
        [Required]
        [MaxLength(30), MinLength(3)]
        public string Mainboard { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string GPU { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string CPU { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Ram { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Ssd { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string PowerSupply { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Cooler { get; set; } = null!;

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Case { get; set; } = null!;

        [Required]
        public double Price { get; set; }
    }
}
