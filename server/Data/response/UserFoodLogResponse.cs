using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Response
{
    public class UserFoodLogWeekHistory
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double? TotalCaloriesLogged { get; set; }
        public double? TotalProtienLogged { get; set; }
        public double? TotalCarbLogged { get; set; }
        public double? TotalFatLogged { get; set; }
        public List<UserFoodLogDayHistory> UserFoodLogDayHistory { get; set; }
    }

    public class UserFoodLogDayHistory
    {
        public DateTime? CreatedDate { get; set; }
        public double? TotalCaloriesLogged { get; set; }
        public double? TotalProtienLogged { get; set; }
        public double? TotalCarbLogged { get; set; }
        public double? TotalFatLogged { get; set; }
        public bool Expanded { get; set; } = false;
        public List<FoodLogByType> FoodLogByTypeResponse { get; set; }
    }

    public class FoodLogByType
    {
        public string TimeOfTheDay { get; set; }
        public List<UserFoodLogResponse> UserFoodLogResponseList { get; set; }
    }
    public class UserFoodLogResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FoodName { get; set; }
        public double? CaloriesLogged { get; set; }
        public double? ProtienLogged { get; set; }
        public double? CarbLogged { get; set; }
        public double? FatLogged { get; set; }
        public string TimeOfTheDay { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}