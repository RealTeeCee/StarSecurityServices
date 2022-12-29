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
<<<<<<< HEAD
            if(localeId == null)
            {
                //mac dinh HCM
                HomeViewModel modelNoBranch = new HomeViewModel();
                modelNoBranch.CategoriesBranches = (List<global::Models.CategoryBranch>)await unitOfWork.CategoryBranch.GetAll(x=>x.BranchId == 1,includeProperties:"Category,Branch");
                return View(modelNoBranch);
            }
            // vo day
=======
>>>>>>> 7d9819a96615b0925c9f2ee0e4321aa8d7a22c50
            HomeViewModel model = new HomeViewModel();
            model.CategoriesBranches = (List<global::Models.CategoryBranch>)await unitOfWork.CategoryBranch.GetAll(x=>x.BranchId == localeId, includeProperties: "Category,Branch");

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
                    HttpContext.Session.SetString("branchAddress", branch.Address);
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