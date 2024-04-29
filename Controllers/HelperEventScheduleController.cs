using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI {
    [ApiController]
    [Route("[controller]")]
    public class HelperEventScheduleController : ControllerBase {
        [HttpGet(Name = "HelperEventSchedule")]
        public List<object> Get() {
            HelperEventSchedule helperEventSchedule = new HelperEventSchedule();
            return helperEventSchedule.GetEvents();
        }
    }
}
