using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI {
    [ApiController]
    [Route("[controller]")]
    public class HelperEventGeoLocationController : ControllerBase {
        [HttpGet(Name = "GetEventGeoLocation")]
        public Dictionary<string, object> Get([FromQuery] string zip, string lat, string lng) {
            FactoryEvents factoryEvents = new FactoryEvents();
            var result = factoryEvents.GetEventZipsByLocation(zip, lat, lng);
            return result;
        }
    }
}
