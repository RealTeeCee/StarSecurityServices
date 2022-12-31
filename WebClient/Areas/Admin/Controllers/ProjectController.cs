using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Data;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin, Employee")]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly StarSecurityDbContext context;

        private int pageSize = 6;

        public ProjectController(IUnitOfWork unitOfWork, StarSecurityDbContext context)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
        }
        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var listProject = await unitOfWork.Project.GetAll();

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Projects.Count() / this.pageSize);

                ViewBag.List = "List Projects";
                ViewBag.Controller = "Project";
                ViewBag.AspAction = "Index";

                return View(listProject.Skip((p - 1) * this.pageSize).Take(this.pageSize));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
    }
}
