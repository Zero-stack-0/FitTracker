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
    }
}