using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI {
    [ApiController]
    [Route("[controller]")]
    public class Helper29248Controller : ControllerBase {

        [HttpGet(Name = "GetInstrictors")]
        public List<object> Get() {
            Helper29248 helper29248 = new Helper29248();
            return helper29248.Get();
        }
    }
}
