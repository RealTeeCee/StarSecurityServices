using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("service/{id?}")]
    public class ServiceController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
