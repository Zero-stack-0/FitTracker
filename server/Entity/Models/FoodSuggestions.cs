using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class FoodSuggestions
    {
        public List<string> protein { get; set; }
        public List<string> carbs { get; set; }
        public List<string> fats { get; set; }
    }
}