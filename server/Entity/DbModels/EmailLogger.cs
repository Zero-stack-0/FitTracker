using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static Entity.Enums;

namespace Entity.DbModels
{
    public class EmailLogger
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("emailTemplateType")]
        public EMAIL_TEMPLATE_TYPE EmailTemplateType { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("isEmailSent")]
        public bool IsEmailSent { get; set; }
    }
}