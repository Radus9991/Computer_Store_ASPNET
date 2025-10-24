using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entity;

namespace ApplicationCore.Models
{
    public class Order : IEntity
    {
        [Key]
        public string Id { get; set; } = null!;

        public DateTime Date { get; set; } = DateTime.Now;

        public string PaypalId { get; set; } = null!;

        public List<Computer> Computers { get; set; } = new List<Computer>();

        public User User { get; set; } = null!;

        public double TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.PENDING;
    }
}
