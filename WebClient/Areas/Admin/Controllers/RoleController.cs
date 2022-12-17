using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(StarSecurityDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var model = await _unitOfWork.Role.GetAll();

                int pageSize = 6;
                ViewBag.PageNumber = p;
                ViewBag.PageRange = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Categories.Count() / pageSize);

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        public async Task<IActionResult> Permission()
        {
            try
            {
                var module = _context.Modules.ToList();


                return View(module);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Permission(int id, string[] name)
        {
            try
            {
                var role = await _unitOfWork.Role.GetFirstOrDefault(x => x.Id == id);

                foreach (var item in name)
                {
                    if (item.EndsWith("security"))
                    {
                        if (item.StartsWith("View"))
                        {

                        }
                        if (item.StartsWith("View"))
                        {

                        }
                        if (item.StartsWith("View"))
                        {

                        }
                        if (item.StartsWith("View"))
                        {

                        }
                    }
                }
                return RedirectToAction("Permission");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
    }
}
