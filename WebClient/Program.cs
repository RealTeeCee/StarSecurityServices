using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models;
using Services;
using System.Configuration;
using WebClient.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<StarSecurityDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("StarDB")));


builder.Services.AddIdentity<User, IdentityRole>(options => 
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 3; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Khóa 2 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    //options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    //options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
    //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    //options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)    
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

    
}).AddEntityFrameworkStores<StarSecurityDbContext>()
  .AddDefaultTokenProviders();

//builder.Services.AddMvc(option =>
//{
//    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//    option.Filters.Add(new AuthorizeFilter(policy));
//});
//builder.Services.AddAuthentication()
//        .AddCookie(options =>
//        {
//            options.LoginPath = "/login/";

//        });


//Claims Base Authorization
//Claims are policy base -> Create policy and include Claim then register that Claim Policy

builder.Services.AddAuthorization(options => 
{
    //options.AddPolicy("CreatePolicy", policy => policy.RequireClaim("Create", "true")); //This is ClaimType not ClaimValue, ClaimType is CaseInSentitive
    //options.AddPolicy("EditPolicy", policy => policy.RequireClaim("Edit", "true"));
    //options.AddPolicy("DeletePolicy", policy => policy.RequireClaim("Delete", "true"));

    //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));//ClaimValue "true" is CaseSensitive (must match ClaimValue in db). 
    //=> policy.RequireClaim("Edit Role", "allow","true","ok", "yes" )) --> ClaimValue is a string[] , "allow" OR "true" , etc..


    //Create policy with custom requirement
    //options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement())); //our Requirement

    //options.InvokeHandlersAfterFailure = false; //Not invoke other handlers if current handler is failure

    //Custom policy using func: Must have role Admin AND have edit role claim with claim value of true OR have role SuperAdmin (Different to Normal Policy: Can use && || operator)
    options.AddPolicy("CreatePolicy", policy => policy.RequireAssertion(
                                                context => context.User.IsInRole("GeneralAdmin") ||
                                                context.User.IsInRole("Admin") && 
                                                context.User.HasClaim(claim => claim.Type == "Create"  && claim.Value == "true") ||
                                                context.User.IsInRole("Admin") &&
                                                context.User.HasClaim(claim => claim.Type == "All" && claim.Value == "true") ||
                                                context.User.IsInRole("SuperAdmin") 
                                                ));

    options.AddPolicy("EditPolicy", policy => policy.RequireAssertion(
                                                context => context.User.IsInRole("GeneralAdmin") ||
                                                context.User.IsInRole("Admin") &&
                                                context.User.HasClaim(claim => claim.Type == "Edit" && claim.Value == "true") ||
                                                context.User.IsInRole("Admin") &&
                                                context.User.HasClaim(claim => claim.Type == "All" && claim.Value == "true") ||
                                                context.User.IsInRole("SuperAdmin")
                                               ));

    options.AddPolicy("DeletePolicy", policy => policy.RequireAssertion(
                                           context => context.User.IsInRole("GeneralAdmin") ||
                                            context.User.IsInRole("Admin") &&
                                            context.User.HasClaim(claim => claim.Type == "Delete" && claim.Value == "true") ||
                                            context.User.IsInRole("Admin") &&
                                            context.User.HasClaim(claim => claim.Type == "All" && claim.Value == "true") ||
                                            context.User.IsInRole("SuperAdmin")
                                           ));

});       

builder.Services.ConfigureApplicationCookie(config => 
{
    config.LoginPath = "/Admin/Account/Login";
    config.AccessDeniedPath = "/Admin/Account/AccessDenied";
});



builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages();

builder.Services.Configure<MailSettings>(
        builder.Configuration.GetSection("MailSettings")
    );


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, SendMailService>();
builder.Services.AddScoped<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();//Register this handler 
builder.Services.AddScoped<IAuthorizationHandler, SuperAdminHandler>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) 
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "demo",
//    pattern: "admin/{action}/{id?}",
//    defaults: new { controller = "User", action = "Login" });

app.Run();

public class Programs
{
    public static string pathRoot =  "https://localhost:7273";
}