namespace Service.Dto.Request
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int ActivityLevel { get; set; }
        public int FitnessGoal { get; set; }
    }
}