using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("free-quote")]
    public class FreeQuoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
