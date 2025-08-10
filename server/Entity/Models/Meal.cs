using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Meal
    {
        public string meal_name { get; set; }
        public string recommended_for { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public int total_calories { get; set; }
        public Macros macros { get; set; }
        public string preparation_steps { get; set; }
    }
}