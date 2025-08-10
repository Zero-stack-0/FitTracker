using static Entity.Enums;

namespace Service.Dto.Request
{
    public class GenerateFitnessPlanRequest
    {
        public string? UserId { get; set; }
        public DIET_TYPE DietType { get; set; }
        public string Location { get; set; }
    }
}