
using MongoDB.Bson.Serialization.Attributes;
using static Entity.Enums;

namespace Entity.Models
{
    public class AiPrompt
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Type")]
        public AiPromptType Type { get; set; }
        [BsonElement("prompt")]
        public string Prompt { get; set; }
    }
}