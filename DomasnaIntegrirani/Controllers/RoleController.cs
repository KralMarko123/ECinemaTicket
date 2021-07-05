using ECinemaTicket.Domain;
using ECinemaTicket.Domain.Identity;
using ECinemaTicket.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DomasnaIntegrirani.Controllers
{
    public class RoleController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ECinemaApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController( RoleManager<IdentityRole> roleManager, IUserRepository userRepository, UserManager<ECinemaApplicationUser> userManager)
        {

            this._userRepository = userRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        //CREATE ROLE
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ManageUserRoles", "Role");
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        //MANAGE USER ROLE
        [HttpGet]
        public IActionResult ManageUserRoles()
        {

            var allUsers = this._userRepository.GetAll().ToList();

            return View(allUsers);
        }


        //GRANT ADMIN
        public async Task<IActionResult> GrantAdminAsync(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = this._userRepository.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            if (await userManager.IsInRoleAsync(user, user.Role))
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, "Admin");

                if (result != null)
                {
                    var secondResult = await userManager.RemoveFromRoleAsync(user, "User");

                    if (secondResult != null)
                    {
                        user.Role = "Admin";
                        _userRepository.Update(user);

                        return RedirectToAction("ManageUserRoles", "Role");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction("ManageUserRoles", "Role");

        }

        //REMOVE ADMIN
        public async Task<IActionResult> RemoveAdminAsync(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = this._userRepository.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            if(await userManager.IsInRoleAsync(user, user.Role))
            {
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, user.Role);

                if (result != null)
                {
                    var secondResult = await userManager.AddToRoleAsync(user, "User");

                    if (secondResult != null)
                    {
                        user.Role = "User";
                        _userRepository.Update(user);

                        return RedirectToAction("ManageUserRoles", "Role");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction("ManageUserRoles", "Role");

        }

    }
}
 