using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI
{
    [ApiController]
    [Route("[controller]")]
    public class HelperDraftedPlayersController : ControllerBase
    {
        [HttpGet(Name = "GetDraftedPlayers")]
        public List<object> Get([FromQuery] int draftYear) {
            FactoryAlumni factoryAlumni = new FactoryAlumni();
            factoryAlumni.SetDraftedPlayers(draftYear);
            return factoryAlumni.Drafted;
        }
    }
}                                                                                    
