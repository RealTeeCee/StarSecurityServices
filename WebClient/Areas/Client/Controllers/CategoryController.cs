using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace WebClient.Areas.Customer.Controllers
{
    [Area("Client")]
    [Route("category")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //https://localhost:7273/client/category
        //trang này sd trang Our Team -> show 4 cards
        public IActionResult Index()
        {
            //Show toan bo cate, tra ve view Category
            var model = unitOfWork.Category.GetAll();
            return View(model);
        }
        // nút Readmore
        //https://localhost:7273/category/{categorySlug}

        [HttpGet("{categorySlug?}")]
        public IActionResult Detail(string categorySlug)
        {
            //Show toan bo cate, tra ve view Category
            // Nhớ trả về View Detail trong folder Category

            return View("");
        }

    }
}
