using static Entity.Enums;

namespace Service.Dto.Request
{
    public class CreateUserFoodLogRq
    {
        public string? UserId { get; set; }
        public TimeOfTheDay TimeOfTheDay { get; set; }
        public string FoodId { get; set; }
        public double Quantity { get; set; }
    }

    public class CalculatedMaros
    {
        public double Calories { get; set; }
        public double ProteinG { get; set; }
        public double CarbsG { get; set; }
        public double FatG { get; set; }
    }
}