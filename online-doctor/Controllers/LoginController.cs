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
        private readonly DoctorRepository _doctorRepository;

        public LoginController(UserRepository userRepository, DoctorRepository doctorRepository)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        // TODO inheritance from IUser (doctor and common user)
        public IActionResult Login(User user)
        {
            if (user.IsDoctor)
            {
                Doctor existDoctor = _doctorRepository.GetDoctorByLogin(user.Login);
                if (existDoctor == null)
                {
                    user.ErrorMessage = "Неправильный логин или пароль.";
                    return View("Login", user);
                }

                if (!BCrypt.Net.BCrypt.Verify(user.UserPassword, existDoctor.DoctorPassword))
                {
                    user.ErrorMessage = "Неправильный логин или пароль.";
                    return View("Login", user);
                }

                HttpContext.Session.SetString("Login", existDoctor.Login);
                HttpContext.Session.SetInt32("UserID", existDoctor.DoctorId);
                HttpContext.Session.SetInt32("RoleID", 3);
            }
            else
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
            }

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
