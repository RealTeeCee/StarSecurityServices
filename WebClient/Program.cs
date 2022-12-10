using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StarSecurityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StarDB")));

//Dang ky Identity su dung giao dien.
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
                  .AddEntityFrameworkStores<StarSecurityDbContext>();

//Dang ky Identity su dung giao dien default
//builder.Services.AddDefaultIdentity<User>()
//                  .AddEntityFrameworkStores<StarSecurityDbContext>()
//                  .AddDefaultTokenProviders();

//Add service Mail
builder.Services.AddOptions(); // Kích hoạt Options
// đọc config đăng ký để Inject
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
// Đăng ký dịch vụ Mail
builder.Services.AddScoped<IEmailSender, SendMailService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSession();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
