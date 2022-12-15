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

        public async Task<IActionResult> Index(int p =1)
        {
            try
            {
                var model = await _unitOfWork.User.GetAll(x => x.Role.Id != 1 && x.Role.Id != 2, includeProperties: "Role");

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
            //return View(await _unitOfWork.User.GetAll(x=>x.Role.Id != 1 && x.Role.Id != 2 ,includeProperties: "Role"));
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

                    //lấy user mới tạo ra
                    //var newUser = _context.Users.OrderByDescending(x => x.Id).FirstOrDefault();

                    UserBranch userBranch = new UserBranch();
                    userBranch.UserId = user.Id;
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
                
                var user = await _unitOfWork.User.GetFirstOrDefault(x => x.Id == id); //var model = User user
                var model = new Administrator();
                model.Id = user.Id;
                model.Name = user.Name;
                model.Email = user.Email;
                model.Password = user.Password;
                model.Phone = user.Phone;
                model.Address = user.Address;                
                model.RoleId = user.RoleId;
                model.Status = user.Status;
            
                // BranchId , UserId
                var userBranch = await _unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == id);
                model.BranchId = userBranch.BranchId;
                
                ViewBag.Branch = new SelectList(_context.Branches.ToList(), "Id", "Name", model.BranchId);
                return View(model);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Administrator model)
        {
            try
            {

                if (model!=null)
                {
                    var user = await _unitOfWork.User.GetFirstOrDefault(x => x.Id == model.Id);
                    
                    if (user != null)
                    {
                        user.Name = model.Name;
                        user.Email = model.Email;
                        user.Password = model.Password;
                        user.Phone = model.Phone;
                        user.Address = model.Address;                        
                        user.RoleId = model.RoleId;
                        user.Status = model.Status;
                        user.UpdatedAt = DateTime.Now;
                        _unitOfWork.User.Update(user);
                        await _unitOfWork.Save();

                        //var newUser = _context.Users.OrderByDescending(x => x.Id).FirstOrDefault();

                        var userBranch = await _unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == model.Id);
                        userBranch.BranchId = model.BranchId;

                        _context.Update(userBranch);
                        await _unitOfWork.Save();

                        TempData["msg"] = "User has been Updated.";
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
                var model = await _unitOfWork.User.GetFirstOrDefault(x => x.Id == id);

                if (model == null)
                {
                    TempData["msg"] = "User does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }

                _unitOfWork.User.Remove(model);
                await _unitOfWork.Save();
                TempData["msg"] = "User has been Deleted.";
                TempData["msg_type"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }

        }

        //public async Task<IActionResult> Reorder(int[] id)
        //{
        //    int count = 1;
        //    foreach (var userId in id)
        //    {
        //        var user = await _context.Users.FindAsync(userId);
        //        user.Sorting = count;
        //        _context.Update(user);
        //        await _unitOfWork.Save();
        //        count++;
        //    }
        //    return Ok();
        //}
    }
}
