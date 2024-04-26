using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI
{
    [ApiController]
    [Route("[controller]")]
    public class HelperEventRosterController : ControllerBase
    {
        [HttpGet(Name = "EventRoster")]
        public List<object> Get([FromQuery] string eventID)
        {
            FactoryRosters factoryRosters = new FactoryRosters();
            factoryRosters.SetPreseasonRoster(eventID);
            return factoryRosters.PreSeasonRoster;
        }
    }
}
