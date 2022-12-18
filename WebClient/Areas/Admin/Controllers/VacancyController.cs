using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModel;
using Services;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VacancyController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment env;
        private int pageSizes = 6;

        public VacancyController(IUnitOfWork unitOfWork, StarSecurityDbContext context, IWebHostEnvironment env)
        {
            this._unitOfWork = unitOfWork;
            _context = context;
            this.env = env;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var model = await _unitOfWork.Vacancy.GetAll(includeProperties: "User,Category,Branch");

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSizes;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Vacancies.Count() / this.pageSizes);

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> Create()
        {
            // User check Session, lay Id User đang đăng nhập
            ViewBag.Branch = new SelectList(_context.Branches.ToList(), "Id", "Name");
            ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Slug != "vacancy-service").ToList(), "Id", "Name");

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
        public async Task<IActionResult> Create(Vacancy model)
        {
            try
            {
                if (model != null)
                {
                    // Kiểm tra Limit Title lại 100 ký tự, để slug không bị full
                    model.Slug = SlugService.Create(model.Title).ToLower();

                    //Kiểm tra slug exists trên db hay chưa
                    var slug = await _unitOfWork.Vacancy.GetFirstOrDefault(x => x.Slug == model.Slug && x.CategoryId == model.CategoryId);
                    if (slug != null)
                    {
                        TempData["msg"] = "This Vacancy has been exists.";
                        TempData["msg_type"] = "danger";

                        ViewBag.Branch = new SelectList(_context.Branches.ToList(), "Id", "Name",model.BranchId);
                        ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Slug != "vacancy-service").ToList(), "Id", "Name",model.CategoryId);
                        return View(model);
                    }

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

                    Vacancy vacancy = new Vacancy();
                    vacancy.Title = model.Title;
                    vacancy.Slug = model.Slug;
                    vacancy.Description = model.Description;
                    vacancy.Phone = model.Phone;
                    // Sau này fix = Session User login
                    vacancy.UserId = 2;
                    vacancy.BranchId = model.BranchId;
                    vacancy.CategoryId = model.CategoryId;
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

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var vacancy = await _unitOfWork.Vacancy.GetFirstOrDefault(x => x.Id == id);            

                ViewBag.Branch = new SelectList(_context.Branches.ToList(), "Id", "Name", vacancy.BranchId);
                ViewBag.Category = new SelectList(_context.Categories.Where(x => x.Slug != "vacancy-service").ToList(), "Id", "Name", vacancy.CategoryId);
                ViewBag.User = new SelectList(_context.Users.ToList(), "Id", "Name", vacancy.UserId);

                return View(vacancy);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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

                        var vacancyDb = await _unitOfWork.Vacancy.GetAll(x => x.Id != id);
                        foreach (var itemDb in vacancyDb)
                        {
                            //Kiểm tra slug exists trên db hay chưa, ngoại trừ Id hiện tại và slug trùng phải cùng Category
                            var slug = await _context.Vacancies.Where(x => x.Id != id).FirstOrDefaultAsync(p => p.Slug == vacancy.Slug && p.CategoryId == itemDb.CategoryId);
                            if (slug != null)
                            {
                                //ModelState.AddModelError("", "This Vacancy already exists !");
                                TempData["msg"] = "This Vacancy already exists in this category";
                                TempData["msg_type"] = "danger";
                                return View(vacancy);
                            }
                        }

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
                        
                        //vacancy.UserId = model.UserId;
                        vacancy.CategoryId = model.CategoryId;
                        vacancy.BranchId = model.BranchId;
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
                Vacancy vacancy = await _unitOfWork.Vacancy.GetFirstOrDefault(x => x.Id == id, includeProperties: "User,Category,Branch");
                if (vacancy == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }
                return View(vacancy);

            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }
        }
    }
}
