using Microsoft.AspNetCore.Mvc;
using online_doctor.Models;
using online_doctor.Repositories;
using System.Collections.Generic;

namespace online_doctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly DoctorRepository _doctorRepository;

        public HomeController(DoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IActionResult Index()
        {
            List<Doctor> doctors = _doctorRepository.GetAllDoctors();

            return View(doctors);
        }
    }
}
