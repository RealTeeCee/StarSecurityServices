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
            ViewBag.Role = new SelectList(_context.Roles.Where(x => x.Id == 2), "Id", "Name");

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
        public async Task<IActionResult> Create(User model)
        {
            try
            {
                if (model!=null)
                {
                    //Collection{ anyValue }
                    var branches = await _unitOfWork.Branch.GetAll();
                    if(branches.Count() == 0)
                    {
                        TempData["msg"] = "Branch not found! Please create branch first!";
                        TempData["msg_type"] = "danger";
                        return View();
                    }

                    await _unitOfWork.User.Add(model);                    
                    await _unitOfWork.Save();

                    // select * from Users LIMIT 1
                    //SELECT TOP 1 Id FROM Users ORDERBY Id DESC
                    // luc nay co UserId = 2 roi
                    // 
                    // Id = 4                  

                    var user = await _context.Users.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    for(int i=1;i<= branches.Count(); i++ )
                    {

                        UserBranch userBranch = new UserBranch();
                        userBranch.UserId = user.Id;
                        userBranch.BranchId = i;

                        await _unitOfWork.UserBranch.Add(userBranch);
                        await _unitOfWork.Save();

                    }


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
                if(user == null)
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
                ViewBag.Role = new SelectList(_context.Roles.Where(x => x.Id != 1), "Id", "Name");
                var model = await _unitOfWork.User.GetFirstOrDefault(x => x.Id == id, includeProperties: "Role"); //var model = User user
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
                    if(user != null)
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
