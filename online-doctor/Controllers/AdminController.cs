using Microsoft.AspNetCore.Mvc;
using online_doctor.Models;
using online_doctor.Repositories;

namespace online_doctor.Controllers
{
    public class AdminController : Controller
    {
        private readonly DoctorRepository _doctorRepository;

        public AdminController(DoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddDoctor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDoctor(Doctor doctor)
        {
            Doctor existUser = _doctorRepository.GetDoctorByLogin(doctor.Login);
            if (existUser != null)
            {
                existUser.ErrorMessage = "Такой доктор уже существует";
                return View("AddDoctor", doctor);
            }

            _doctorRepository.RegistrationDoctor(doctor);

            return RedirectToAction("Index", "Admin");
        }
    }
}
