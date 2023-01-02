using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Models;
using DataAccess.Repositories.IRepositories;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("project")]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork)
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

                foreach (var projects in modelNoBranch.CategoriesBranches)
                {
                    var project = await unitOfWork.Project.GetFirstOrDefault(x => x.Service.Category.Id == projects.CategoryId, includeProperties: "Service.Category,Service");
                    if (project != null)
                    {
                        modelNoBranch.Projects.Add((Project)project);
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

            foreach (var projects in model.CategoriesBranches)
            {
                var project = await unitOfWork.Project.GetFirstOrDefault(x => x.Service.Category.Id == projects.CategoryId, includeProperties: "Service.Category,Service");
                if (project != null)
                {
                    model.Projects.Add((Project)project);
                }
            }

            return View(model);
        }
        [HttpGet("{projectSlug?}")]
        public async Task<IActionResult> DetailProject(string serviceSlug, string projectSlug)
        {
            var project = await unitOfWork.Project.GetFirstOrDefault(x => x.Slug == projectSlug);
            if (project == null)
            {
                return RedirectToAction("PageNotFound", "Error", new { area = "Client" });
            }
            var projectRelated = await unitOfWork.Project.GetAll(x => x.Id != project.Id && x.Service.Slug == "serviceSlug", includeProperties: "Service");

            ViewBag.ProjectRelated = projectRelated;

            return View(project);
        }
    }
}
