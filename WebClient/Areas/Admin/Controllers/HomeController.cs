using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

		public HomeController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
			this.env = env;
		}

        public async Task<IActionResult> Index()
		{
			DashBoardViewModel model = new DashBoardViewModel();
			model.Projects = await unitOfWork.Project.GetAll(includeProperties: "User");
			model.Services = await unitOfWork.Service.GetAll(x => x.Status == 1);
			model.Vacancies = await unitOfWork.Vacancy.GetAll(x => x.Status == 1);
			ViewBag.ActiveService = model.Services.Count() + model.Vacancies.Count();
			ViewBag.Resources = Directory.GetFiles(env.WebRootPath, "*", SearchOption.AllDirectories).Length;
			model.Contacts = await unitOfWork.Contact.GetAll();
			ViewBag.Contact = model.Contacts.Count();
            return View(model);
		}
	}
}
