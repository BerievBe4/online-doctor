using Microsoft.AspNetCore.Mvc;
using online_doctor.Filters;
using online_doctor.Models;
using online_doctor.Repositories;

namespace online_doctor.Controllers
{
    public class AdminController : Controller
    {
        private readonly DoctorRepository _doctorRepository;
        private readonly DoctorSpecializationRepository _doctorSpecializationRepository;

        public AdminController(DoctorRepository doctorRepository, DoctorSpecializationRepository doctorSpecializationRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorSpecializationRepository = doctorSpecializationRepository;
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

        [IsAuthorizedAdmin]
        [HttpGet]
        public IActionResult AddDoctorSpecialization()
        {
            return View();
        }

        [IsAuthorizedAdmin]
        [HttpPost]
        public IActionResult AddDoctorSpecialization(DoctorSpecialization doctorSpecialization)
        {
            DoctorSpecialization existDoctorSpecialization = _doctorSpecializationRepository.GetDoctorSpecializationsByName(doctorSpecialization.DoctorType);
            if (existDoctorSpecialization != null)
            {
                existDoctorSpecialization.ErrorMessage = "Такая специализация доктора уже существует";
                return View("AddDoctorSpecialization", existDoctorSpecialization);
            }

            _doctorSpecializationRepository.AddDoctorSpecialization(doctorSpecialization);

            return RedirectToAction("Index", "Admin");
        }
    }
}
