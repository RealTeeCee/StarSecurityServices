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

        public async Task<IActionResult> Index(int? localeId)
        {
            // vo day
            HomeViewModel model = new HomeViewModel();
            model.Categories = (List<global::Models.Category>)await unitOfWork.Category.GetAll();

            return View(model);
        }

        public async Task<IActionResult> CreateSessionForBrachId(int selectBranch)
        {
            try
            {
                var branch = await unitOfWork.Branch.GetFirstOrDefault(x => x.Id == selectBranch, includeProperties: "");

                if (branch != null)
                {
                    //CookieOptions cookieOptions = new CookieOptions()
                    //{
                    //    Expires = DateTime.Now.AddDays(30),
                    //};
                    //Response.Cookies.Append("branchId", selectBranch.ToString(), cookieOptions);
                    //Response.Cookies.Append("branchName", branch.Name, cookieOptions);
                    HttpContext.Session.SetString("branchId", selectBranch.ToString());
                    HttpContext.Session.SetString("branchName", branch.Name);
                    HttpContext.Session.SetString("branchEmail", branch.Email);
                    HttpContext.Session.SetString("branchPhone", branch.Phone);
                    HttpContext.Session.SetString("branchTimeOpen", branch.TimeOpen);                    
                }

                return RedirectToAction("Index", new { localeId = selectBranch });
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Customer" });

            }

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