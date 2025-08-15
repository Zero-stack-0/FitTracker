using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;

namespace Data.response
{
    public class DashboardResponse
    {
        public double? TotalCaloriesLogged { get; set; }
        public double? TotalProtienLogged { get; set; }
        public double? TotalCarbLogged { get; set; }
        public double? TotalFatLogged { get; set; }
        public MacroTargets? MacroTargets { get; set; }
    }
}