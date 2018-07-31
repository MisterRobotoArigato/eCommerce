using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.ViewModel;

namespace MisterRobotoArigato.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        [BindProperty]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [BindProperty]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }

        public ProfileModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// get user information
        /// </summary>
        /// <returns>no returns</returns>
        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }

        /// <summary>
        /// Updates user info with a post request, adds claims to the new change
        /// </summary>
        /// <returns>no returns</returns>
        public async Task OnPost()
        {
            var user = await _userManager.GetUserAsync(User);
            user.FirstName = FirstName;
            user.LastName = LastName;

            await _userManager.UpdateAsync(user);

            List<Claim> claims = new List<Claim>();
            Claim fullNameclaim = User.Claims.First(c => c.Type == "FullName");
            Claim firstNameclaim = User.Claims.First(c => c.Type == "FirstName");
            Claim newFullNameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
            Claim newFirstNameClaim = new Claim("FirstName", $"{user.FirstName}");

            claims.Add(newFullNameClaim);
            claims.Add(newFirstNameClaim);
            await _userManager.RemoveClaimAsync(user, fullNameclaim);
            await _userManager.RemoveClaimAsync(user, firstNameclaim);
            await _userManager.AddClaimsAsync(user, claims);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, false);
        }
    }
}