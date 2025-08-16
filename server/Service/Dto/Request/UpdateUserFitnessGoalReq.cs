using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Entity.Enums;

namespace Service.Dto.Request
{
    public class UpdateUserFitnessGoalReq
    {
        public string FullName { get; set; }
        public string? UserId { get; set; }
        public double? Weight { get; set; }
        public ActivityLevel? ActivityLevel { get; set; }
        public DIET_TYPE? DietType { get; set; }
        public FintnessGoal? FitnessGoal { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public Gender? Gender { get; set; }
        public bool CanUpdateDietPlan { get; set; }
    }
}