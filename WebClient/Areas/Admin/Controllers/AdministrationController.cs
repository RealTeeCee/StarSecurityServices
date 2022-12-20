using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Models;
using Models.ViewModel;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,Admin, Employee")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        //--------------------------------- USER ---------------------------------
        //Create User is Register

        
        public async Task<IActionResult> ListUsers()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ViewBag.UserId = claims.Value;
            var users = userManager.Users; //List of Users
            return View(users);

        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

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
                Address = user.Address,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles                
            };                       

            return View(model);

        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
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
                user.Address = model.Address;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        //[Authorize(Policy = "EditRolePolicy")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            var model = new List<UserRolesViewModel>();


            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }

            return View(model);

        }

        [HttpPost]
        //[Authorize(Policy ="EditRolePolicy")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId) //Muon nhan dc roleId o ben View chi dc sd form method post (ko action)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            var roles = await userManager.GetRolesAsync(user); //lay tat ca Roles thuoc User nay
            var result = await userManager.RemoveFromRolesAsync(user, roles); //xoa tat ca roles trong user

            if (!result.Succeeded)//Neu ko xoa dc User nay
            {
                ModelState.AddModelError("", "cannot remove user existing roles");//Khong the xoa cac roles dang ton tai trong user
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
                                                            //Get List of Selected roles -> select RoleName -> Add selected RoleNames to User

            if (!result.Succeeded)//neu xay ra loi khi add selected Roles to User
            {
                ModelState.AddModelError("", "cannot add selected roles to user");
                return View(model);
            } 

            return RedirectToAction("EditUser", new { Id = userId });

        }

        [Authorize(Roles = "SuperAdmin, Admin")]
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

                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true")) //Checking if existingUserClaims existing user Claims (existingUserClaims contains all the user claims)
                //To determine the value for IsSelected Property we need to know whether if the user that we are currently editing has this claim that we iterating over 
                {
                    userClaim.IsSelected = true; //populating its Selected property 
                }
               
                model.Claims.Add(userClaim);
            }

            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
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

            result = await userManager.AddClaimsAsync(user, model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));//If the Claim selected on UI -> store it with true otherwise false
            //Get List of Selected roles -> select RoleName -> Add selected RoleNames to User

            if (!result.Succeeded)//neu xay ra loi khi add selected Claims to User
            {
                ModelState.AddModelError("", "cannot add selected roles to user");
                return View(model);
            }

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
                    return RedirectToAction("ListUsers");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        //--------------------------------- ROLE ---------------------------------
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    if(identityRole.Name != "Admin" || identityRole.Name != "SuperAdmin" || identityRole.Name != "Employee")
                    {
                        TempData["msg"] = "Warning! This Role is not in Star Security System. Any action with this role has no effect";
                        return RedirectToAction("ListRoles", "Administration");
                    }                    
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }            

            return View(model);
        }

        
        public async Task<IActionResult> ListRoles()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ViewBag.UserId = claims.Value;

            var roles = roleManager.Roles; //List of Roles
            return View(roles);

        }
        [Authorize(Roles = "SuperAdmin")]
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

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
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
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
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

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return RedirectToAction("NotFound", "Error");
            }

            var model = new List<RoleUsersViewModel>();
             

            foreach (var user in userManager.Users)
            {               
                var roleUsersViewModel = new RoleUsersViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    roleUsersViewModel.IsSelected = true;
                }
                else
                {
                    roleUsersViewModel.IsSelected = false;
                }
                model.Add(roleUsersViewModel);                
            }

            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditUsersInRole(List<RoleUsersViewModel> model, string roleId) //Muon nhan dc roleId o ben View chi dc sd form method post (ko action)
        {
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
                }

                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });

        }

        [Authorize(Roles = "SuperAdmin")]
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
                    return RedirectToAction("ListRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListRoles");
            }
        }
    }
}
