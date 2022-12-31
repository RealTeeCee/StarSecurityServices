using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Data;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin")]
    public class ClientDetailController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<User> userManager;
        private int pageSize = 6;

        public ClientDetailController(StarSecurityDbContext context, IUnitOfWork unitOfWork, IWebHostEnvironment env, UserManager<User> userManager)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
            this.env = env;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            try
            {
                var model = await _unitOfWork.ClientDetail.GetAll(includeProperties:"User,Service");

                ViewBag.PageNumber = p;
                ViewBag.PageRange = this.pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.ClientDetails.Count() / this.pageSize);

                ViewBag.List = "List Clients";
                ViewBag.Controller = "Client";
                ViewBag.AspAction = "Index";

                return View(model.Skip((p - 1) * this.pageSize).Take(this.pageSize));
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
                var users = await _unitOfWork.User.GetAll();

                List<User> list = new List<User>();
                foreach (var user in users)
                {
                    if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                    {
                        bool checkAssignedUserExist = false;
                        var isAssignedUsers = await _context.ClientDetails.Select(x => x.UserId).ToListAsync();
                        foreach (var item in isAssignedUsers)
                        {
                            if (item == user.Id)
                            {
                                checkAssignedUserExist = true;
                            }
                        }
                        if (checkAssignedUserExist == true)
                        {
                            ViewBag.NoUser = "No User Available";
                            continue;
                        }
                        list.Add(user);
                    }
                }

                ViewBag.User = new SelectList(list, "Id", "UserName");
                ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name");

                ViewBag.List = "List Clients";
                ViewBag.Controller = "Client";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Create";
                ViewBag.Action = "Create Client Information";

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
        public async Task<IActionResult> Create(ClientDetail model)
        {
            try
            {
                var users = await _unitOfWork.User.GetAll();

                if (model.UserId == "0")
                {
                    TempData["msg"] = "Please choose User.";
                    TempData["msg_type"] = "danger";

                    ViewBag.List = "List Clients";
                    ViewBag.Controller = "Client";
                    ViewBag.AspAction = "Index";
                    ViewBag.AspSubAction = "Create";
                    ViewBag.Action = "Create Client Information";

                    List<User> list = new List<User>();
                    foreach (var user in users)
                    {
                        if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                        {
                            bool checkAssignedUserExist = false;
                            var isAssignedUsers = await _context.ClientDetails.Select(x => x.UserId).ToListAsync();
                            foreach (var item in isAssignedUsers)
                            {
                                if (item == user.Id)
                                {
                                    checkAssignedUserExist = true;
                                }
                            }
                            if (checkAssignedUserExist == true)
                            {
                                ViewBag.NoUser = "No User Available";
                                continue;
                            }
                            list.Add(user);
                        }
                    }

                    ViewBag.User = new SelectList(list, "Id", "UserName");
                    ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name");

                    return View(model);
                }

                if (model.UserId == "-1")
                {
                    TempData["msg"] = "There is no user Available!.";
                    TempData["msg_type"] = "danger";

                    ViewBag.List = "List Clients";
                    ViewBag.Controller = "Client";
                    ViewBag.AspAction = "Index";
                    ViewBag.AspSubAction = "Create";
                    ViewBag.Action = "Create Client Information";

                    List<User> list = new List<User>();
                    foreach (var user in users)
                    {
                        if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                        {
                            bool checkAssignedUserExist = false;
                            var isAssignedUsers = await _context.ClientDetails.Select(x => x.UserId).ToListAsync();
                            foreach (var item in isAssignedUsers)
                            {
                                if (item == user.Id)
                                {
                                    checkAssignedUserExist = true;
                                }
                            }
                            if (checkAssignedUserExist == true)
                            {
                                ViewBag.NoUser = "No User Available";
                                continue;
                            }
                            list.Add(user);
                        }
                    }

                    ViewBag.User = new SelectList(list, "Id", "UserName");
                    ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name");

                    return View(model);
                }

                if (model.ServiceId == 0)
                {
                    TempData["msg"] = "Please choose a Service.";
                    TempData["msg_type"] = "danger";

                    ViewBag.List = "List Services";
                    ViewBag.Controller = "Service";
                    ViewBag.AspAction = "Index";
                    ViewBag.AspSubAction = "Create";
                    ViewBag.Action = "Create Service";

                    List<User> list = new List<User>();
                    foreach (var user in users)
                    {
                        if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                        {
                            bool checkAssignedUserExist = false;
                            var isAssignedUsers = await _context.ClientDetails.Select(x => x.UserId).ToListAsync();
                            foreach (var item in isAssignedUsers)
                            {
                                if (item == user.Id)
                                {
                                    checkAssignedUserExist = true;
                                }
                            }
                            if (checkAssignedUserExist == true)
                            {
                                ViewBag.NoUser = "No User Available";
                                continue;
                            }
                            list.Add(user);
                        }
                    }

                    ViewBag.User = new SelectList(list, "Id", "UserName");
                    ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name");

                    return View(model);
                }

                if (model != null)
                {
                    model.CreatedAt = DateTime.Now;
                    await _unitOfWork.ClientDetail.Add(model);
                    await _unitOfWork.Save();
                    TempData["msg"] = "ClientDetail has been Created.";
                    TempData["msg_type"] = "success";

                    ViewBag.List = "List Services";
                    ViewBag.Controller = "Service";
                    ViewBag.AspAction = "Index";
                    ViewBag.AspSubAction = "Create";
                    ViewBag.Action = "Create Service";

                    List<User> list = new List<User>();
                    foreach (var user in users)
                    {
                        if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                        {
                            bool checkAssignedUserExist = false;
                            var isAssignedUsers = await _context.ClientDetails.Select(x => x.UserId).ToListAsync();
                            foreach (var item in isAssignedUsers)
                            {
                                if (item == user.Id)
                                {
                                    checkAssignedUserExist = true;
                                }
                            }
                            if (checkAssignedUserExist == true)
                            {
                                ViewBag.NoUser = "No User Available";
                                continue;
                            }
                            list.Add(user);
                        }
                    }

                    ViewBag.User = new SelectList(list, "Id", "UserName");
                    ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name");
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
            ClientDetail clientDetail = await _unitOfWork.ClientDetail.GetFirstOrDefault(x => x.Id == id);
            try
            {
                if (clientDetail == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }

                var users = await _unitOfWork.User.GetAll();

                List<User> list = new List<User>();
                foreach (var user in users)
                {
                    if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                    {
                        list.Add(user);
                    }
                }

                ViewBag.User = new SelectList(list, "Id", "UserName");
                ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name");

                ViewBag.List = "List Client Detail";
                ViewBag.Controller = "ClientDetail";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Details";
                ViewBag.Action = "ClientDetail Details";

                return View(clientDetail);
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
                var clientDetail = await _unitOfWork.ClientDetail.GetFirstOrDefault(x => x.Id == id);
                if (clientDetail == null)
                {
                    return RedirectToAction("Index", "Error", new { area = "Admin" });
                }
                
                var users = await _unitOfWork.User.GetAll();

                List<User> list = new List<User>();
                foreach (var user in users)
                {
                    if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                    {
                        list.Add(user);
                    }
                }

                ViewBag.User = new SelectList(list, "Id", "UserName", clientDetail.UserId);
                ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name", clientDetail.ServiceId);


                ViewBag.List = "List Client Detail";
                ViewBag.Controller = "ClientDetail";
                ViewBag.AspAction = "Index";
                ViewBag.AspSubAction = "Edit";
                ViewBag.Action = "Edit ClientDetail";

                return View(clientDetail);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = ("EditPolicy"))]
        public async Task<IActionResult> Edit(ClientDetail model)
        {
            try
            {
                var users = await _unitOfWork.User.GetAll();

                if (model != null)
                {
                    var clientDetail = await _unitOfWork.ClientDetail.GetFirstOrDefault(x => x.Id == model.Id);

                    if (model.UserId == "0")
                    {
                        TempData["msg"] = "Please choose User.";
                        TempData["msg_type"] = "danger";

                        ViewBag.List = "List Clients";
                        ViewBag.Controller = "Client";
                        ViewBag.AspAction = "Index";
                        ViewBag.AspSubAction = "Edit";
                        ViewBag.Action = "Edit Client Information";

                        List<User> list = new List<User>();
                        foreach (var user in users)
                        {
                            if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                            {
                                list.Add(user);
                            }
                        }

                        ViewBag.User = new SelectList(list, "Id", "UserName");
                        ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name");

                        return View(model);
                    }

                    if (model.ServiceId == 0)
                    {
                        TempData["msg"] = "Please choose a Service.";
                        TempData["msg_type"] = "danger";

                        ViewBag.List = "List Services";
                        ViewBag.Controller = "Service";
                        ViewBag.AspAction = "Index";
                        ViewBag.AspSubAction = "Edit";
                        ViewBag.Action = "Edit Client Information";

                        List<User> list = new List<User>();
                        foreach (var user in users)
                        {
                            if (!await userManager.IsInRoleAsync(user, "GeneralAdmin") && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
                            {
                                list.Add(user);
                            }
                        }

                        ViewBag.User = new SelectList(list, "Id", "UserName");
                        ViewBag.Service = new SelectList(await _unitOfWork.Service.GetAll(), "Id", "Name");

                        return View(model);
                    }



                    if (clientDetail != null)
                    {
                        var clientDetails = await _context.ClientDetails.ToListAsync();
                        bool deleted = false;                        
                        string itemUserId = "";
                        long itemId = 0;
                        string clientDetailUserIdBeforeDetele = "";
                        foreach (var item in clientDetails)
                        {
                            //Check trong Db neu co UserId == model vua Edit UserId va Model vua edit UserId khac voi  Model truoc khi Edit UserId

                            if (item.UserId == clientDetail.UserId && model.UserId != clientDetail.UserId)
                            {
                                clientDetailUserIdBeforeDetele = clientDetail.UserId;
                                //Xoa Model truoc khi Edit UserId
                                _context.Remove(clientDetail);
                                //Them Model sau khi Edit UserId
                                await _unitOfWork.ClientDetail.Add(model);
                                await _unitOfWork.Save();
                                deleted = true;                                
                                itemUserId = item.UserId;
                                itemId = item.Id;
                                //TempData["msg"] = "Client Detail has been Updated.";
                                //TempData["msg_type"] = "success";

                                //return RedirectToAction("Index");
                            }
                        }

                        //Kiem tra trong Db neu co UserId == voi Model vua add UserId vao thi xoa Model chua' UserId do'

                        if (deleted == true)
                        {                                                        

                            var switchClientDetail = await _unitOfWork.ClientDetail.GetFirstOrDefault(x => x.UserId == model.UserId && x.Id != itemId);
                            _context.Remove(switchClientDetail);
                            ClientDetail newClientDetail = new ClientDetail();
                            newClientDetail.Name = switchClientDetail.Name;
                            newClientDetail.Address = switchClientDetail.Address;
                            newClientDetail.ServiceId = switchClientDetail.ServiceId;
                            newClientDetail.Email = switchClientDetail.Email;
                            newClientDetail.UserId = clientDetailUserIdBeforeDetele;
                            newClientDetail.UpdatedAt = DateTime.Now;
                            await _unitOfWork.ClientDetail.Add(newClientDetail);
                            await _unitOfWork.Save();

                            TempData["msg"] = "Client Detail has been Updated.";
                            TempData["msg_type"] = "success";

                            return RedirectToAction("Index");
                        }

                        clientDetail.UserId = model.UserId;
                        clientDetail.ServiceId = model.ServiceId;
                        clientDetail.Address = model.Address;
                        clientDetail.Email = model.Email;
                        clientDetail.Name = model.Name;
                        clientDetail.UpdatedAt = DateTime.Now;                       

                        _context.ClientDetails.Update(clientDetail);
                        await _unitOfWork.Save();

                        TempData["msg"] = "Client Detail has been Updated.";
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
                var model = await _unitOfWork.ClientDetail.GetFirstOrDefault(x => x.Id == id);
                if (model == null)
                {
                    TempData["msg"] = "ClientDetail does not exists.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("Index");
                }
               

                _unitOfWork.ClientDetail.Remove(model);
                await _unitOfWork.Save();

                TempData["msg"] = "ClientDetail has been Deleted.";
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
