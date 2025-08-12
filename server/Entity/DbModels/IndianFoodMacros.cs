using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Entity.DbModels
{

    public class IndianFoodMacros
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("food")]
        public string Name { get; set; }

        [BsonElement("serving_size_g")]
        public double ServingSizeG { get; set; }

        [BsonElement("calories")]
        public double Calories { get; set; }

        [BsonElement("protein_g")]
        public double ProteinG { get; set; }

        [BsonElement("carbs_g")]
        public double CarbsG { get; set; }

        [BsonElement("fat_g")]
        public double FatG { get; set; }
    }
}