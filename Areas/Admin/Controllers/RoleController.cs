using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookStore.Areas.Admin.Repository;
using BookStore.Areas.Admin.Models;
using BookStore.Models;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleRepository _roleRepository = null;

        public RoleController(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager,
            RoleRepository roleRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleRepository = roleRepository;
        }

        [Route("add-role")]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost("add-role")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RoleModel roleModel)
        {
            // create new role
            if (ModelState.IsValid)
            {
                var role = await _roleRepository.AddNewRole(roleModel);
                if (role.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    return RedirectToAction("Index", "Admin");
                }

                foreach (var error in role.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(roleModel);
        }

        [HttpGet("users/update-user")]
        public async Task<IActionResult> UpdateUserData(string id)
        {
            var user = await _roleRepository.GetUserById(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = true;
                // return View("Notfound");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost("users/update-user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserData(EditUserModel editUserModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleRepository.EditUserAsync(editUserModel);

                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = $"User data has been updated successfully!";
                    return RedirectToAction("UserListView", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(editUserModel);
        }

        [HttpGet("users/update-user/manage-user")]
        public async Task<IActionResult> ManageUserRole(string userId)
        {
            ViewBag.userId = userId;

            var user = await _roleRepository.GetUserById(userId);
            if (user == null)
            {
                ViewBag.isError = true;
                return View("UserListView");
            }

            var model = new List<ManageUserRoleModel>();

            foreach (var role in _roleManager.Roles)
            {
                var manageUserRole = new ManageUserRoleModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    manageUserRole.IsSelected = true;
                }
                else
                {
                    manageUserRole.IsSelected = false;
                }

                model.Add(manageUserRole);
            }

            return View(model);
        }

        [HttpPost("users/update-user/manage-user")]
        public async Task<IActionResult> ManageUserRole(List<ManageUserRoleModel> model, string userId)
        {
            var user = await _roleRepository.GetUserById(userId);
            if (user == null)
            {
                ViewBag.isError = true;
                return View("UserListView");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            var result1 = await _userManager.AddToRolesAsync(user, 
                model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result1.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("UpdateUserData", new { Id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _roleRepository.DeleteUserAsync(id);
            if (result != null)
            {
                if (result.Succeeded)
                {
                    return RedirectToAction("UserListView", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("UserListView");
            }

            return View("UserListView");
            
        }
    }
}