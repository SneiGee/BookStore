using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookStore.Areas.Admin.Repository;
using BookStore.Data;
using BookStore.Models;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    public class HomeController : Controller
    {
        private readonly RoleRepository _roleRepository = null;

        public HomeController(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // [Authorize(Roles = "Users")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            // retrive all role member
            var result = await _roleRepository.AllRoleAsync();
            return View(result);
        }

        [Route("users")]
        public async Task<IActionResult> UserListView()
        {
            // display or retrieve all users
            var result = await _roleRepository.AllUserAsync();
            ViewBag.IsSuccess = true;
            return View(result);
        }

    }
}