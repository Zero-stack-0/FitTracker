using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static Entity.Enums;

namespace Entity.DbModels
{
    public class UserFoodLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("foodId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string FoodId { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("quantity_g")]
        public double Quantity { get; set; }

        [BsonElement("timeOfTheDay")]
        public TimeOfTheDay TimeOfTheDay { get; set; }
        public bool IsActive { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        [BsonElement("calories")]
        public double Calories { get; set; }
        [BsonElement("protein")]
        public double ProteinG { get; set; }
        [BsonElement("carb")]
        public double CarbsG { get; set; }
        [BsonElement("fat")]
        public double FatG { get; set; }
    }
}