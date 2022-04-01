using Microsoft.AspNetCore.Mvc;
using online_doctor.Filters;
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

        [IsAuthorizedAdmin]
        public IActionResult Index()
        {
            return View();
        }

        [IsAuthorizedAdmin]
        [HttpGet]
        public IActionResult AddDoctor()
        {
            return View();
        }

        [IsAuthorizedAdmin]
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
