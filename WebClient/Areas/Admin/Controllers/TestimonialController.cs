﻿using DataAccess.Data;
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
        private readonly IWebHostEnvironment _env;

        public TestimonialController(StarSecurityDbContext context, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
            this._env = env;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var model = await _unitOfWork.Testimonial.GetAll();

                int pageSize = 6;
                ViewBag.PageNumber = p;
                ViewBag.PageRange = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Categories.Count() / pageSize);

                ViewBag.List = "List Testimonials";
                ViewBag.Controller = "Testimonial";
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
                ViewBag.List = "List Testimonials";
                ViewBag.Controller = "Testimonials";
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
                ViewBag.Controller = "Testimonials";
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
                ViewBag.Controller = "Testimonials";
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