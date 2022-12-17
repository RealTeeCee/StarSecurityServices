using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<StarSecurityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StarDB")));

builder.Services.AddIdentity<User, IdentityRole>(option => {
        option.Password.RequiredLength = 10;
        option.Password.RequiredUniqueChars = 3;
    }).AddEntityFrameworkStores<StarSecurityDbContext>();

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
builder.Services.ConfigureApplicationCookie(config => {
    config.LoginPath = "/Admin/Account/Login";
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


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


app.Run();
