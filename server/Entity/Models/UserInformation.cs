using MongoDB.Bson.Serialization.Attributes;

namespace Entity.Models
{
    public class UserInformation
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("gender")]
        public int Gender { get; set; }

        [BsonElement("height")]
        public double Height { get; set; }
        [BsonElement("weight")]
        public double Weight { get; set; }
        [BsonElement("activityLevel")]
        public int ActivityLevel { get; set; }
        [BsonElement("fitnessGoal")]
        public int FitnessGoal { get; set; }
        [BsonElement("isActive")]
        public bool IsActive { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}