using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI
{
    [ApiController]
    [Route("[controller]")]
    public class HelperAlumniCommitmentsController : ControllerBase
    {
        [HttpGet(Name = "AlumniCommitments")]
        public List<object> Get([FromQuery] int currentYear) {
            FactoryAlumni factoryAlumni = new FactoryAlumni();
            factoryAlumni.SetCommitmentsData(currentYear);
            return factoryAlumni.Commitments;
        }
    }
}                                                                                    
