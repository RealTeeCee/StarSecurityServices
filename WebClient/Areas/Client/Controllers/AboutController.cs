using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("about")]
    public class AboutController : Controller
    {
        public IActionResult Index(int? id)
        {
            return View();
        }
    }
}
