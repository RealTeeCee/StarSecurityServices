using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("contact")]
    public class ContactController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
