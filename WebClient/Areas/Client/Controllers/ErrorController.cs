using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
