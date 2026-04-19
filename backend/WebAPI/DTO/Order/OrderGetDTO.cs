using ApplicationCore.Models;
using System.ComponentModel.DataAnnotations;
using WebAPI.DTO.Computer;

namespace WebAPI.DTO.Order
{
    public class OrderGetDTO
    {
        public string Id { get; set; } = null!;

        public DateTime? Date { get; set; } = null!;

        public string PaypalId { get; set; } = null!;

        public double TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
