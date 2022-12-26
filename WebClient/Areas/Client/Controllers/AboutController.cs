using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
    }
}
