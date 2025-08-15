using MongoDB.Bson.Serialization.Attributes;
using static Entity.Enums;

namespace Entity.Models
{
    public class AiResponse
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("type")]
        public AiPromptType Type { get; set; }
        [BsonElement("response")]
        public dynamic Response { get; set; }
        [BsonElement("prompt-used")]
        public string PromptUsed { get; set; }
        [BsonElement("userId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}