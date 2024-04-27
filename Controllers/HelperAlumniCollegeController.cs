using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI {
    [ApiController]
    [Route("[controller]")]
    public class HelperAlumniCollegeController : ControllerBase {
        [HttpGet(Name = "AlumniCollege")]
        public List<object> Get() {
            FactoryAlumni factoryAlumni = new FactoryAlumni();
            factoryAlumni.SetCollegeData();
            return factoryAlumni.College;
        }
    }
}
