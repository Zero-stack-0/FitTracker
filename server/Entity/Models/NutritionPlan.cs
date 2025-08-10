using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class NutritionPlan
    {
        public string dietary_preference { get; set; }
        public string location { get; set; }
        public List<Meal> meals { get; set; }
    }
}