using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin, Employee")]
    public class TestimonialController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment env;
        private int pageSize = 6;

        public TestimonialController(StarSecurityDbContext context, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
            this.env = env;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var model = await _unitOfWork.Testimonial.GetAll();

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Testimonials.Count() / this.pageSize);

                ViewBag.List = "List Testimonials";
                ViewBag.Controller = "Testimonial";
                ViewBag.AspAction = "Index";

                return View(model.Skip((p - 1) * this.pageSize).Take(this.pageSize));
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
                ViewBag.List = "List Testimonials";
                ViewBag.Controller = "Testimonial";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Create";
                ViewBag.Action = "Create Testimonial";

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            try
            {
                if (testimonial != null)
                {
                    string imageName = "default.jpg";
                    if (testimonial.ImageUpload != null)
                    {
                        string uploadDir = Path.Combine(env.WebRootPath, "media/testimonials");
                        imageName = Guid.NewGuid().ToString() + "_" + testimonial.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await testimonial.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                    }
                    testimonial.Image = imageName;

                    await _unitOfWork.Testimonial.Add(testimonial);
                    await _unitOfWork.Save();
                    TempData["msg"] = "Testimonial has been Created.";
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
            Testimonial testimonial = await _unitOfWork.Testimonial.GetFirstOrDefault(x => x.Id == id);
            try
            {
                if (testimonial == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }

                ViewBag.List = "List Testimonials";
                ViewBag.Controller = "Testimonial";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Details";
                ViewBag.Action = "Testimonial Details";

                return View(testimonial);
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
                var testimonial = await _unitOfWork.Testimonial.GetFirstOrDefault(x => x.Id == id);

                ViewBag.List = "List Testimonials";
                ViewBag.Controller = "Testimonial";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Edit";
                ViewBag.Action = "Edit Testimonial";

                return View(testimonial);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Testimonial model)
        {
            try
            {
                if (model != null)
                {
                    var testimonial = await _unitOfWork.Testimonial.GetFirstOrDefault(x => x.Id == model.Id);

                    if (testimonial != null)
                    {                        
                        testimonial.Description = model.Description;
                        testimonial.Title = model.Title;
                        testimonial.Name = model.Name;
                        testimonial.UpdatedAt = DateTime.Now;

                        if (model.ImageUpload != null)
                        {
                            string uploadDir = Path.Combine(env.WebRootPath, "media/testimonials");
                            if (!string.Equals(testimonial.Image, "default.jpg"))
                            {
                                string oldImagePath = Path.Combine(uploadDir, testimonial.Image);
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
                            testimonial.Image = imageName;
                        }

                        _context.Testimonials.Update(testimonial);
                        await _unitOfWork.Save();

                        TempData["msg"] = "Testimonial has been Updated.";
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
                var model = await _unitOfWork.Testimonial.GetFirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    TempData["msg"] = "Testimonial does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }

                if (!string.Equals(model.Image, "default.jpg"))
                {
                    string uploadDir = Path.Combine(env.WebRootPath, "media/testimonials");
                    string oldImagePath = Path.Combine(uploadDir, model.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Testimonial.Remove(model);
                await _unitOfWork.Save();

                TempData["msg"] = "Testimonial has been Deleted.";
                TempData["msg_type"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
    }
}