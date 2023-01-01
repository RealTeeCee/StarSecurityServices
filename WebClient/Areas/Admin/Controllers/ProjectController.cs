    using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Models;
using Org.BouncyCastle.Bcpg;
using Services;
using System.Data;
using System.Security.Claims;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin, Employee")]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly StarSecurityDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment env;
        private int pageSize = 6;

        public ProjectController(IUnitOfWork unitOfWork, StarSecurityDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
            this.userManager = userManager;
            this.env = env;
        }
        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var listProject = await unitOfWork.Project.GetAll(includeProperties:"User,Service");

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Projects.Count() / this.pageSize);

                ViewBag.List = "List Projects";
                ViewBag.Controller = "Project";
                ViewBag.AspAction = "Index";

                return View(listProject.Skip((p - 1) * this.pageSize).Take(this.pageSize));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [Authorize(Policy = ("CreatePolicy"))]
        public async Task<IActionResult> Create()
        {
            try
            {
                var users = await unitOfWork.User.GetAll();

                List<User> list = new List<User>();
                foreach (var user in users)
                {
                    if (await userManager.IsInRoleAsync(user, "Employee"))
                    {
                        list.Add(user);
                    }
                }
                var priority = new List<int> { 0, 1, 2 };

                ViewBag.User = new SelectList(list, "Id", "UserName");
                //ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                // Get Current User Model
                var curretUser = await userManager.GetUserAsync(User);
                var checkRoleCurrent = await userManager.IsInRoleAsync(curretUser, "Admin");                

                if (checkRoleCurrent)
                {
                    var userBrand = await unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == curretUser.Id);
                    if(userBrand == null)
                    {
                        ViewBag.Service = null; 
                    }else
                    {
                        ViewBag.Service = new SelectList((from s in context.Services
                                                          join c in context.Categories on s.CategoryId equals c.Id
                                                          join cb in context.CategoryBranches on c.Id equals cb.CategoryId
                                                          where s.Status == 1
                                                          where cb.BranchId == userBrand.BranchId
                                                          select new
                                                          {
                                                              Id = s.Id,
                                                              Name = s.Name,
                                                          }).ToList(), "Id", "Name");
                    }
                    
                }
                else
                {
                    ViewBag.Service = new SelectList(context.Services.Where(x => x.Status == 1).ToList(), "Id", "Name");
                }

               // Mở comment dưới ra làm tiếp cái userBrand.BrandId giùm t nha, xong xem ViewBag lấy đc đúng Service thuộc về Admin đang đăng nhập ko


                ViewBag.Priority = new SelectList(priority, new String[] { "Low", "Medium", "High" });

                ViewBag.List = "List Projects";
                ViewBag.Controller = "Project";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Create";
                ViewBag.Action = "Create Project";

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("CreatePolicy"))]
        public async Task<IActionResult> Create(Project model)
        {
            try
            {
                var users = await unitOfWork.User.GetAll();

                if (model.UserId == "0")
                {
                    TempData["msg"] = "Please choose User.";
                    TempData["msg_type"] = "danger";

                    ViewBag.List = "List Projects";
                    ViewBag.Controller = "Project";
                    ViewBag.AspAction = "Index";
                    ViewBag.AspSubAction = "Create";
                    ViewBag.Action = "Create Project";

                    List<User> list = new List<User>();
                    foreach (var user in users)
                    {
                        if (await userManager.IsInRoleAsync(user, "Employee"))
                        {
                            list.Add(user);
                        }
                    }
                    var priority = new List<int> { 0, 1, 2 };

                    ViewBag.User = new SelectList(list, "Id", "UserName");
                    //ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                    // Get Current User Model
                    var curretUser = await userManager.GetUserAsync(User);
                    var checkRoleCurrent = await userManager.IsInRoleAsync(curretUser, "Admin");

                    if (checkRoleCurrent)
                    {
                        var userBrand = await unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == curretUser.Id);
                        if (userBrand == null)
                        {
                            ViewBag.Service = null;
                        }
                        else
                        {
                            ViewBag.Service = new SelectList((from s in context.Services
                                                              join c in context.Categories on s.CategoryId equals c.Id
                                                              join cb in context.CategoryBranches on c.Id equals cb.CategoryId
                                                              where s.Status == 1
                                                              where cb.BranchId == userBrand.BranchId
                                                              select new
                                                              {
                                                                  Id = s.Id,
                                                                  Name = s.Name,
                                                              }).ToList(), "Id", "Name");
                        }

                    }
                    else
                    {
                        ViewBag.Service = new SelectList(context.Services.Where(x => x.Status == 1).ToList(), "Id", "Name");
                    }

                    // Mở comment dưới ra làm tiếp cái userBrand.BrandId giùm t nha, xong xem ViewBag lấy đc đúng Service thuộc về Admin đang đăng nhập ko


                    ViewBag.Priority = new SelectList(priority, new String[] { "Low", "Medium", "High" });

                    //return RedirectToAction("Create");

                    //List<User> list = new List<User>();
                    //foreach (var user in users)
                    //{
                    //    if (await userManager.IsInRoleAsync(user, "Employee"))
                    //    {
                    //        list.Add(user);
                    //    }
                    //}

                    //ViewBag.User = new SelectList(list, "Id", "UserName");
                    //ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                    return View(model);
                }

                if (model.ServiceId == 0)
                {
                    TempData["msg"] = "Please choose a Service.";
                    TempData["msg_type"] = "danger";

                    ViewBag.List = "List Projects";
                    ViewBag.Controller = "Project";
                    ViewBag.AspAction = "Index";
                    ViewBag.AspSubAction = "Create";
                    ViewBag.Action = "Create Project";

                    List<User> list = new List<User>();
                    foreach (var user in users)
                    {
                        if (await userManager.IsInRoleAsync(user, "Employee"))
                        {
                            list.Add(user);
                        }
                    }
                    var priority = new List<int> { 0, 1, 2 };

                    ViewBag.User = new SelectList(list, "Id", "UserName");
                    //ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                    // Get Current User Model
                    var curretUser = await userManager.GetUserAsync(User);
                    var checkRoleCurrent = await userManager.IsInRoleAsync(curretUser, "Admin");

                    if (checkRoleCurrent)
                    {
                        var userBrand = await unitOfWork.UserBranch.GetFirstOrDefault(x => x.UserId == curretUser.Id);
                        if (userBrand == null)
                        {
                            ViewBag.Service = null;
                        }
                        else
                        {
                            ViewBag.Service = new SelectList((from s in context.Services
                                                              join c in context.Categories on s.CategoryId equals c.Id
                                                              join cb in context.CategoryBranches on c.Id equals cb.CategoryId
                                                              where s.Status == 1
                                                              where cb.BranchId == userBrand.BranchId
                                                              select new
                                                              {
                                                                  Id = s.Id,
                                                                  Name = s.Name,
                                                              }).ToList(), "Id", "Name");
                        }

                    }
                    else
                    {
                        ViewBag.Service = new SelectList(context.Services.Where(x => x.Status == 1).ToList(), "Id", "Name");
                    }

                    // Mở comment dưới ra làm tiếp cái userBrand.BrandId giùm t nha, xong xem ViewBag lấy đc đúng Service thuộc về Admin đang đăng nhập ko


                    ViewBag.Priority = new SelectList(priority, new String[] { "Low", "Medium", "High" });

                    //return RedirectToAction("Create");

                    //List<User> list = new List<User>();
                    //foreach (var user in users)
                    //{
                    //    if (await userManager.IsInRoleAsync(user, "Employee"))
                    //    {
                    //        list.Add(user);
                    //    }
                    //}

                    //ViewBag.User = new SelectList(list, "Id", "UserName");
                    //ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                    return View(model);
                }

                if (model != null)
                {
                    // Kiểm tra Limit Name lại 100 ký tự, để slug không bị full
                    model.Slug = SlugService.Create(model.Name).ToLower();

                    //Kiểm tra slug exists trên db hay chưa
                    var slug = await unitOfWork.Project.GetFirstOrDefault(x => x.Slug == model.Slug && x.ServiceId == model.ServiceId);
                    if (slug != null)
                    {
                        TempData["msg"] = "This Project has been exists.";
                        TempData["msg_type"] = "danger";

                        ViewBag.List = "List Projects";
                        ViewBag.Controller = "Project";
                        ViewBag.AspAction = "Index";
                        ViewBag.AspSubAction = "Create";
                        ViewBag.Action = "Create Project";

                        List<User> list = new List<User>();
                        foreach (var user in users)
                        {
                            if (await userManager.IsInRoleAsync(user, "Employee"))
                            {
                                list.Add(user);
                            }
                        }

                        ViewBag.User = new SelectList(list, "Id", "UserName");
                        ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");
                        return View(model);
                    }

                    string imageName = "default.jpg";
                    if (model.ImageUpload != null)
                    {
                        foreach (var item in ModelState)
                        {
                            if (item.Key == "ImageUpload")
                            {
                                if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                                {
                                    TempData["msg"] = "Only accept extension image: .jpg, .png ";
                                    TempData["msg_type"] = "danger";

                                    ViewBag.List = "List Projects";
                                    ViewBag.Controller = "Project";
                                    ViewBag.AspAction = "Index";
                                    ViewBag.AspSubAction = "Create";
                                    ViewBag.Action = "Create Project";

                                    List<User> list = new List<User>();
                                    foreach (var user in users)
                                    {
                                        if (await userManager.IsInRoleAsync(user, "Employee"))
                                        {
                                            list.Add(user);
                                        }
                                    }

                                    ViewBag.User = new SelectList(list, "Id", "UserName");
                                    ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");
                                    return View(model);
                                }
                            }
                        }

                        string uploadDir = Path.Combine(env.WebRootPath, "media/projects");
                        imageName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadDir, imageName);
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        await model.ImageUpload.CopyToAsync(fs);
                        fs.Close();
                    }
                    
                    model.Image = imageName;
                    model.UpdatedBy = User.FindFirstValue(ClaimTypes.Name);
                    model.CreatedAt = DateTime.Now;

                    await unitOfWork.Project.Add(model);
                    await unitOfWork.Save();

                    TempData["msg"] = "Project has been Created.";
                    TempData["msg_type"] = "success";

                }
                return RedirectToAction("Create");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            Project project = await unitOfWork.Project.GetFirstOrDefault(x => x.Id == id);
            try
            {
                if (project == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }

                var users = await unitOfWork.User.GetAll();

                List<User> list = new List<User>();
                foreach (var user in users)
                {
                    if (!await userManager.IsInRoleAsync(user, "Employee"))
                    {
                        list.Add(user);
                    }
                }

                ViewBag.User = new SelectList(list, "Id", "UserName");
                ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                ViewBag.List = "List Projects";
                ViewBag.Controller = "Project";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Details";
                ViewBag.Action = "Project Details";

                return View(project);
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
                var project = await unitOfWork.Project.GetFirstOrDefault(x => x.Id == id);
                if (project == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }

                var users = await unitOfWork.User.GetAll();

                List<User> list = new List<User>();
                foreach (var user in users)
                {
                    if (await userManager.IsInRoleAsync(user, "Employee"))
                    {
                        list.Add(user);
                    }
                }

                ViewBag.User = new SelectList(list, "Id", "UserName", project.UserId);
                ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name", project.ServiceId);


                ViewBag.List = "List Projects";
                ViewBag.Controller = "Project";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Edit";
                ViewBag.Action = "Edit Project";

                return View(project);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("EditPolicy"))]
        public async Task<IActionResult> Edit(Project model)
        {
            try
            {
                

                if (model != null)
                {
                    var project = await unitOfWork.Project.GetFirstOrDefault(x => x.Id == model.Id);

                    if (model.UserId == "0")
                    {
                        TempData["msg"] = "Please choose User.";
                        TempData["msg_type"] = "danger";

                        ViewBag.List = "List Projects";
                        ViewBag.Controller = "Project";
                        ViewBag.AspAction = "Index";
                        ViewBag.AspSubAction = "Edit";
                        ViewBag.Action = "Edit Project";

                        var users = await unitOfWork.User.GetAll();
                        List<User> list = new List<User>();
                        foreach (var user in users)
                        {
                            if (!await userManager.IsInRoleAsync(user, "Employee"))
                            {
                                list.Add(user);
                            }
                        }

                        ViewBag.User = new SelectList(list, "Id", "UserName");
                        ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                        return View(model);
                    }

                    if (model.ServiceId == 0)
                    {
                        TempData["msg"] = "Please choose a Service.";
                        TempData["msg_type"] = "danger";

                        ViewBag.List = "List Projects";
                        ViewBag.Controller = "Project";
                        ViewBag.AspAction = "Index";
                        ViewBag.AspSubAction = "Edit";
                        ViewBag.Action = "Edit Project";

                        var users = await unitOfWork.User.GetAll();
                        List<User> list = new List<User>();
                        foreach (var user in users)
                        {
                            if (!await userManager.IsInRoleAsync(user, "Employee") )
                            {
                                list.Add(user);
                            }
                        }

                        ViewBag.User = new SelectList(list, "Id", "UserName");
                        ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                        return View(model);
                    }

                    if (project != null)
                    {
                        project.Slug = SlugService.Create(model.Name).ToLower();

                        var projectDb = await unitOfWork.Project.GetAll(x => x.Id != model.Id);

                        foreach (var itemDb in projectDb)
                        {
                            //Kiểm tra slug exists trên db hay chưa, ngoại trừ Id hiện tại và slug trùng phải cùng Service
                            var slug = await context.Projects.Where(x => x.Id != model.Id).FirstOrDefaultAsync(p => p.Slug == project.Slug && p.ServiceId == model.ServiceId);

                            if (slug != null)
                            {
                                var users = await unitOfWork.User.GetAll();
                                List<User> list = new List<User>();
                                foreach (var user in users)
                                {
                                    if (!await userManager.IsInRoleAsync(user, "Employee"))
                                    {
                                        list.Add(user);
                                    }
                                }

                                ViewBag.User = new SelectList(list, "Id", "UserName");
                                ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                                TempData["msg"] = "This Project already exists in this Service";
                                TempData["msg_type"] = "danger";

                                ViewBag.List = "List Projects";
                                ViewBag.Controller = "Project";
                                ViewBag.AspAction = "Index";
                                ViewBag.AspSubAction = "Edit";
                                ViewBag.Action = "Edit Project";

                                return View(project);
                            }
                        }

                        if (model.ImageUpload != null)
                        {
                            // Nếu ko Validate Hợp lệ
                            foreach (var item in ModelState)
                            {
                                if (item.Key == "ImageUpload")
                                {
                                    if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                                    {
                                        TempData["msg"] = "Only accept extension image: .jpg, .png ";
                                        TempData["msg_type"] = "danger";

                                        ViewBag.List = "List Services";
                                        ViewBag.Controller = "Service";
                                        ViewBag.AspAction = "Index";
                                        ViewBag.AspSubAction = "Edit";
                                        ViewBag.Action = "Edit Service";

                                        var users = await unitOfWork.User.GetAll();
                                        List<User> list = new List<User>();
                                        foreach (var user in users)
                                        {
                                            if (!await userManager.IsInRoleAsync(user, "Employee"))
                                            {
                                                list.Add(user);
                                            }
                                        }

                                        ViewBag.User = new SelectList(list, "Id", "UserName");
                                        ViewBag.Service = new SelectList(await unitOfWork.Service.GetAll(), "Id", "Name");

                                        return View(project);
                                    }
                                }
                            }


                            string uploadDir = Path.Combine(env.WebRootPath, "media/projects");
                            if (!string.Equals(project.Image, "default.jpg"))
                            {
                                string oldImagePath = Path.Combine(uploadDir, model.Image);
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
                            project.Image = imageName;
                        }
                        project.Name = model.Name;
                        project.Priority = model.Priority;
                        project.UserId = model.UserId;
                        project.ServiceId = model.ServiceId;

                        // Get Current user UserName
                        project.UpdatedBy = User.FindFirstValue(ClaimTypes.Name);
                        project.DueDate = model.DueDate;                                                                    
                        project.UpdatedAt = DateTime.Now;
                        context.Projects.Update(project);
                        await unitOfWork.Save();

                        TempData["msg"] = "This Project has been Updated.";
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

        [Authorize(Policy = ("DeletePolicy"))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await unitOfWork.Project.GetFirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    TempData["msg"] = "Project does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }


                unitOfWork.Project.Remove(model);
                await unitOfWork.Save();

                TempData["msg"] = "Project has been Deleted.";
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
