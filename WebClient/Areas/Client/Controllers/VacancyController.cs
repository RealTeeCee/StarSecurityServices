using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    public class VacancyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
