using static Entity.Enums;

namespace Service.Dto.Request
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public ActivityLevel ActivityLevel { get; set; }
        public FintnessGoal FitnessGoal { get; set; }
        public DIET_TYPE DietType { get; set; }
        public string Location { get; set; }
    }
}