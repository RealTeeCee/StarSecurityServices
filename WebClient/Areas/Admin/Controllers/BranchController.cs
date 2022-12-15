using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Index(int p=1)
        {
            try
            {
                var model = await _unitOfWork.Branch.GetAll();

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

                    // Get ra toan bo General Admin trong bang User
                    // List
                    List<User> generalAdmins = _context.Users.Where(x => x.RoleId == 2).ToList();
                    if(generalAdmins.Count > 0)
                    {
                        foreach (var item in generalAdmins)
                        {
                            var userBranch = await _unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == item.Id);
                            if (userBranch != null)
                            {
                                UserBranch newBranch = new UserBranch();
                                newBranch.UserId = item.Id;
                                newBranch.BranchId = model.Id;
                                await _unitOfWork.UserBranch.Add(newBranch);
                                await _unitOfWork.Save();
                            }

                        }
                    }

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
        public async Task<IActionResult> Details(int id)
        {
            Branch branch = await _unitOfWork.Branch.GetFirstOrDefault(x => x.Id == id);
            try
            {
                if (branch == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }
                return View(branch);

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
                var branch = await _unitOfWork.Branch.GetFirstOrDefault(x => x.Id == id); //var model = User user             
            
                return View(branch);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Branch model)
        {
            try
            {

                if (model != null)
                {
                    var branch = await _unitOfWork.Branch.GetFirstOrDefault(x => x.Id == model.Id);

                    if (branch != null)
                    {
                        branch.Name = model.Name;
                        branch.Email = model.Email;
                        branch.Address = model.Address;
                        branch.Phone = model.Phone;
                        branch.Facebook = model.Facebook;
                        branch.Twitter = model.Twitter;
                        branch.Instagram = model.Instagram;
                        branch.Youtube = model.Youtube;
                        branch.TimeOpen = model.TimeOpen;
                        branch.UpdatedAt = DateTime.Now;
                        _context.Branches.Update(branch);
                        await _unitOfWork.Save();
                                           
                        TempData["msg"] = "Branch has been Updated.";
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
                var model = await _unitOfWork.Branch.GetFirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    TempData["msg"] = "Branch does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }


                _unitOfWork.Branch.Remove(model);
                await _unitOfWork.Save();
                TempData["msg"] = "Branch has been Deleted.";
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
