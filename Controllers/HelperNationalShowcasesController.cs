using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI {
    [ApiController]
    [Route("[controller]")]
    public class HelperNationalShowcasesController : ControllerBase {
        [HttpGet(Name = "Events")]
        public List<object> Get([FromQuery] string ageGroup, [FromQuery] string st) {
            HelperNationalShowcases helperNationalShowcases = new HelperNationalShowcases();
            return helperNationalShowcases.GetShowcases(ageGroup, st);
        }
    }
}
