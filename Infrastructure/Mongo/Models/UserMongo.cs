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
    public class UserMongo : IEntity<ObjectId?>
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId? Id { get; set; }

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
        public DateTime Birthday { get; set; } = DateTime.Now;

        [Required]
        public UserType Type { get; set; } = UserType.Client;

        [Required]
        public byte[] Hash { get; set; } = null!;

        [Required]
        public byte[] Salt { get; set; } = null!;
    }
}
