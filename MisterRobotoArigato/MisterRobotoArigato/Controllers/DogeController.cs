using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MisterRobotoArigato.Controllers
{
    /// <summary>
    /// Directs a user to a special index page if they meet a certain policy requirement
    /// </summary>
    [Authorize(Policy = "IsDoge")]
    public class DogeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}