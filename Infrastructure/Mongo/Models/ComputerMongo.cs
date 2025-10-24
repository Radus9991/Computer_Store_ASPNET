using ApplicationCore.Entity;
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
    public class ComputerMongo : IEntity<ObjectId?>
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId? Id { get; set; }

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
