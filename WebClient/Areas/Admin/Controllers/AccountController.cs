using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "SuperAdmin, GeneralAdmin,  Admin, Employee")]
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
            return View();
        }      

        [HttpPost]
        [Authorize(Policy = ("CreatePolicy"))]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new User {
                    UserName = model.Email,
                    Email = model.Email ,
                    Address = model.Address
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if(signInManager.IsSignedIn(User))
                    {
                        TempData["msg"] = "Create new User Successfully.";
                        TempData["msg_type"] = "success";
                        return RedirectToAction("Index", "Home");
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
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)//AuthorizationPolicyBuilder.RequireAuthenticatedUser se add 1 returnUrl param vao khi Login
        {
            if (ModelState.IsValid)
            {                
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,model.RememberMe, true);//tham so rememberMe = isPersistent(rememberMe.value)

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
                    ViewBag.userEmail = model.Email;
                    return View("AccountLocked");
                }
               
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

            return View(); 
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

        
        public async Task<IActionResult> ChangePassword()
        {

            return View();
        }
        
        [HttpPost]
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
                return RedirectToAction("Index", "Home");
                return View("ChangePasswordConfirmation");
            }

            return View(model);


        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccountLocked()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> EditProfile(User model)
        {
            try
            {

                if (model != null)
                {
                    var user = await unitOfWork.User.GetFirstOrDefault(x => x.Id == model.Id); //var model = User user             

                    if (user != null)
                    {
                        user.UserName = model.UserName;
                        user.Address = model.Address;
                        user.Phone = model.Phone;
                        if (model.ImageUpload != null)
                        {
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
                        user.UpdatedAt = DateTime.Now;
                        context.Users.Update(user);
                        await unitOfWork.Save();

                        TempData["msg"] = "User Profile has been Updated.";
                        TempData["msg_type"] = "success";
                    }
                }
                return RedirectToAction("Index","Home");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
    }
}
