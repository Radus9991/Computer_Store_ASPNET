using ApplicationCore.Entity;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Models
{
    public class UserEntity : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30), MinLength(3)]
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

        public List<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }
}
