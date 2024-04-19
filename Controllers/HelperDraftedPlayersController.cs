using Microsoft.AspNetCore.Mvc;

namespace CView.BaseballAPI
{
    [ApiController]
    [Route("[controller]")]
    public class HelperDraftedPlayersController : ControllerBase
    {

        [HttpGet(Name = "GetLLWS")]
        public dynamic Get()
        {
            //return HelperLLWS.Get(HttpContext.Request.Query);
        }
    }
}                                                                                    
