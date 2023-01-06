using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModel;
using Services;
using System.Data;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin")]
    public class VacancyController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment env;
        private int pageSizes = 6;

        public VacancyController(IUnitOfWork unitOfWork, StarSecurityDbContext context, IWebHostEnvironment env)
        {
            this._unitOfWork = unitOfWork;
            this._context = context;
            this.env = env;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var model = await _unitOfWork.Vacancy.GetAll(includeProperties: "Category");

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSizes;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Vacancies.Count() / this.pageSizes);

                ViewBag.List = "List Vacancies";
                ViewBag.Controller = "Vacancy";
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
            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Name");

            try
            {
                ViewBag.List = "List Vacancies";
                ViewBag.Controller = "Vacancy";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Create";
                ViewBag.Action = "Create Vacancy";

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
        public async Task<IActionResult> Create(Vacancy model)
        {
            try
            {
                if (model != null)
                {
                    // Kiểm tra Limit Title lại 100 ký tự, để slug không bị full
                    model.Slug = SlugService.Create(model.Title).ToLower();

                    string imageName = "default.jpg";
                    if (model.ImageUpload != null)
                    {
                        string uploadDir = Path.Combine(env.WebRootPath, "media/vacancies");
                        imageName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await model.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                    }

                    var categoryVacancy = await _unitOfWork.Category.GetFirstOrDefault(x => x.Slug == "vacancy-service");

                    Vacancy vacancy = new Vacancy();
                    vacancy.Title = model.Title;
                    vacancy.Slug = model.Slug;
                    vacancy.Description = model.Description;
                    vacancy.Phone = model.Phone;
                    // Get Current user UserName
                    vacancy.UpdatedBy = User.FindFirstValue(ClaimTypes.Name);
                    vacancy.CategoryId = categoryVacancy.Id;
                    vacancy.Noted = model.Noted;
                    vacancy.Image = imageName;
                    vacancy.Status = 1;

                    await _unitOfWork.Vacancy.Add(vacancy);
                    await _unitOfWork.Save();

                    TempData["msg"] = "New Vacancy has been Created.";
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
                var vacancy = await _unitOfWork.Vacancy.GetFirstOrDefault(x => x.Id == id);

                ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Name", vacancy.CategoryId);
                //ViewBag.User = new SelectList(_context.Users.ToList(), "Id", "Name", vacancy.UserId);

                ViewBag.List = "List Vacancies";
                ViewBag.Controller = "Vacancy";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Edit";
                ViewBag.Action = "Edit Vacancy";

                return View(vacancy);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("EditPolicy"))]
        public async Task<IActionResult> Edit(int id, Vacancy model)
        {
            try
            {
                if (model != null)
                {
                    var vacancy = await _unitOfWork.Vacancy.GetFirstOrDefault(x => x.Id == model.Id); //var model = User user             

                    if (vacancy != null)
                    {
                        vacancy.Slug = SlugService.Create(model.Title).ToLower();

                        if (model.ImageUpload != null)
                        {
                            string uploadDir = Path.Combine(env.WebRootPath, "media/vacancies");
                            if (!string.Equals(vacancy.Image, "default.jpg"))
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
                            vacancy.Image = imageName;
                        }

                        vacancy.Title = model.Title;
                        vacancy.Description = model.Description;
                        vacancy.Phone = model.Phone;
                        // Get Current user UserName
                        vacancy.UpdatedBy = User.FindFirstValue(ClaimTypes.Name);
                        vacancy.Noted = model.Noted;
                        vacancy.Status = model.Status;
                        vacancy.UpdatedAt = DateTime.Now;
                        _context.Vacancies.Update(vacancy);
                        await _unitOfWork.Save();

                        TempData["msg"] = "Vacancy has been Updated.";
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
                var model = await _unitOfWork.Vacancy.GetFirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    TempData["msg"] = "This Vacancy does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }
                else
                {
                    if (!string.Equals(model.Image, "default.jpg"))
                    {
                        string uploadDir = Path.Combine(env.WebRootPath, "media/vacancies");
                        string oldImagePath = Path.Combine(uploadDir, model.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _unitOfWork.Vacancy.Remove(model);
                    await _unitOfWork.Save();
                    TempData["msg"] = "This Vacancy has been Deleted.";
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
                Vacancy vacancy = await _unitOfWork.Vacancy.GetFirstOrDefault(x => x.Id == id);
                if (vacancy == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }

                ViewBag.List = "List Vacancies";
                ViewBag.Controller = "Vacancy";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Details";
                ViewBag.Action = "Vacancy Details";

                return View(vacancy);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
    }
}
