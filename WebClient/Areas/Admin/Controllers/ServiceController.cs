using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using System.Drawing.Printing;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin")]
    public class ServiceController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment env;
        private int pageSizes = 6;

        public ServiceController(IUnitOfWork unitOfWork, StarSecurityDbContext context, IWebHostEnvironment env)
        {
            this._unitOfWork = unitOfWork;
            this._context = context;
            this.env = env;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var model = await _unitOfWork.Service.GetAll(includeProperties: "Category");

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSizes;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Vacancies.Count() / this.pageSizes);

                ViewBag.List = "List Services";
                ViewBag.Controller = "Service";
                ViewBag.AspAction = "Index";

                return View(model.Skip((p - 1) * this.pageSizes).Take(this.pageSizes));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [Authorize(Policy = ("CreatePolicy"))]
        public async Task<IActionResult> Create()
        {
            // User check Session, lay Id User đang đăng nhập
            ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Slug != "vacancy-service").ToList(), "Id", "Name");
            
            try
            {
                ViewBag.List = "List Services";
                ViewBag.Controller = "Service";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Create";
                ViewBag.Action = "Create Service";

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("CreatePolicy"))]
        public async Task<IActionResult> Create(Service model)
        {
            try
            {
                if (model != null)
                {
                    // Kiểm tra Limit Name lại 100 ký tự, để slug không bị full
                    model.Slug = SlugService.Create(model.Name).ToLower();

                    //Kiểm tra slug exists trên db hay chưa
                    var slug = await _unitOfWork.Service.GetFirstOrDefault(x => x.Slug == model.Slug && x.CategoryId == model.CategoryId);
                    if (slug != null)
                    {
                        TempData["msg"] = "This Service has been exists.";
                        TempData["msg_type"] = "danger";

                        ViewBag.List = "List Services";
                        ViewBag.Controller = "Service";
                        ViewBag.AspAction = "Index";
                        ViewBag.AspSubAction = "Create";
                        ViewBag.Action = "Create Service";

                        ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Slug != "vacancy-service").ToList(), "Id", "Name", model.CategoryId);
                        return View(model);
                    }

                    string imageName = "default.jpg";
                    if (model.ImageUpload != null)
                    {
                        foreach (var item in ModelState)
                        {
                            if (item.Key == "ImageUpload")
                            {
                                if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                                {
                                    TempData["msg"] = "Only accept extension image: .jpg, .png ";
                                    TempData["msg_type"] = "danger";

                                    ViewBag.List = "List Services";
                                    ViewBag.Controller = "Service";
                                    ViewBag.AspAction = "Index";
                                    ViewBag.AspSubAction = "Create";
                                    ViewBag.Action = "Create Service";

                                    ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Slug != "vacancy-service").ToList(), "Id", "Name", model.CategoryId);
                                    return View(model);
                                }
                            }
                        }

                        string uploadDir = Path.Combine(env.WebRootPath, "media/services");
                        imageName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await model.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                    }

                    Service service = new Service();
                    service.Name = model.Name;
                    service.Slug = model.Slug;
                    service.Description = model.Description;
                    service.ShortDescription = model.ShortDescription;

                    service.CategoryId = model.CategoryId;
                    service.Image = imageName;
                    service.Status = 1;
                    // Xử lý Thêm UserId cho Service

                    // Get Current user UserName
                    service.UpdatedBy = User.FindFirstValue(ClaimTypes.Name);


                    await _unitOfWork.Service.Add(service);
                    await _unitOfWork.Save();

                    TempData["msg"] = "New Service has been Created.";
                    TempData["msg_type"] = "success";

                }
                return RedirectToAction("Create");
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
                var service = await _unitOfWork.Service.GetFirstOrDefault(x => x.Id == id);
                ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Slug != "vacancy-service").ToList(), "Id", "Name", service.CategoryId);

                ViewBag.List = "List Services";
                ViewBag.Controller = "Service";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Edit";
                ViewBag.Action = "Edit Service";

                return View(service);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("EditPolicy"))]
        public async Task<IActionResult> Edit(int id, Service model)
        {
            try
            {
                if (model != null)
                {
                    var service = await _unitOfWork.Service.GetFirstOrDefault(x => x.Id == model.Id);

                    if (service != null)
                    {
                        service.Slug = SlugService.Create(model.Name).ToLower();

                        var serviceDb = await _unitOfWork.Service.GetAll(x => x.Id != id);

                        foreach (var itemDb in serviceDb)
                        {
                            //Kiểm tra slug exists trên db hay chưa, ngoại trừ Id hiện tại và slug trùng phải cùng Category
                            var slug = await _context.Services.Where(x => x.Id != id).FirstOrDefaultAsync(p => p.Slug == service.Slug && p.CategoryId == model.CategoryId);

                            if (slug != null)
                            {
                                ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Slug != "vacancy-service").ToList(), "Id", "Name", service.CategoryId);
                                TempData["msg"] = "This Service already exists in this category";
                                TempData["msg_type"] = "danger";

                                ViewBag.List = "List Services";
                                ViewBag.Controller = "Service";
                                ViewBag.AspAction = "Index";
                                ViewBag.AspSubAction = "Edit";
                                ViewBag.Action = "Edit Service";

                                return View(service);
                            }
                        }

                        if (model.ImageUpload != null)
                        {
                            // Nếu ko Validate Hợp lệ
                            foreach (var item in ModelState)
                            {
                                if (item.Key == "ImageUpload")
                                {
                                    if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                                    {
                                        TempData["msg"] = "Only accept extension image: .jpg, .png ";
                                        TempData["msg_type"] = "danger";

                                        ViewBag.List = "List Services";
                                        ViewBag.Controller = "Service";
                                        ViewBag.AspAction = "Index";
                                        ViewBag.AspSubAction = "Edit";
                                        ViewBag.Action = "Edit Service";

                                        return View(service);
                                    }
                                }
                            }


                            string uploadDir = Path.Combine(env.WebRootPath, "media/services");
                            if (!string.Equals(service.Image, "default.jpg"))
                            {
                                string oldImagePath = Path.Combine(uploadDir, model.Image);
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
                            service.Image = imageName;
                        }

                        service.Name = model.Name;
                        service.ShortDescription = model.ShortDescription;
                        service.Description = model.Description;
                        service.ShortDescription = model.ShortDescription;

                        // Get Current user UserName
                        service.UpdatedBy = User.FindFirstValue(ClaimTypes.Name);
                        service.CategoryId = model.CategoryId;

                        service.Status = model.Status;
                        service.UpdatedAt = DateTime.Now;
                        _context.Services.Update(service);
                        await _unitOfWork.Save();

                        TempData["msg"] = "This Service has been Updated.";
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
                var model = await _unitOfWork.Service.GetFirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    TempData["msg"] = "This Service does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }
                else
                {
                    if (!string.Equals(model.Image, "default.jpg"))
                    {
                        string uploadDir = Path.Combine(env.WebRootPath, "media/services");
                        string oldImagePath = Path.Combine(uploadDir, model.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _unitOfWork.Service.Remove(model);
                    await _unitOfWork.Save();

                    TempData["msg"] = "This Service has been Deleted.";
                    TempData["msg_type"] = "success";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                Service service = await _unitOfWork.Service.GetFirstOrDefault(x => x.Id == id, includeProperties: "Category");
                if (service == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }

                ViewBag.List = "List Services";
                ViewBag.Controller = "Service";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Details";
                ViewBag.Action = "Service Details";

                return View(service);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
    }
}
