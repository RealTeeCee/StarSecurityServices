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

        //https://localhost:7273/category
        //trang này sd trang Our Team -> show 4 cards
        public async Task<IActionResult> Index()
        {
            //Show toan bo cate, tra ve view Category
            CategoryViewModel model = new CategoryViewModel();
            model.Categories = (List<Category>)await unitOfWork.Category.GetAll();
            return View(model);
        }
        // nút Readmore
        //https://localhost:7273/category/{categorySlug}

        [HttpGet("{categorySlug?}")]
        public async Task<IActionResult> Detail(string categorySlug)
        {
            try
            {
                var category = await unitOfWork.Category.GetFirstOrDefault(x => x.Slug == categorySlug);
                if(category == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Client" });
                }

                long categoryId = category.Id;

                //Show toan bo cate, tra ve view Category
                ServiceViewModel model = new ServiceViewModel();
                if(categorySlug != "vacancy-service")
                {
                    model.Services = (List<Service>)await unitOfWork.Service.GetAll(x => x.CategoryId == categoryId, includeProperties: "Category");
                }
                else
                {
                    model.Vacancies = (List<Vacancy>)await unitOfWork.Vacancy.GetAll(x => x.CategoryId == categoryId, includeProperties: "Category");
                }

                // Nhớ trả về View Detail trong folder Category

                string categoryName = category.Name;
                ViewBag.CategoryName = categoryName;
                ViewBag.CategorySlug = categorySlug;

                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Client" });
            }
        }

        [HttpGet("{categorySlug?}/{serviceSlug?}")]
        public async Task<IActionResult> DetailService(string categorySlug, string serviceSlug)
        {
            try
            {
                //Show toan bo cate, tra ve view Category            
                // Nhớ trả về View Detail trong folder Category
                var category = await unitOfWork.Category.GetFirstOrDefault(x => x.Slug == categorySlug);
                if (category == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Client" });
                }

                if(categorySlug != "vacancy-service")
                {
                    var service = await unitOfWork.Service.GetFirstOrDefault(x => x.Slug == serviceSlug);
                    if (service == null)
                    {
                        return RedirectToAction("Index", "Error", new { area = "Client" });
                    }

                    var serviceRelated = await unitOfWork.Service.GetAll(x => x.Id != service.Id && x.CategoryId == category.Id);
                    ViewBag.ServiceRelated = serviceRelated;

                    ViewBag.CategoryName = category.Name;
                    ViewBag.CategorySlug = category.Slug;

                    return View(service);
                }
                else
                {
                    var service = await unitOfWork.Vacancy.GetFirstOrDefault(x => x.Slug == serviceSlug);
                    if (service == null)
                    {
                        return RedirectToAction("Index", "Error", new { area = "Client" });
                    }

                    var serviceRelated = await unitOfWork.Vacancy.GetAll(x => x.Id != service.Id && x.CategoryId == category.Id);
                    ViewBag.ServiceRelated = serviceRelated;

                    ViewBag.CategoryName = category.Name;
                    ViewBag.CategorySlug = category.Slug;

                    return View("DetailVacancy", service);
                }


            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Client" });
            }
        }

    }
}
