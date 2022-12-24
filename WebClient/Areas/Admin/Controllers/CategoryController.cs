using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModel;
using Services;
using System.Linq;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin, Employee")]
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

        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {                               
                var model = await _unitOfWork.Category.GetAll();
                

                int pageSize = 6;
                ViewBag.PageNumber = p;
                ViewBag.PageRange = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Categories.Count() / pageSize);                

                ViewBag.List = "List Categories";
                ViewBag.Controller = "Category";
                ViewBag.AspAction = "Index";

                return View(model.Skip((p - 1) * pageSize).Take(pageSize));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }            
        }
        [Authorize(Policy = ("CreatePolicy"))]
        public async Task<IActionResult> Create()
        {
            
            try
            {
                ViewBag.List = "List Categories";
                ViewBag.Controller = "Category";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Create";
                ViewBag.Action = "Create Category";

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [Authorize(Policy = ("CreatePolicy"))]
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

                    string imageName = "default.jpg";
                    if(category.ImageUpload != null)
                    {
                        string uploadDir = Path.Combine(env.WebRootPath, "media/categories");
                        imageName = Guid.NewGuid().ToString() + "_" + category.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await category.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                    }

                    category.Image = imageName;

                    await _unitOfWork.Category.Add(category);
                    await _unitOfWork.Save();
                    TempData["msg"] = "Category has been Created.";
                    TempData["msg_type"] = "success";

                }
           
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            Category category = await _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            try
            {
                if (category == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }
                ViewBag.List = "List Categories";
                ViewBag.Controller = "Category";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Details";
                ViewBag.Action = "Category Details";
                return View(category);

            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }
        }
        [Authorize(Policy = ("EditPolicy"))]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id); //var model = User user             

                ViewBag.List = "List Categories";
                ViewBag.Controller = "Category";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Edit";
                ViewBag.Action = "Edit Category";
                return View(category);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("EditPolicy"))]
        public async Task<IActionResult> Edit(Category model)
        {
            try
            {

                if (model != null)
                {
                    var category = await _unitOfWork.Category.GetFirstOrDefault(x => x.Id == model.Id); //var model = User user             

                    if (category != null)
                    {
                        category.Name = model.Name;
                        category.Slug = model.Slug;                        
                        if (model.ImageUpload != null)
                        { 
                            string uploadDir = Path.Combine(env.WebRootPath, "media/categories");
                            if(!string.Equals(category.Image, "default.jpg")){
                                string oldImagePath = Path.Combine(uploadDir, category.Image);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }
                            string imageName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                            string filePath = Path.Combine(uploadDir, imageName);
                            FileStream fs = new FileStream(filePath, FileMode.Create);
                            await model.ImageUpload.CopyToAsync(fs);
                            fs.Close();
                            category.Image = imageName;
                        }                        
                        category.UpdatedAt = DateTime.Now;
                        _context.Categories.Update(category);
                        await _unitOfWork.Save();

                        TempData["msg"] = "Category has been Updated.";
                        TempData["msg_type"] = "success";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [Authorize(Policy = ("DeletePolicy"))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    TempData["msg"] = "Category does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }
                else
                {
                    if (!string.Equals(model.Image, "default.jpg"))
                    {
                        string uploadDir = Path.Combine(env.WebRootPath, "media/categories");
                        string oldImagePath = Path.Combine(uploadDir, model.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _unitOfWork.Category.Remove(model);
                    await _unitOfWork.Save();
                    TempData["msg"] = "Category has been Deleted.";
                    TempData["msg_type"] = "success";
                    return RedirectToAction("Index");
                }              
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }

        }
    }
}
