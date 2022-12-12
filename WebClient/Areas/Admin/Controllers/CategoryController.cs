using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModel;
using Services;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment env;

        public CategoryController(IUnitOfWork unitOfWork, StarSecurityDbContext context, IWebHostEnvironment env)
        {
            this._unitOfWork = unitOfWork;
            _context = context;
            this.env = env;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                CategoryVM categoryVM = new CategoryVM()
                {
                    Categories = (List<Category>)await _unitOfWork.Category.GetAll()
                };
                return View(categoryVM);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }            
        }
        public async Task<IActionResult> Create()
        {
            
            try
            {                                           
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                if (category != null)
                {
                    category.Slug = SlugService.Create(category.Name).ToLower();                    

                    //Kiểm tra slug exists trên db hay chưa
                    var slug = await _unitOfWork.Category.GetFirstOrDefault(c => c.Slug == category.Slug);
                    if (slug != null)
                    {
                        ModelState.AddModelError("", "The category already exists !");
                        return View(category);
                    }                 


                    await _unitOfWork.Category.Add(category);
                    await _unitOfWork.Save();
                    TempData["msg"] = "User has been Created.";
                    TempData["msg_type"] = "success";

                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> Update(int id, string? msg)
        {
            try
            {
                if (msg != null)
                {
                    ViewBag.msg = msg;
                }
                var data = await _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
                return View(data);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Customer" });

            }

        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            try
            {
                var cate = await _unitOfWork.Category.GetAll(x => x.Id != category.Id);
                if (cate.Count() == 0)
                {
                    var temp = await _unitOfWork.Category.GetFirstOrDefault(x => x.Id == category.Id);
                    category.Image = temp.Image;
                    category.UpdatedAt = DateTime.Now;
                    _unitOfWork.Category.Update(category);
                    await _unitOfWork.Save();
                    return RedirectToAction("Update", new { msg = "Categories has been Updated." });

                }
                else
                {
                    foreach (var item in cate)
                    {
                        var temp = await _unitOfWork.Category.GetFirstOrDefault(x => x.Id == category.Id);
                        category.Image = temp.Image;
                        category.UpdatedAt = DateTime.Now;
                        _unitOfWork.Category.Update(category);
                        await _unitOfWork.Save();
                        return RedirectToAction("Update", new { msg = "Categories has been Updated." });
                    }
                }
                return null;
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Customer" });
            }
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var data = await _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

                _unitOfWork.Category.Remove(data);
                await _unitOfWork.Save();
                return Json(new { success = true });
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Customer" });

            }

        }
    }
}
