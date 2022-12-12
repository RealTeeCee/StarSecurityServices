using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SuperAdminController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
    
        public SuperAdminController(StarSecurityDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _unitOfWork.User.GetAll(includeProperties: "Role"));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.RoleId = new SelectList(_context.Roles.Where(x => x.Id != 1), "Id", "Name");

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
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                if(user != null)
                {
                    await _unitOfWork.User.Add(user);
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
    }
}
