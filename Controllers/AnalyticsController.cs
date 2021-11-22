using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFM.Services;

namespace PFM.Controllers
{
    [ApiController]
    [Route("/spending-analytics")]
    public class AnalyticsController : ControllerBase{

       
        private readonly ILogger<AnalyticsController> _logger;
        private readonly IAnalyticsService _analyticsService;


        public AnalyticsController(ILogger<AnalyticsController> logger,  IAnalyticsService analyticsService)
        {
            _logger = logger;
            _analyticsService = analyticsService;
        }

        [HttpGet]
        public IActionResult GetSpendingAnalytics([FromQuery] string catcode, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string direction){
            var analytics = _analyticsService.GetAnalytics(catcode, startDate, endDate, direction);
            return Ok(analytics);
        }
    }
}