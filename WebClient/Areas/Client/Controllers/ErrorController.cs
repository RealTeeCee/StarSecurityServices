using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("error")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("not-found")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
