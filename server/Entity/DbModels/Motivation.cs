using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entity.DbModels
{
    public class Motivation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("quote")]
        public string Quote { get; set; }

        [BsonElement("number")]
        public int Number { get; set; }
    }
}