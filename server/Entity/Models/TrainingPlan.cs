
namespace Entity.Models
{
    public class TrainingPlan
    {
        public string type { get; set; }
        public int days_per_week { get; set; }
        public int duration_minutes { get; set; }
        public List<string> example_exercises { get; set; }
    }
}