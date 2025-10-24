using ApplicationCore.Entity;
using ApplicationCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mongo.Models
{
    public class OrderMongo : IEntity<ObjectId?>
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId? Id { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public string PaypalId { get; set; } = null!;

        [Required]
        public List<ComputerMongo> Computers { get; set; } = new List<ComputerMongo>();

        [Required]
        public UserMongo User { get; set; } = null!;

        [Required]
        public double TotalAmount { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.PENDING;
    }
}
