using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("our-team")]
    public class OurTeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
