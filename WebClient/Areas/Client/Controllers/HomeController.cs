using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Models;
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
            if (localeId == null)
            {
                //mac dinh HCM
                HomeViewModel modelNoBranch = new HomeViewModel();
                modelNoBranch.CategoriesBranches = (List<CategoryBranch>)await unitOfWork.CategoryBranch.GetAll(x => x.BranchId == 1, includeProperties: "Category,Branch");
                modelNoBranch.Projects = new List<Project>();

                var projects = await unitOfWork.Project.GetAll(includeProperties: "Service.Category,Service");
                
                foreach (var project in projects)
                {
                    foreach (var item in modelNoBranch.CategoriesBranches)
                    {
                        if(project.Service.CategoryId == item.Category.Id)
                        {
                            modelNoBranch.Projects.Add((Project)project);
                        }
                    }                   
                }
                //Lay ra tat ca project thuoc service co categorybranch.category.BranchId = 1
                //project.service.category.Id == categorybranch.category.Id && categoryBranch.Category.BranchId == localeId
                
                return View(modelNoBranch);
            }
            // vo day
            HomeViewModel model = new HomeViewModel();            
            model.CategoriesBranches = (List<global::Models.CategoryBranch>)await unitOfWork.CategoryBranch.GetAll(x => x.BranchId == localeId, includeProperties: "Category,Branch");
            model.Projects = new List<Project>();            

            var allProjects = await unitOfWork.Project.GetAll(includeProperties: "Service.Category,Service");
            //Project in Service in SecurityService in HCM , HN
            foreach (var project in allProjects)
            {
                foreach (var item in model.CategoriesBranches)
                {
                    if (project.Service.CategoryId == item.Category.Id)
                    {
                        model.Projects.Add((Project)project);
                    }
                }
            }

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
                    HttpContext.Session.SetString("branchFacebook", branch.Facebook);
                    HttpContext.Session.SetString("branchTwitter", branch.Twitter);
                    HttpContext.Session.SetString("branchYoutube", branch.Youtube);
                    HttpContext.Session.SetString("branchInstagram", branch.Instagram);
                }

                return RedirectToAction("Index", new { localeId = selectBranch });
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Client" });

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