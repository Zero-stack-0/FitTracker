using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace WebService.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FoodMacroController : ControllerBase
    {
        private readonly IFoodMacrosService _foodMacrosService;
        public FoodMacroController(IFoodMacrosService foodMacrosService)
        {
            _foodMacrosService = foodMacrosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoodMacrosByName([FromQuery] string name)
        {
            return Ok(await _foodMacrosService.GetFoodMacrosByName(name));
        }
    }
}