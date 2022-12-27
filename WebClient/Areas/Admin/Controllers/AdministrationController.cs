using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModel;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin, Employee")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly StarSecurityDbContext context;
        private readonly SignInManager<User> signInManager;
        private int pageSize = 6;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IUnitOfWork unitOfWork, StarSecurityDbContext context, SignInManager<User> signInManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.context = context;
            this.signInManager = signInManager;
        }
        //--------------------------------- USER ---------------------------------
        //Create User is Register

        
        public async Task<IActionResult> ListUsers(int p = 1)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                ViewBag.UserId = claims.Value;
                var users = userManager.Users; //List of Users

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Categories.Count() / this.pageSize);

                ViewBag.List = "List Users";
                ViewBag.Controller = "Administration";
                ViewBag.AspAction = "ListUsers";

                return View(users.Skip((p - 1) * this.pageSize).Take(this.pageSize));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }

        }
        [Authorize(Roles = "SuperAdmin, GeneralAdmin, Admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var userRole = await userManager.GetRolesAsync(user);
            if(userRole.Count > 0)
            {
                ViewBag.UserRole = userRole[0];
            }

            if (user == null)
            {                
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);


            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name,
                Address = user.Address,
                Claims = userClaims.Select(c => c.Type + " : " + (c.Value == "True" ? "Allowed" : "Not Allow")).ToList(),
                Roles = userRoles                
            };

            ViewBag.List = "List Users";
            ViewBag.Controller = "Administration";
            ViewBag.AspAction = "ListUsers";
            ViewBag.AspSubAction = "EditUser";
            ViewBag.Action = "Edit User";            

            return View(model);

        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, GeneralAdmin, Admin")]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Name = model.Name;
                user.Address = model.Address;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Update User Infomation Successfully.";
                    TempData["msg_type"] = "success";
                    
                    return RedirectToAction("ListUsers");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("ListUsers");
            }
        }


        //[Authorize(Roles = "SuperAdmin, GeneralAdmin")]
        //public async Task<IActionResult> ManageUserRoles(string userId)
        //{
        //    ViewBag.userId = userId;

        //    var user = await userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
        //        return RedirectToAction("NotFound", "Error");
        //    }

        //    var model = new List<UserRolesViewModel>();


        //    foreach (var role in roleManager.Roles)
        //    {
        //        var userRolesViewModel = new UserRolesViewModel
        //        {
        //            RoleId = role.Id,
        //            RoleName = role.Name
        //        };

        //        if (await userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            userRolesViewModel.IsSelected = true;
        //        }
        //        else
        //        {
        //            userRolesViewModel.IsSelected = false;
        //        }
        //        model.Add(userRolesViewModel);

        //    }

        //    ViewBag.List = "List Users";
        //    ViewBag.Controller = "Administration";
        //    ViewBag.AspAction = "ListUsers";
        //    ViewBag.AspSubAction = "ManageUserRoles";
        //    ViewBag.Action = "Manage Change Roles In User";

        //    return View(model);

        //}

        //[HttpPost]

        ////[Authorize(Roles = "SuperAdmin")]
        //public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId) //Muon nhan dc roleId o ben View chi dc sd form method post (ko action)
        //{
        //    var user = await userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
        //        return RedirectToAction("NotFound", "Error");
        //    }

        //    //var check = model.Where(x => x.IsSelected).Select(y => y.RoleName);

        //    //foreach (var item in check)
        //    //{
        //    //    if(item == "GeneralAdmin" && await roleManager.RoleExistsAsync("GeneralAdmin"))
        //    //    {
        //    //        TempData["msg"] = "In Star Security System only have one General Admin";
        //    //        TempData["msg_type"] = "danger";
        //    //        return RedirectToAction("EditUser");
        //    //    }
        //    //}

        //    var roles = await userManager.GetRolesAsync(user); //lay tat ca Roles thuoc User nay
        //    var result = await userManager.RemoveFromRolesAsync(user, roles); //xoa tat ca roles trong user

        //    if (!result.Succeeded)//Neu ko xoa dc User nay
        //    {
        //        ModelState.AddModelError("", "cannot remove user existing roles");//Khong the xoa cac roles dang ton tai trong user
        //        return View(model);
        //    }

        //    result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
        //    //Get List of Selected roles -> select RoleName -> Add selected RoleNames to User

        //    if (!result.Succeeded)//neu xay ra loi khi add selected Roles to User
        //    {
        //        ModelState.AddModelError("", "cannot add selected roles to user");
        //        return View(model);
        //    }


        //    // Get All trong Branch
        //    var branchs = await unitOfWork.Branch.GetAll();
        //    foreach (var branch in branchs)
        //    {
        //        // Check Branch trong UserBranch isExist ?
        //        var userBranch = await unitOfWork.UserBranch.GetFirstOrDefault(x => x.BranchId == branch.Id && x.UserId == user.Id);
        //        // If not exist => created
        //        if (userBranch == null)
        //        {
        //            var newUserBranch = new UserBranch();
        //            newUserBranch.BranchId = branch.Id;
        //            newUserBranch.UserId = user.Id;
        //            await unitOfWork.UserBranch.Add(newUserBranch);
        //            await unitOfWork.Save();
        //        }
        //    }

        //    TempData["msg"] = "Edit User 's Roles Successfully.";
        //    TempData["msg_type"] = "success";
        //    return RedirectToAction("EditUser", new { Id = userId });

        //}

        [Authorize(Roles = "SuperAdmin, GeneralAdmin, Admin")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {                        
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            var model = new UserClaimsViewModel() //Create an instance of ViewModel
            {
                UserId = userId //populating the UserId property on the UI
            };

            var existingUserClaims = await userManager.GetClaimsAsync(user); //Retrieve all Claims from User
            
            foreach (Claim claim in ClaimsStore.AllClaims) //Use foreach to loop through each claim and retrieve list of all Claims
            {
                UserClaim userClaim = new UserClaim // Create an instance of UserClaim each loop 
                {
                    ClaimType = claim.Type //and populating its ClaimType
                };

                //If the user has the claim, set IsSelected property to true, so the check box next to the claim is checked on the UI

                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "True")) //Checking if existingUserClaims existing user Claims (existingUserClaims contains all the user claims)
                //To determine the value for IsSelected Property we need to know whether if the user that we are currently editing has this claim that we iterating over 
                {
                    userClaim.IsSelected = true; //populating its Selected property 
                }
               
                model.Claims.Add(userClaim);
            }

            ViewBag.List = "List Users";
            ViewBag.Controller = "Administration";
            ViewBag.AspAction = "ListUsers";
            ViewBag.AspSubAction = "ManageUserClaims";
            ViewBag.Action = "Manage User Permission";

            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, GeneralAdmin")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model) //Muon nhan dc roleId o ben View chi dc sd form method post (ko action)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            var claims = await userManager.GetClaimsAsync(user); //lay tat ca claims thuoc User nay
            var result = await userManager.RemoveClaimsAsync(user, claims); //xoa tat ca claims trong user

            if (!result.Succeeded)//Neu ko xoa dc User nay
            {
                ModelState.AddModelError("", "cannot remove user existing claims");//Khong the xoa cac claims dang ton tai trong user
                return View(model);
            }

            result = await userManager.AddClaimsAsync(user, model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "True" : "False")));//If the Claim selected on UI -> store it with true otherwise false
            //Get List of Selected roles -> select RoleName -> Add selected RoleNames to User

            if (!result.Succeeded)//neu xay ra loi khi add selected Claims to User
            {
                ModelState.AddModelError("", "cannot add selected roles to user");
                return View(model);
            }
            else
            {
                await userManager.UpdateSecurityStampAsync(user);                
            }

            TempData["msg"] = "Edit User 's Claims Successfully.";
            TempData["msg_type"] = "success";
            
            return RedirectToAction("EditUser", new { Id = model.UserId });

        }

        //[Authorize(Roles = "SuperAdmin, Admin")]
        //public async Task<IActionResult> ManageRoleClaims(string roleId)
        //{
        //    var role = await roleManager.FindByIdAsync(roleId);

        //    if (role == null)
        //    {
        //        ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
        //        return RedirectToAction("NotFound", "Error");
        //    }

        //    var model = new RoleClaimsViewModel() //Create an instance of ViewModel
        //    {
        //        RoleId = roleId //populating the RoleId property on the UI
        //    };

        //    var existingRoleClaims = await roleManager.GetClaimsAsync(role); //Retrieve all Claims from Role

        //    foreach (Claim claim in RoleClaimsStore.AllClaims) //Use foreach to loop through each claim and retrieve list of all Claims
        //    {
        //        RoleClaim roleClaim = new RoleClaim // Create an instance of RoleClaim each loop 
        //        {
        //            ClaimType = claim.Type //and populating its ClaimType
        //        };

        //        //If the role has the claim, set IsSelected property to true, so the check box next to the claim is checked on the UI

        //        if (existingRoleClaims.Any(c => c.Type == claim.Type && c.Value == "true")) //Checking if existingRoleClaims existing role Claims (existingRoleClaims contains all the role claims)
        //        //To determine the value for IsSelected Property we need to know whether if the role that we are currently editing has this claim that we iterating over 
        //        {
        //            roleClaim.IsSelected = true; //populating its Selected property 
        //        }

        //        model.Claims.Add(roleClaim);
        //    }

        //    return View(model);

        //}

        //[HttpPost]
        //[Authorize(Roles = "SuperAdmin, Admin")]
        //public async Task<IActionResult> ManageRoleClaims(RoleClaimsViewModel model) //Muon nhan dc roleId o ben View chi dc sd form method post (ko action)
        //{
        //    var role = await roleManager.FindByIdAsync(model.RoleId);

        //    if (role == null)
        //    {
        //        ViewBag.ErrorMessage = $"Role with Id = {model.RoleId} cannot be found";
        //        return RedirectToAction("NotFound", "Error");
        //    }

        //    var claims = await roleManager.GetClaimsAsync(role); //lay tat ca claims thuoc Role nay
        //    foreach (var claim in claims)
        //    {
        //        await roleManager.RemoveClaimAsync(role, claim); //xoa tat ca claims trong Role
        //    }
        //    //var result = await roleManager.RemoveClaimAsync(role, claims); //xoa tat ca claims trong user

        //    //if (!result.Succeeded)//Neu ko xoa dc User nay
        //    //{
        //    //    ModelState.AddModelError("", "cannot remove Role existing claims");//Khong the xoa cac claims dang ton tai trong user
        //    //    return View(model);
        //    //}

        //    //var result = await roleManager.AddClaimAsync(role, model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));//If the Claim selected on UI -> store it with true otherwise false
        //    //Get List of Selected roles -> select RoleName -> Add selected RoleNames to User

        //    //if (!result.Succeeded)//neu xay ra loi khi add selected Claims to User
        //    //{
        //    //    ModelState.AddModelError("", "cannot add selected roles to user");
        //    //    return View(model);
        //    //}

        //    return RedirectToAction("EditUser", new { Id = model.RoleId });

        //}

        [Authorize(Policy = ("DeletePolicy"))]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Delete User Successfully.";
                    TempData["msg_type"] = "success";
                    
                    return RedirectToAction("ListUsers");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("ListRoles");
            }
        }

        //--------------------------------- ROLE ---------------------------------
        [Authorize(Roles = "SuperAdmin, GeneralAdmin")]
        public IActionResult CreateRole()
        {

            ViewBag.List = "List Roles";
            ViewBag.Controller = "Administration";
            ViewBag.AspAction = "ListRoles";
            ViewBag.AspSubAction = "CreateRole";
            ViewBag.Action = "Create Role";

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, GeneralAdmin")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                if (identityRole.Name != "Admin" && identityRole.Name != "GeneralAdmin" && identityRole.Name != "SuperAdmin" && identityRole.Name != "Employee")
                {
                    TempData["msg"] = "Failed to create Role! This Role is not in Star Security System.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("ListRoles", "Administration");
                }

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Create Role Successfully.";
                    TempData["msg_type"] = "success";
                    
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }            

            return View(model);
        }

        
        public async Task<IActionResult> ListRoles(int p = 1)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                ViewBag.UserId = claims.Value;

                var roles = roleManager.Roles; //List of Roles

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Categories.Count() / this.pageSize);

                ViewBag.List = "List Roles";
                ViewBag.Controller = "Administration";
                ViewBag.AspAction = "ListRoles";

                return View(roles.Skip((p - 1) * this.pageSize).Take(this.pageSize));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }

        }
        [Authorize(Roles = "SuperAdmin, GeneralAdmin, Admin")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name                
            };

            foreach (var user in userManager.Users) //userManager.Users return List of all register user 
            {
                if(await userManager.IsInRoleAsync(user, role.Name)){
                    model.Users.Add(user.UserName);
                }
            }


            ViewBag.List = "List Roles";
            ViewBag.Controller = "Administration";
            ViewBag.AspAction = "ListRoles";
            ViewBag.AspSubAction = "EditRole";
            ViewBag.Action = "Edit Role";            

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, GeneralAdmin, Admin")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }
            else
            {
                role.Name = model.RoleName;

                if (role.Name != "Admin" && role.Name != "GeneralAdmin" && role.Name != "SuperAdmin" && role.Name != "Employee")
                {
                    TempData["msg"] = "Failed to Edit Role! This Role is not in Star Security System.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("ListRoles", "Administration");
                }

                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Update Successfully.";
                    TempData["msg_type"] = "success";
                    
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
           
       

            return View(model);

        }

        [Authorize(Roles = "SuperAdmin, GeneralAdmin, Admin")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;            

            var role = await roleManager.FindByIdAsync(roleId);
            ViewBag.RoleName = role.Name;
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            var model = new List<RoleUsersViewModel>();
             

            foreach (var user in userManager.Users)
            {
                var rolesName = userManager.GetRolesAsync(user).Result;

                //foreach (var roleName in roleNames)
                //{

                var roleUsersViewModel = new RoleUsersViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    RolesName = rolesName                    
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    roleUsersViewModel.IsSelected = true;
                }
                else
                {
                    roleUsersViewModel.IsSelected = false;
                }



                model.Add(roleUsersViewModel);
                //}                    
            }

            ViewBag.List = "List Roles";
            ViewBag.Controller = "Administration";
            ViewBag.AspAction = "ListRoles";
            ViewBag.AspSubAction = "EditUsersInRole";
            ViewBag.Action = "Edit Users In Role";            

            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, GeneralAdmin, Admin")]
        public async Task<IActionResult> EditUsersInRole(List<RoleUsersViewModel> model, string roleId) //Muon nhan dc roleId o ben View chi dc sd form method post (ko action)
        {
            //Tim role cua 
            var role = await roleManager.FindByIdAsync(roleId);            

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsSelected && !await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                    if(result.Succeeded)
                    {


                        // Get All trong Branch
                        var branchs = await unitOfWork.Branch.GetAll();
                        foreach (var branch in branchs)
                        {
                            // Check Branch trong UserBranch isExist ?
                            var userBranch = await unitOfWork.UserBranch.GetFirstOrDefault(x => x.BranchId == branch.Id && x.UserId == user.Id);
                            // If not exist => created
                            if (userBranch == null)
                            {
                                // Check If User == GeneralAdmin
                                if(await userManager.IsInRoleAsync(user, "GeneralAdmin"))
                                {
                                    var newUserBranch = new UserBranch();
                                    newUserBranch.BranchId = branch.Id;
                                    newUserBranch.UserId = user.Id;
                                    await unitOfWork.UserBranch.Add(newUserBranch);
                                    await unitOfWork.Save();
                                }
                            }
                        }
                    }
                }

                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    // Get All trong Branch
                    var branchs = await unitOfWork.Branch.GetAll();
                    foreach (var branch in branchs)
                    {
                        // Check Branch trong UserBranch isExist ?
                        var userBranch = await unitOfWork.UserBranch.GetFirstOrDefault(x => x.BranchId == branch.Id && x.UserId == user.Id);
                        // If exist => deleted
                        if (userBranch != null)
                        {
                            // Check If User == GeneralAdmin
                            if (await userManager.IsInRoleAsync(user, "GeneralAdmin"))
                            {
                                unitOfWork.UserBranch.Remove(userBranch);
                                await unitOfWork.Save();
                            }
                        }
                    }

                    result = await userManager.RemoveFromRoleAsync(user, role.Name);

                }

                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    //if (i < model.Count - 1)
                    //{
                    //    continue;
                    //}
                    //else
                    //{

                        TempData["msg"] = "Edit Users in Role Successfully.";
                        TempData["msg_type"] = "success";
                        
                        return RedirectToAction("EditRole", new { Id = roleId });
                    //}
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });

        }

        [Authorize(Roles = "SuperAdmin, GeneralAdmin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["msg"] = "Delete Role Successfully.";
                    TempData["msg_type"] = "success";
                    
                    return RedirectToAction("ListRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("ListRoles");
            }
        }
    }
}
