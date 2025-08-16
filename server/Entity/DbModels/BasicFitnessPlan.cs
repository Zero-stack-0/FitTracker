using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entity.Models
{
    public class BasicFitnessPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("macroTargets")]
        public MacroTargets MacroTargets { get; set; }

        [BsonElement("mealIdeas")]
        public List<MealIdea> MealIdeas { get; set; }

        [BsonElement("workoutPlan")]
        public WorkoutPlan WorkoutPlan { get; set; }

        [BsonElement("mealTimingTips")]
        public List<string> MealTimingTips { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
    public class MacroTargets
    {
        [BsonElement("calories")]
        public int Calories { get; set; }

        [BsonElement("protein_g")]
        public int protein_g { get; set; }

        [BsonElement("carbs_g")]
        public int carbs_g { get; set; }

        [BsonElement("fats_g")]
        public int fats_g { get; set; }
    }

    public class MealIdea
    {
        [BsonElement("meal")]
        public string Meal { get; set; }

        [BsonElement("ideas")]
        public List<string> Ideas { get; set; }
    }

    public class WorkoutPlan
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("days")]
        public List<WorkoutDay> Days { get; set; }

        [BsonIgnore]
        [BsonElement("mealTimingTips")]
        public List<string> MealTimingTips { get; set; }
    }

    public class WorkoutDay
    {
        [BsonElement("day")]
        public string Day { get; set; }

        [BsonElement("focus")]
        public string Focus { get; set; }

        [BsonElement("exercises")]
        public List<string> Exercises { get; set; }
    }
}