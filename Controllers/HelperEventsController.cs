using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI
{
    [ApiController]
    [Route("[controller]")]
    public class HelperEventsController : ControllerBase
    {
        [HttpGet(Name = "Events")]
        public List<object> Get([FromQuery] string eventID)
        {
            FactoryEvents factoryEvents = new FactoryEvents();
            factoryEvents.SetAllEvents();
            return factoryEvents.EventsByMonth;
        }
    }
}
