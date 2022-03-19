using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_doctor.Models;
using online_doctor.Repositories;

namespace online_doctor.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserRepository _userRepository;

        public RegistrationController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(User user)
        {
            // TODO Check user by id
            User existUser = _userRepository.GetUserByLogin(user.Login);
            if (existUser != null)
            {
                user.ErrorMessage = "Такой пользователь уже существует";
                return View("Registration", user);
            }

            if (user.UserPassword != user.RepeatUserPassword)
            {
                user.ErrorMessage = "Пароли не совпадают";
                return View("Registration", user);
            }

            _userRepository.RegistrationUser(user);
            existUser = _userRepository.GetUserByLogin(user.Login);

            HttpContext.Session.SetString("Login", existUser.Login);
            HttpContext.Session.SetInt32("UserID", existUser.UserId);
            HttpContext.Session.SetInt32("RoleID", existUser.IdRole);

            return RedirectToAction("Index", "Home");
        }
    }
}
