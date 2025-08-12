using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.DbModels;

namespace Data.Repository.Interface
{
    public interface IIndianFoodMacrosRepository
    {
        Task<List<IndianFoodMacros>> GetFoodMacrosByName(string name);
    }
}