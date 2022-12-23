﻿using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;

namespace WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin, Employee")]
    public class CategoryBranchController : Controller
    {
        private readonly StarSecurityDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryBranchController(StarSecurityDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int p =1)
        {
            try
            {
                var model = await _unitOfWork.CategoryBranch.GetAll(includeProperties:"Branch,Category");
                
                int pageSize = 6;
                ViewBag.PageNumber = p;
                ViewBag.PageRange = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.CategoryBranches.Count() / pageSize);

                

                return View(model.Skip((p - 1) * pageSize).Take(pageSize));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }
        [Authorize(Roles = "SuperAdmin ,GeneralAdmin ,Admin")]
        public async Task<IActionResult> OnChangeCategoryBranch()
        {           
            try
            {

                //var branch = await _unitOfWork.Branch.GetAll();
                //var category = await _unitOfWork.Category.GetAll();
                //ViewBag.Category = category;
                //ViewBag.Branch = branch;


                ViewCategoryBranch viewCategoryBranch = new ViewCategoryBranch();
                viewCategoryBranch.Branches = _context.Branches.ToList();
                viewCategoryBranch.Categories = _context.Categories.ToList();


                return View(viewCategoryBranch);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Admin" });
            }
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnChangeCategoryBranch(ViewCategoryBranch model)
        {                                 
            try
            {
                if (model.BranchId == 0)
                {
                    TempData["msg"] = "Please choose Branch.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("OnChangeCategoryBranch");
                }

                // Edit, branch co san, them category vao branch
                if (model.CategoryId == null)
                {
                    TempData["msg"] = "Please choose Category.";
                    TempData["msg_type"] = "danger";
                    return RedirectToAction("OnChangeCategoryBranch");
                }
                foreach (var item in model.CategoryId)
                {
                    var categoryBranchDb = await _unitOfWork.CategoryBranch.GetFirstOrDefault(x => x.BranchId == model.BranchId && x.CategoryId == item);
                    if (categoryBranchDb == null)
                    {
                        var categoryBranch = new CategoryBranch();
                        categoryBranch.BranchId = model.BranchId;
                        categoryBranch.CategoryId = item;

                        await _unitOfWork.CategoryBranch.Add(categoryBranch);
                        await _unitOfWork.Save();
                    }
               
                }

                TempData["msg"] = "Updated Category in Branch success.";
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
