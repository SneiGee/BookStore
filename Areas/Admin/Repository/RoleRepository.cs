using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookStore.Areas.Admin.Models;
using BookStore.Models;
using BookStore.Service;

namespace BookStore.Areas.Admin.Repository
{
    public class RoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserService _userService;

        public RoleRepository(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            UserService userService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<IdentityRole>> AllRoleAsync()
        {
            // all role
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> AddNewRole(RoleModel roleModel)
        {
            // implement adding new role
            var role = new IdentityRole()
            {
                Name = roleModel.Name
            };
            var result = await _roleManager.CreateAsync(role);

            return result;
        }

        public async Task<List<UserInRoleModel>> AllUserAsync()
        {
            // retreive or list all users with their role name
            var userlist = new List<UserInRoleModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                string roleName = userRoles.Count() == 0 ? "No user" : userRoles[0];

                userlist.Add(new UserInRoleModel()
                {
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserRole = roleName 
                });
            }
            
            return userlist;
        }

        public async Task<IdentityResult> EditUserAsync(EditUserModel editUserModel)
        {
            var user = await _userManager.FindByIdAsync(editUserModel.Id);
            if (user != null)
            {
                user.FirstName = editUserModel.FirstName;
                user.LastName = editUserModel.LastName;
                user.UserName = editUserModel.Email;
                user.Email = editUserModel.Email;
            }
            
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return await _userManager.DeleteAsync(user);
        }
    }
}