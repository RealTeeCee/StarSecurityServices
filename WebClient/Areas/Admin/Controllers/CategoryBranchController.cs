using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryBranchController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryBranchController(StarSecurityDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {           
            try
            {
                //ViewBag.Branch = new SelectList(_context.Branches.ToList(), "Id", "Name");                 
                var category = await _unitOfWork.Category.GetAll();

                ViewCategoryBranch viewCategoryBranch = new ViewCategoryBranch();
                viewCategoryBranch.Branches =  _context.Branches.ToList();
                viewCategoryBranch.Categories =  _context.Categories.ToList();


                return View(viewCategoryBranch);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryBranch categoryBranch, string[] test)
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
    }
}
