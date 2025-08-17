using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static Entity.Enums;

namespace Entity.DbModels
{
    public class EmailTemplate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("subject")]
        public string Subject { get; set; }
        [BsonElement("body")]
        public string Body { get; set; }
        [BsonElement("type")]
        public EMAIL_TEMPLATE_TYPE Type { get; set; }
    }
}