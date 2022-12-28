using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModel;
using NuGet.Common;
using Services;
using System.Text;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<User> logger;
        private readonly IEmailSender emailSender;
        private readonly IUnitOfWork unitOfWork;
        private readonly StarSecurityDbContext context;
        private readonly IWebHostEnvironment env;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<User> logger, IEmailSender emailSender, IUnitOfWork unitOfWork, StarSecurityDbContext context, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.unitOfWork = unitOfWork;
            this.context = context;
            this.env = env;
        }

        [Authorize(Policy = ("CreatePolicy"))]
        public IActionResult Register()
        {
            try
            {
                ViewBag.List = "List Users";
                ViewBag.Controller = "Account";
                ViewBag.AspAction = "Register";
                ViewBag.AspSubAction = "CreateUser";
                ViewBag.Action = "Create User";

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }  
        }      

        [HttpPost]
        [Authorize(Policy = ("CreatePolicy"))]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new User {
                    UserName = model.UserName,
                    Email = model.Email ,
                    Address = model.Address,
                    Name = model.Name
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if(signInManager.IsSignedIn(User))
                    {
                        TempData["msg"] = "Create new User Successfully.";
                        TempData["msg_type"] = "success";                        
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    //Login
                    //signInManager.SignInAsync(user, isPersistent: false); //isPersistent: false -> luu webcookie vao session cookie (out ra mat cookie). true -> luu webcookie vao persistent                                                          cookie
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors) //result.Errors chua cac dk truyen qua Data Annotation truyen vao <asp-validate-summary> trong view
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        } 

        [AcceptVerbs("Get","Post")] //if we type email to Email field,the client-side issues and get request to server 
        
        public async Task<IActionResult> IsEmailInUse(string email) //call by jquery-validate method => issues an ajax call => expect a JSON  response return from this method
        {
            var user = await userManager.FindByEmailAsync(email);
            if(user == null) //this mean we dont have validation errors
            {
                return Json(true); 
            }
            else //Email already in use
            {
                return Json($"Email {email} is already in use");
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)//AuthorizationPolicyBuilder.RequireAuthenticatedUser se add 1 returnUrl param vao khi Login
        {

            if (ModelState.IsValid)
            {                
                var result = await signInManager.PasswordSignInAsync(model.UserNameOrEmail, model.Password,model.RememberMe, true);//tham so rememberMe = isPersistent(rememberMe.value)                               

                if (!result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(model.UserNameOrEmail);
                    if (user != null)
                    {
                        result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);//tham so rememberMe = isPersistent(rememberMe.value)                               
                    }
                }

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        TempData["msg"] = "Login Successfully.";
                        TempData["msg_type"] = "success";
                        return RedirectToAction("Index", "Home");
                    }                    
                }

                if (result.IsLockedOut)
                {
                    ViewBag.userEmail = model.UserNameOrEmail;
                    return View("AccountLocked");
                }

                TempData["msg"] = "Username or Password incorrect !";
                TempData["msg_type"] = "danger";
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt") ;
                
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                string[] strings = model.Email.Split('@');
                var gmailDomain = strings[0] + "@gmail.com";
                
                
                if(user != null )
                {

                    // Phát sinh Token để reset password
                    // Token sẽ được kèm vào link trong email,
                    // link dẫn đến trang /Account/ResetPassword để kiểm tra và đặt lại mật khẩu
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

                    // Gửi email
                    await emailSender.SendEmailAsync(
                        gmailDomain,
                        "Reset password",
                        $"To reset your password please <a href='{passwordResetLink}'>Click here</a>.");

                    // Chuyển đến trang thông báo đã gửi mail để reset password
                    return View("ForgotPasswordConfirmation");

                    
                    //var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    //var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

                    //logger.Log(LogLevel.Warning, passwordResetLink);

                    //return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }

            // Giải mã lại code từ code trong url (do mã này khi gửi mail
            // đã thực hiện Encode bằng WebEncoders.Base64UrlEncode)
            var encodeToken = new ResetPasswordViewModel
            {
                Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token))
            };

            return View(encodeToken); 
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user,model.Token,model.Password);

                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword()
        {

            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }

                await signInManager.RefreshSignInAsync(user);
                TempData["msg"] = "Change password Successfully.";
                TempData["msg_type"] = "success";
                
                return View("ChangePasswordConfirmation");
            }

            return View(model);

        }


        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccountLocked()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await unitOfWork.User.GetFirstOrDefault(x=>x.Id == id);
            var model = await unitOfWork.UserDetail.GetFirstOrDefault(x => x.UserId == id, includeProperties: "User");
            if (model == null)
            {
                UserDetailViewModel userDetails = new UserDetailViewModel();
                userDetails.UserId = id;
                userDetails.Email = user.Email;
                userDetails.Address = user.Address;
                userDetails.UserName = user.UserName;
                userDetails.Name = user.Name;
                userDetails.Image = user.Image;
                userDetails.ImageUpload = user.ImageUpload;
                userDetails.Phone = user.Phone;

                ViewBag.List = "Your Profile";
                ViewBag.Controller = "Account";
                ViewBag.AspAction = "Profile";

                return View(userDetails);
            }
            UserDetailViewModel userDetailsFetchData = new UserDetailViewModel();
            userDetailsFetchData.UserId = id;
            userDetailsFetchData.Email = user.Email;
            userDetailsFetchData.Address = user.Address;
            userDetailsFetchData.UserName = user.UserName;
            userDetailsFetchData.Name = user.Name;
            userDetailsFetchData.Image = user.Image;
            userDetailsFetchData.ImageUpload = user.ImageUpload;
            userDetailsFetchData.Phone = user.Phone;
            userDetailsFetchData.UserCode = model.UserCode;
            userDetailsFetchData.Award = model.Award;
            userDetailsFetchData.Client = model.Client;
            userDetailsFetchData.Department = model.Department;
            userDetailsFetchData.Grade = model.Grade;
            userDetailsFetchData.UpdatedAt = model.UpdatedAt;

            ViewBag.List = "Your Profile";
            ViewBag.Controller = "Account";
            ViewBag.AspAction = "Profile";

            return View(userDetailsFetchData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProfile(UserDetailViewModel model)
        {
            if (model != null)
            {
                //var model = User user    
                var userDetails = await unitOfWork.UserDetail.GetFirstOrDefault(x => x.UserId == model.UserId,includeProperties:"User");          
                    
                var user = await unitOfWork.User.GetFirstOrDefault(x => x.Id == model.UserId);
                user.Name = model.Name;
                user.Address = model.Address;
                user.Phone = model.Phone;                        

                if (model.ImageUpload != null)
                {
                    foreach (var item in ModelState)
                    {
                        if(item.Key == "ImageUpload")
                        {
                            if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                            {
                                TempData["msg"] = "Only accept extension image: .jpg, .png ";
                                TempData["msg_type"] = "danger";
                                return RedirectToAction("Profile", new { id = model.UserId });
                            }
                        }
                    }
                            
                    string uploadDir = Path.Combine(env.WebRootPath, "media/profiles");
                    if (!string.Equals(user.Image, "default.jpg"))
                    {
                        string oldImagePath = Path.Combine(uploadDir, user.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string imageName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await model.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    user.Image = imageName;
                }
                context.Users.Update(user);
                await unitOfWork.Save();                        
                        
                if(userDetails != null)
                {
                    userDetails.Award = model.Award;
                    userDetails.Client = model.Client;
                    userDetails.Department = model.Department;
                    userDetails.Education = model.Education;
                    userDetails.UpdatedAt = DateTime.Now;

                    context.UserDetails.Update(userDetails);
                    await unitOfWork.Save();
                }
                else
                {
                    // Generate UserCode
                    string templateUserCode = "STAR_";
                    var lastUserDetail = context.UserDetails.OrderByDescending(x => x.Id).FirstOrDefault();
                    if(lastUserDetail != null && lastUserDetail.UserCode != null)
                    {
                        int index;
                        string[] codeNumber = lastUserDetail.UserCode.Split("_");
                        index = Int32.Parse(codeNumber[1]);
                        ++index;
                        templateUserCode += Convert.ToString(index);
                    }
                    else
                    {
                        templateUserCode = "STAR_1";
                    }
                    
                    UserDetail newUserDetails = new UserDetail();                            
                    newUserDetails.UserId = model.UserId;
                    newUserDetails.Award = model.Award;
                    newUserDetails.UserCode = templateUserCode;
                    newUserDetails.Client = model.Client;
                    newUserDetails.Department = model.Department;
                    newUserDetails.Education = model.Education;
                    newUserDetails.UpdatedAt = DateTime.Now;
                    await unitOfWork.UserDetail.Add(newUserDetails);
                    await unitOfWork.Save();
                }                       
                TempData["msg"] = "User Profile has been Updated.";
                TempData["msg_type"] = "success";
            }
                
            return RedirectToAction("Profile", "Account", new { id = model.UserId });
        }

    }
}
