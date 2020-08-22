using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Models;
using Clinic.Models.ViewModels;
using Clinic.Views.Admonstrator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Clinic.Controllers
{
    [Authorize(Roles ="Admins")]
    public class AdmonstratorController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdmonstratorController(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = model.Name;
                IdentityResult result = await roleManager.CreateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction("Roles", "Admonstrator");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("CreateRole");
        }

        public IActionResult Roles()
        {
            var roles = roleManager.Roles;
            return View("roles",roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if(role==null)
            {
                ViewBag.ErrorMessage = "Not Found";
                return View("NotGound");
            }
            var model = new EfitRoleViewMode
            {
                ID = role.Id,
                Name = role.Name
            };

            foreach (var User in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(User,role.Name))
                {
                    model.Users.Add(User.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EfitRoleViewMode model)
        {
            var role = await roleManager.FindByIdAsync(model.ID);
            if (role == null)
            {
                ViewBag.ErrorMessage = "Not Found";
                return View("NotGound");
            }
            else
            {
                role.Name = model.Name;
                var result = await roleManager.UpdateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string RoleId)
        {
            ViewBag.RoleId = RoleId;
            var role = await roleManager.FindByIdAsync(RoleId);
            if(role==null)
            {
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewMode = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if(await userManager.IsInRoleAsync(user,role.Name))
                {
                    userRoleViewMode.IsSelected = true;
                }
                else
                {
                    userRoleViewMode.IsSelected = false;
                }
                model.Add(userRoleViewMode);
            }
            return View("EditUsersInRole",model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel>model,string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                return View("NotFound");
            }
            for(int i=0;i<model.Count;i++)
            {
                var user =await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if(model[i].IsSelected && !(await userManager.IsInRoleAsync(user,role.Name)))
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
                if(result.Succeeded)
                {
                    if (i < model.Count - 1)
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = RoleId });
                }

            }
            return RedirectToAction("EditRole", new { Id = RoleId });
        }

    }
}