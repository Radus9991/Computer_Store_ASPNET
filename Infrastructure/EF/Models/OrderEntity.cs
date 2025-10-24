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
    public class OrderEntity : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? PaypalId { get; set; } = null;

        [Required]
        public DateTime? Date { get; set; } = DateTime.Now;

        [Required]
        public List<ComputerEntity> Computers { get; set; } = new List<ComputerEntity>();

        [Required]
        public UserEntity User { get; set; } = null!;

        [Required]
        public double TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.PENDING;
    }
}
