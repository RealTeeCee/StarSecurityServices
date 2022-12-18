using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestimonialController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public TestimonialController(StarSecurityDbContext context, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
            this._env = env;
        }

        public IActionResult Index()
        {
            return View();
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
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            try
            {
                if (testimonial != null)
                {
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
                return View(testimonial);
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
                var testimonial = await _unitOfWork.Testimonial.GetFirstOrDefault(x => x.Id == id);
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
                        testimonial.UpdatedAt = DateTime.Now;
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
    }
}
