using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI
{
    [ApiController]
    [Route("[controller]")]
    public class Helper29248Controller : ControllerBase
    {

        [HttpGet(Name = "GetInstrictors")]
        public IEnumerable<Instructor> Get()
        {
            return Helper29248.Get();
        }
    }
}
