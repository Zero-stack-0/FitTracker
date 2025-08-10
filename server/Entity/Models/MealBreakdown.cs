using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class MealBreakdown
    {
        public MealDetail breakfast { get; set; }
        public MealDetail lunch { get; set; }
        public MealDetail dinner { get; set; }
        public MealDetail snacks { get; set; }
    }
}