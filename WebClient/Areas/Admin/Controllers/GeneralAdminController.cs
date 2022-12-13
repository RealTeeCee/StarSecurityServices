using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GeneralAdminController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public GeneralAdminController(StarSecurityDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.User.GetAll(x=>x.Role.Id != 1 ,includeProperties: "Role"));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Role = new SelectList(_context.Roles.Where(x => x.Id != 1 && x.Id !=2), "Id", "Name");
            ViewBag.Branch = new SelectList(_context.Branches.ToList(), "Id", "Name");

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
        public async Task<IActionResult> Create(Administrator model)
        {
            try
            {
                if (model!=null)
                {
                    User user = new User();
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.Password = model.Password;
                    user.Phone = model.Phone;
                    user.RoleId = model.RoleId;              

                    await _unitOfWork.User.Add(user);
                    await _unitOfWork.Save();

                    var newUser = _context.Users.OrderByDescending(x => x.Id).FirstOrDefault();

                    UserBranch userBranch = new UserBranch();
                    userBranch.UserId = newUser.Id;
                    userBranch.BranchId = model.BranchId;

                    await _unitOfWork.UserBranch.Add(userBranch);
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
        public async Task<IActionResult> Details(int id)
        {            
            User user = await _unitOfWork.User.GetFirstOrDefault(x => x.Id == id,includeProperties:"Role");
            try
            {
                if (user == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }
                return View(user);

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
                ViewBag.Role = new SelectList(_context.Roles.Where(x => x.Id != 1 && x.Id != 2), "Id", "Name");
                var model = await _unitOfWork.User.GetFirstOrDefault(x => x.Id == id); //var model = User user
                return View(model);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Customer" });

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User model)
        {
            try
            {

                if (model!=null)
                {
                    var user = await _unitOfWork.User.GetFirstOrDefault(x => x.Id != model.Id);
                    if (user != null)
                    {
                        model.Name = user.Name;
                        model.Email = user.Email;
                        model.Password = user.Password;
                        model.Phone = user.Phone;
                        model.Address = user.Address;
                        model.Image = user.Image;
                        model.RoleId = user.RoleId;
                        model.Status = user.Status;
                        model.UpdatedAt = DateTime.Now;
                        _unitOfWork.User.Update(model);
                        await _unitOfWork.Save();
                        TempData["msg"] = "User has been Updated.";
                        TempData["msg_type"] = "primary";
                    }
                }
                return RedirectToAction("Edit");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Customer" });
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await _unitOfWork.User.GetFirstOrDefault(x => x.Id == id);

                _unitOfWork.User.Remove(model);
                await _unitOfWork.Save();
                TempData["msg"] = "User has been Deleted.";
                TempData["msg_type"] = "danger";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Customer" });

            }

        }
    }
}
