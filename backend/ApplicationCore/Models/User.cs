using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entity;

namespace ApplicationCore.Models
{
    public class User : IEntity
    {
        [Key]
        public string Id { get; set; } = null!;

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
        public DateTime Birthday { get; set; }

        [Required]
        public UserType Type { get; set; } = UserType.Client;

        [Required]
        public byte[] Hash { get; set; } = null!;

        [Required]
        public byte[] Salt { get; set; } = null!;

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
