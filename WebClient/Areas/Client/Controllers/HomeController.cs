using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebClient.Models;

namespace WebClient.Areas.Customer.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {            

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Project()
        {
            return View();
        }

        public IActionResult Feature()
        {
            return View();
        }

        public IActionResult FreeQuote()
        {
            return View();
        }

        public IActionResult OurTeam()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}