using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModel;
using System.Data;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin, Employee")]
    public class BranchController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;        

        public BranchController(StarSecurityDbContext context, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            this.roleManager = roleManager;
            this.userManager = userManager;
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

                ViewBag.List = "List Branches";
                ViewBag.Controller = "Branch";
                ViewBag.AspAction = "Index";

                return View(model.Skip((p - 1) * pageSize).Take(pageSize));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [Authorize(Roles = "SuperAdmin, GeneralAdmin")]
        public async Task<IActionResult> EditUsersToBranch(long id)
        {            
                try
                {
                    //ViewBag.Role = new SelectList(_context.Roles.Where(x => x.Id == 2), "Id", "Name");
                    ViewBag.BranchId = id;
                    var branch = await _unitOfWork.Branch.GetFirstOrDefault(x => x.Id == id);

                    if (branch == null)
                    {
                        TempData["msg"] = "Branch does not exists.";
                        TempData["msg_type"] = "danger";
                        return RedirectToAction("Index");
                    }
                    var model = new List<BranchUsersViewModel>();

                    

                    foreach (var user in userManager.Users)
                    {   
                        var roleNames = userManager.GetRolesAsync(user).Result;
                        // lay ra tat ca userId = v
                        var usersBranch = await _unitOfWork.UserBranch.GetAll(x => x.UserId == user.Id);

                        foreach (var roleName in roleNames)
                        {

                            var branchUsersViewModel = new BranchUsersViewModel
                            {
                                UserId = user.Id,
                                UserName = user.UserName,
                                RoleName = roleName
                            };

                                           
                            
                                                                                    
                            foreach (var userBranch in usersBranch)
                            {
                                //Neu User da co branch (tuc la trong UsersBranches co BranchId = thisRecordBranchid da co UserId nay roi ) thi true
                                if ( userBranch.BranchId == id)
                                {
                                    branchUsersViewModel.IsSelected = true;
                                }
                                else
                                {
                                    branchUsersViewModel.IsSelected = false;
                                }
                                
                            }

                            model.Add(branchUsersViewModel);
                        }

                    }

                    ViewBag.List = "List Branches";
                    ViewBag.Controller = "Branch";
                    ViewBag.AspAction = "Index";
                    ViewBag.AspSubAction = "EditUsersToBranch";
                    ViewBag.Action = "Edit Users To Branch";

                    return View(model);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, GeneralAdmin")]
        public async Task<IActionResult> EditUsersToBranch(List<BranchUsersViewModel> model, long id) //Muon nhan dc roleId o ben View chi dc sd form method post (ko action) va asp-route-id
        {
            if (model.Count() > 0)
            {
                foreach (var item in model)
                {
                    // Lấy toàn bộ UserBranch ra so sánh với model.UserId truyền vào
                    var usersBranch = await _unitOfWork.UserBranch.GetAll(x => x.UserId == item.UserId);

                    //if (item.IsSelected && usersBranch.UserId != item.UserId)

                    // Nếu chưa tồn tại, thì thêm mới với BrandId == id
                    if (item.IsSelected && usersBranch.Count() == 0)
                    {
                        var newUserBranch = new UserBranch();
                        newUserBranch.UserId = item.UserId;
                        newUserBranch.BranchId = id;
                        await _unitOfWork.UserBranch.Add(newUserBranch);
                        await _unitOfWork.Save();
                    }

                    else if (!item.IsSelected && usersBranch.Count() > 0)
                    {

                        var deleteUserBranch = await _unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == item.UserId && x.BranchId == id);                        
                        _unitOfWork.UserBranch.Remove(deleteUserBranch);
                        await _unitOfWork.Save();
                    }                  

                }
                TempData["msg"] = "Update Branch for Users Successfully.";
                TempData["msg_type"] = "success";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        [Authorize(Policy = ("CreatePolicy"))]
        public async Task<IActionResult> Create()
        {
            //ViewBag.Role = new SelectList(_context.Roles.Where(x => x.Id == 2), "Id", "Name");
            try
            {
                ViewBag.List = "List Branches";
                ViewBag.Controller = "Branch";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Create";
                ViewBag.Action = "Create Branch";
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("DeletePolicy"))]
        public async Task<IActionResult> Create(Branch model)
        {
            try
            {
                if (model != null)
                {
                    await _unitOfWork.Branch.Add(model);
                    await _unitOfWork.Save();
                   
                    User superAdmins = _context.Users.Where(x => x.Email == "nguyenngocnguyen.rtc@starsec.com").FirstOrDefault();
                    if (superAdmins != null) //Collection kiem tra null = Count > 0
                    {                        
                        var userBranch = await _unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == superAdmins.Id && x.BranchId == model.Id);
                        if (userBranch == null)
                        {
                            UserBranch newBranch = new UserBranch();
                            newBranch.UserId = superAdmins.Id;
                            newBranch.BranchId = model.Id;
                            await _unitOfWork.UserBranch.Add(newBranch);
                            await _unitOfWork.Save();
                        }                        
                    }                   

                    // Get a list of users have GeneralAdmin role
                    var generalAdmins = userManager.GetUsersInRoleAsync("GeneralAdmin").Result;

                    //List<User> generalAdmins = _context.Users.Where(x => x.Email == "generaladmin@starsec.com").ToList();
                    if (generalAdmins.Count > 0)
                    {
                        foreach (var item in generalAdmins)
                        {
                            // Kiem tra trong UserBrach da co UserId hay chua? Neu chua co UserId thi add vo UserBranch
                            var userBranch = await _unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == item.Id && x.BranchId == model.Id);
                            if (userBranch == null)
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
                ViewBag.List = "List Branches";
                ViewBag.Controller = "Branch";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Details";
                ViewBag.Action = "Branch Details";
                return View(branch);

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
                var branch = await _unitOfWork.Branch.GetFirstOrDefault(x => x.Id == id); //var model = User user             

                ViewBag.List = "List Branches";
                ViewBag.Controller = "Branch";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Edit";
                ViewBag.Action = "Edit Branch ";
                return View(branch);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("EditPolicy"))]
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
        [Authorize(Roles ="SuperAdmin, GeneralAdmin")]
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
