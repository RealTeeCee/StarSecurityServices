using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using System.Collections.Generic;

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
        public async Task<IActionResult> Index(int? localeId)
        {
            //Show toan bo cate, tra ve view Category
            if(localeId == null)
            {
                CategoryViewModel modelNoBranch = new CategoryViewModel();
                modelNoBranch.Categories = (List<Category>)await unitOfWork.Category.GetAll();
                return View(modelNoBranch);
            }
            CategoryViewModel model = new CategoryViewModel();
            model.CategoriesBranches = (List<CategoryBranch>)await unitOfWork.CategoryBranch.GetAll(x => x.BranchId == localeId,includeProperties:"Category,Branch");

            return View(model);
        }
        // nút Readmore
        //https://localhost:7273/category/{categorySlug}

        [HttpGet("{categorySlug?}")]
        public async Task<IActionResult> Detail(string categorySlug)
        {
            var category = await unitOfWork.Category.GetFirstOrDefault(x => x.Slug == categorySlug);
            long categoryId = category.Id;

            //Show toan bo cate, tra ve view Category
            ServiceViewModel model = new ServiceViewModel();
            model.Services = (List<Service>)await unitOfWork.Service.GetAll(x=> x.CategoryId == categoryId, includeProperties:"Category");
            // Nhớ trả về View Detail trong folder Category

            
            string categoryName = category.Name;
            ViewBag.CategoryName = categoryName;
            ViewBag.CategorySlug = categorySlug;

            return View(model);
        }

        [HttpGet("{categorySlug?}/{serviceSlug?}")]
        public async Task<IActionResult> DetailService(string categorySlug, string serviceSlug)
        {
            var category = await unitOfWork.Category.GetFirstOrDefault(x => x.Slug == categorySlug);
            string categoryName = category.Name;
            ViewBag.CategoryName = categoryName;
            

            var service = await unitOfWork.Service.GetFirstOrDefault(x => x.Slug == serviceSlug);
            string serviceName = service.Name;
            ViewBag.ServiceName = serviceName;
            

            //Show toan bo cate, tra ve view Category            
            // Nhớ trả về View Detail trong folder Category

            return View(service);
        }

    }
}
