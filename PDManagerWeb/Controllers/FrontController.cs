using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PDManagerWeb.Controllers
{
    [Route("")]
    public class FrontController : Controller
    {
        public FrontController()
        {
        }

        [HttpGet("")]
        public IActionResult Auth()
        {
            if (HttpContext.Session.GetInt32("id") == null)
                return View();
            return View("Documents");
        }

        [HttpGet("Documents")]
        public IActionResult Documents()
        {
            if (HttpContext.Session.GetInt32("id") == null)
                return View("Auth");
            return View();
        }

        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            if (HttpContext.Session.GetInt32("id") == null)
                return View("Auth");
            return View();
        }

        [HttpGet("DeAuth")]
        public IActionResult DeAuth()
        {
            HttpContext.Session.Remove("id");
            return View("Auth");
        }
    }
}
