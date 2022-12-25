using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
