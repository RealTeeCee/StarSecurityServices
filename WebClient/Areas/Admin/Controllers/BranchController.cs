using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public BranchController(StarSecurityDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var branch = await _unitOfWork.Branch.GetAll();
            return View(branch);
        }
        public async Task<IActionResult> Create()
        {
            //ViewBag.Role = new SelectList(_context.Roles.Where(x => x.Id == 2), "Id", "Name");
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
        public async Task<IActionResult> Create(Branch model)
        {
            try
            {
                if (model != null)
                {
                    await _unitOfWork.Branch.Add(model);
                    await _unitOfWork.Save();

                    TempData["msg"] = "Branch has been Created.";
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
