﻿using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin")]
    public class ContactController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;
        private int pageSizes = 6;

        public ContactController(IUnitOfWork unitOfWork, StarSecurityDbContext context, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            this._context = context;
            this.env = env;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var model = await unitOfWork.Contact.GetAll();

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSizes;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Contacts.Count() / this.pageSizes);

                ViewBag.List = "List Contacts";
                ViewBag.Controller = "Contact";
                ViewBag.AspAction = "Index";

                return View(model.Skip((p - 1) * this.pageSizes).Take(this.pageSizes));
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
                var model = await unitOfWork.Contact.GetFirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    TempData["msg"] = "This Contact does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }
                else
                {
                    unitOfWork.Contact.Remove(model);
                    await unitOfWork.Save();

                    TempData["msg"] = "This Contact has been Deleted.";
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
