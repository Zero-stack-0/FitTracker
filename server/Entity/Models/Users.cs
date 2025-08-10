using MongoDB.Bson.Serialization.Attributes;

namespace Entity.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("firstName")]
        public string FirstName { get; set; }
        [BsonElement("lastName")]
        public string LastName { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("isActive")]
        public bool IsActive { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        [BsonElement("role")]
        public int Role { get; set; } = (int)Enums.UserRole.User;

    }
}