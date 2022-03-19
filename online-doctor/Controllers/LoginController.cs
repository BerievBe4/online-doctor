using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_doctor.Models;
using online_doctor.Repositories;
using System.Security.Principal;

namespace online_doctor.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;

        public LoginController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            User existUser = _userRepository.GetUserByLogin(user.Login);
            if (existUser == null)
            {
                user.ErrorMessage = "Неправильный логин или пароль.";
                return View("Login", user);
            }

            if (!BCrypt.Net.BCrypt.Verify(user.UserPassword, existUser.UserPassword))
            {
                user.ErrorMessage = "Неправильный логин или пароль.";
                return View("Login", user);
            }

            HttpContext.Session.SetString("Login", existUser.Login);
            HttpContext.Session.SetInt32("UserID", existUser.UserId);
            HttpContext.Session.SetInt32("RoleID", existUser.IdRole);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult LogOut()
        {
            string UserID = HttpContext.Session.GetString("UserID");

            if (!string.IsNullOrEmpty(UserID))
                HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
