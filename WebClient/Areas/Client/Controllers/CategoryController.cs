using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Customer.Controllers
{
    [Area("Client")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
