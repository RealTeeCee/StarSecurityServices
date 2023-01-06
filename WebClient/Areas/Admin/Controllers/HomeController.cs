using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModel;
using System.Data;
using System.IO;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class HomeController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment env;
		private readonly UserManager<User> userManager;
		private readonly StarSecurityDbContext context;

		public HomeController(IUnitOfWork unitOfWork, IWebHostEnvironment env, UserManager<User> userManager, StarSecurityDbContext context)
        {
            this.unitOfWork = unitOfWork;
			this.env = env;
			this.userManager = userManager;
			this.context = context;
		}

		public async Task<IActionResult> Index()
		{
			DashBoardViewModel model = new DashBoardViewModel();
			model.Projects = await unitOfWork.Project.GetAll(includeProperties: "User");	
			model.Services = await unitOfWork.Service.GetAll(x => x.Status == 1);
			model.Vacancies = await unitOfWork.Vacancy.GetAll(x => x.Status == 1);
			model.Branches = await unitOfWork.Branch.GetAll();
			model.Users = await unitOfWork.User.GetAll();

			model.UserDetails = await unitOfWork.UserDetail.GetAll(includeProperties: "User");
			model.UserDetails.OrderBy(x => x.Grade.Value);

			model.Employees = new List<User>();

			foreach (var user in model.Users)
			{
				if (!await userManager.IsInRoleAsync(user, "Admin") && !await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
				{
					model.Employees.Add(user);
				}
			}

			ViewBag.Employees = model.Employees.Count();

			model.Managers = new List<User>();
			foreach (var user in model.Users)
			{
				if (await userManager.IsInRoleAsync(user, "Admin") || await userManager.IsInRoleAsync(user, "GeneralAdmin") || await userManager.IsInRoleAsync(user, "SuperAdmin"))
                {
                    model.Managers.Add(user);
                }
            }
			ViewBag.Managers = model.Managers.Count();

            ViewBag.Users = model.Managers.Count() + model.Employees.Count();
            ViewBag.Branches = model.Branches.Count();
			ViewBag.ActiveService = model.Services.Count() + model.Vacancies.Count();
			ViewBag.Resources = Directory.GetFiles(env.WebRootPath, "*", SearchOption.AllDirectories).Length;
			model.Contacts = await unitOfWork.Contact.GetAll();
			ViewBag.Contact = model.Contacts.Count();
            return View(model);
		}
	}
}
