using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }      
        [HttpPost]
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
                    signInManager.SignInAsync(user, isPersistent: false); //isPersistent: false -> luu webcookie vao session cookie (out ra mat cookie). true -> luu webcookie vao persistent                                                          cookie
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
        [HttpPost]
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
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]        
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)//AuthorizationPolicyBuilder.RequireAuthenticatedUser se add 1 returnUrl param vao khi Login
        {
            if (ModelState.IsValid)
            {                
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);//tham so rememberMe = isPersistent(rememberMe.value)

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
               
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt") ;
                
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
