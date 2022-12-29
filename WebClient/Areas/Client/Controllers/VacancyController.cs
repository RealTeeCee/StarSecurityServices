using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("vacancy")]
    public class VacancyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
