using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI {
    [ApiController]
    [Route("[controller]")]
    public class HelperBaseballIsBackEventsController : ControllerBase {
        [HttpGet(Name = "HelperBaseballIsBackEvents")]
        public List<object> Get([FromQuery] string ageGroup) {
            HelperBaseballIsBackEvents helperBaseballIsBackEvents = new HelperBaseballIsBackEvents();
            return helperBaseballIsBackEvents.GetEvents(ageGroup);
        }
    }
}
