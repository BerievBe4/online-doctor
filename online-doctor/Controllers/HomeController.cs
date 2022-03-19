using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using online_doctor.Models;
using online_doctor.Repositories;
using System.Collections.Generic;

namespace online_doctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly DoctorRepository _doctorRepository;
        private readonly DoctorSpecializationRepository _doctorSpecializationRepository;

        void loadDoctorSpecializations()
        {
            List<DoctorSpecialization> doctorSpecializations = _doctorSpecializationRepository.GetDoctorSpecializations();
            doctorSpecializations.Insert(0, new DoctorSpecialization { Id = 0, DoctorType = "-" });

            ViewBag.doctorSpecializations = new SelectList(doctorSpecializations, "Id", "DoctorType");
        }
        void loadSortTypes()
        {
            List<SortType> sortTypes = new List<SortType>();
            sortTypes.Add(new SortType { Id = 0, Type = "-" });
            sortTypes.Add(new SortType { Id = 1, Type = "Возрастание" });
            sortTypes.Add(new SortType { Id = 2, Type = "Убывание" });

            ViewBag.sortTypes = new SelectList(sortTypes, "Id", "Type");
        }

        public HomeController(DoctorRepository doctorRepository, DoctorSpecializationRepository doctorSpecializationRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorSpecializationRepository = doctorSpecializationRepository;
        }

        public IActionResult Index()
        {
            loadDoctorSpecializations();
            loadSortTypes();

            List<Doctor> doctors = _doctorRepository.GetAllDoctors();

            return View(doctors);
        }

        public IActionResult More(int doctorId)
        {
            Doctor doctor = _doctorRepository.GetDoctorById(doctorId);
            return View(doctor);
        }
    }
}
