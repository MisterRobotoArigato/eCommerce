using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                List<Claim> claims = new List<Claim>();
                var user = new ApplicationUser
                {
                    UserName = rvm.Email,
                    Email = rvm.Email,
                    FirstName = rvm.FirstName,
                    LastName = rvm.LastName
                };
                //creates user in the database
                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    //capturing the user's name
                    Claim nameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
                    //capturing the user's email
                    Claim emailClaim = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);
                    claims.Add(nameClaim);
                    claims.Add(emailClaim);
                    //adds claim to the user
                    await _userManager.AddClaimsAsync(user, claims);

                    if (user.Email == "doge@gmail.com")
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                    }

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(rvm);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(lvm.Email);
                    if (await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You don't know your credentials.");
                }
            }

            return View(lvm);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["LoggedOut"] = "User Logged Out";

            return RedirectToAction("Index", "Home");
        }
    }
}
