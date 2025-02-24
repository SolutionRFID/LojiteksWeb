using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LojiteksWeb.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        // Örnek statik veri
        [HttpPost]
        public async Task<IActionResult> GetPieChartDataAjax()
        {
            var data = new
            {
                series = new List<int> { 44, 55, 41, 17, 15, 1 },
                labels = new List<string> { "Series 1", "Series 2", "Series 3", "Series 4", "Series 5", "tst" }
            };

            return Ok(data);
        }
    }
}
