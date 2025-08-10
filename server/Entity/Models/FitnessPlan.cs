namespace Entity.Models
{
    public class FitnessPlan
    {
        public double calories_per_day { get; set; }
        public double water_liters_per_day { get; set; }
        public double bmi { get; set; }
        public double body_fat_percentage { get; set; }
        public double lean_body_mass_kg { get; set; }
        public Macros macros { get; set; }
        public int daily_steps_goal { get; set; }
        public MealBreakdown meal_breakdown { get; set; }
        public TrainingPlan training_plan { get; set; }
        public Micronutrients micronutrients { get; set; }
        public int sleep_hours_goal { get; set; }
        public List<string> recovery_tips { get; set; }
        public WeeklyProgressTargets weekly_progress_targets { get; set; }
        public FoodSuggestions food_suggestions { get; set; }
        public List<string> recommendations { get; set; }
    }
}
