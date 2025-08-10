using Entity.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Entity.DbModels
{
    public class FitnessAndNutritionPlan
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("userId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }
        [BsonElement("userInformationId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserInformationId { get; set; }
        [BsonElement("fitnessPlan")]
        public FitnessPlan? FitnessPlan { get; set; }
        [BsonElement("nutritionPlan")]
        public NutritionPlan? NutritionPlan { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

    }
}