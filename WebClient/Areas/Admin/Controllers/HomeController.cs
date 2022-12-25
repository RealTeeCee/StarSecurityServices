using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin, Employee")]
    public class HomeController : Controller
	{
		public IActionResult Index()
		{            
			return View();
		}
	}
}
