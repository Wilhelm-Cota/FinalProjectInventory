using FinalProjectInventory.Data;
using FinalProjectInventory.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinalProjectInventory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(AdminUsers loginUser)
        {
            var status = context.Users.Where(m => m.UserName == loginUser.UserName && m.Password == loginUser.Password).FirstOrDefault();
            if (status == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Regions");
            }
        }

        public IActionResult Regions()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}