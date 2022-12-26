using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
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
            // vo day
            HomeViewModel model = new HomeViewModel();
            model.Categories = (List<global::Models.Category>)await unitOfWork.Category.GetAll();

            return View(model);
        }

        public async Task<IActionResult>  About()
        {
            return View();
        }

        public async Task<IActionResult>  Project()
        {
            return View();
        }

        public async Task<IActionResult> Feature()
        {
            return View();
        }

        public async Task<IActionResult> FreeQuote()
        {
            return View();
        }

        public async Task<IActionResult> OurTeam()
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