using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.ViewModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Displays the view of the Index action
        /// </summary>
        /// <returns>the view of the account</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the view of the Register action
        /// </summary>
        /// <returns>the view of the register action</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Creates a new user, assign claims and roles
        /// Sends a welcome email to new users
        /// </summary>
        /// <param name="rvm"></param>
        /// <returns>the RegisterViewModel object of information about the user</returns>
        [AllowAnonymous]
        [HttpPost, ActionName("Register")]
        public async Task<IActionResult> RegisterConfirmed(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                //assign claims we'll be capturing to their respective variables
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
                    Claim firstNameClaim = new Claim("FirstName", $"{user.FirstName}");

                    //add a basket to the user
                    Claim basketClaim = new Claim("basket", $"{user.Email}");

                    //capturing the user's email
                    Claim emailClaim = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);
                    claims.Add(nameClaim);
                    claims.Add(firstNameClaim);
                    claims.Add(emailClaim);

                    //adds claim to the user
                    await _userManager.AddClaimsAsync(user, claims);

                    //assign roles
                    if (user.Email == "doge@gmail.com" || user.Email == "ecaoile@my.hpu.edu")
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                    }

                    await _signInManager.SignInAsync(user, false);

                    //send a welcome email to new users
                    await _emailSender.SendEmailAsync(user.Email, "Welcome",
                        "<h1>Thank you for registering!</h1>" +
                        "<h4>Mister Roboto Arigato is the <i>bestest store</i>!!!</h4>" +
                        "<h4>We hope to fulfill all your <u>robotic</u> needs!</h4>");
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(rvm);
        }

        /// <summary>
        /// Displays the view of the login
        /// </summary>
        /// <returns>the view of the Login action</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Associates the user logging in to a user in the Db and
        /// sends them a Welcome Back email
        /// </summary>
        /// <param name="lvm"></param>
        /// <returns>a LoginViewModel object of information about a user</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, false, false);

                if (result.Succeeded)
                {   //send an email welcoming the user back
                    var user = await _userManager.FindByEmailAsync(lvm.Email);
                    await _emailSender.SendEmailAsync(user.Email, "You've logged in",
                        "<h1><font color='blue'>You must really like our store!</font><h1>" +
                        "<h2>Our featured product this week is the <font color='red'>Mars Rover.</font></h2>" +
                        "<p>Buy one <b>today</b> and you'll be prepared to travel around your new planet in <i>style</i>.</p>");
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

        /// <summary>
        /// Redirects a user, who chooses to use an external login, to that respective site to log in
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>Sends the user to the 3rd party site to login</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        /// <summary>
        /// Captures the claims of a user who chooses to login with an external log in for the first time
        /// The result of the 3rd party login will be passed to this method
        /// Users are sent a Welcome email
        /// </summary>
        /// <param name="remoteError"></param>
        /// <returns>passes an ExternalLoginViewModel of claims captured from a user</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["ErrorMessage"] = "Error from provider";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name);
            string lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);

            await _emailSender.SendEmailAsync(email, "Welcome",
                        "<h1>Thank you for registering!</h1>" +
                        "<h4>Mister Roboto Arigato is the <i>bestest store</i>!!!</h4>" +
                        "<h4>We hope to fulfill all your <u>robotic</u> needs!</h4>");

            return View("ExternalLogin", new ExternalLoginViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            });
        }

        /// <summary>
        /// Assigns the claims to a user who uses a 3rd party OAUTH to log in
        /// </summary>
        /// <param name="elvm"></param>
        /// <returns>directs the users to the index action of the home controller</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel elvm)
        {
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    TempData["Error"] = "Error loading information";
                }

                RegisterViewModel rvm = new RegisterViewModel
                {
                    FirstName = elvm.FirstName,
                    LastName = elvm.LastName,
                    Email = elvm.Email
                };

                var user = new ApplicationUser
                {
                    FirstName = elvm.FirstName,
                    LastName = elvm.LastName,
                    UserName = elvm.Email,
                    Email = elvm.Email
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>();

                    //capturing the user's name
                    Claim nameClaim = new Claim("FullName", $"{user.FirstName} {user.LastName}");
                    Claim firstNameClaim = new Claim("FirstName", $"{user.FirstName}");

                    //add a basket to the user
                    Claim basketClaim = new Claim("basket", $"{user.Email}");

                    //capturing the user's email
                    Claim emailClaim = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);
                    claims.Add(nameClaim);
                    claims.Add(firstNameClaim);
                    claims.Add(emailClaim);

                    //adds claim to the user
                    await _userManager.AddClaimsAsync(user, claims);

                    if (user.Email == "doge@gmail.com" || user.Email == "ecaoile@my.hpu.edu")
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                    }

                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Register");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Logs a user out
        /// </summary>
        /// <returns>Directs the user back to the index action of the home controller</returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["LoggedOut"] = "User Logged Out";

            return RedirectToAction("Index", "Home");
        }
    }
}