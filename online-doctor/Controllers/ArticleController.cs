using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_doctor.Models;

namespace online_doctor.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            ViewBag.UserID = HttpContext.Session.GetInt32("UserID");
            ViewBag.Login = HttpContext.Session.GetString("Login");

            return View();
        }

        [HttpGet]
        public IActionResult AddSection()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSection(Section article)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddSubsection()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSubsection(Subsection article)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddArticle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddArticle(Article article)
        {
            return RedirectToAction("Index");
        }
    }
}
