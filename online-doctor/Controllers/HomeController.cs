using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using online_doctor.Models;
using online_doctor.Repositories;
using System;
using System.Collections.Generic;

namespace online_doctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly DoctorRepository _doctorRepository;
        private readonly DoctorSpecializationRepository _doctorSpecializationRepository;
        private readonly AppoitmentTypeRepository _appointmentType;

        void loadDoctorSpecializations()
        {
            List<DoctorSpecialization> doctorSpecializations = _doctorSpecializationRepository.GetDoctorSpecializations();
            doctorSpecializations.Insert(0, new DoctorSpecialization { DocTypeId = 0, DoctorType = "-" });

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

        public HomeController(DoctorRepository doctorRepository, DoctorSpecializationRepository doctorSpecializationRepository, AppoitmentTypeRepository appointmentType)
        {
            _doctorRepository = doctorRepository;
            _doctorSpecializationRepository = doctorSpecializationRepository;
            _appointmentType = appointmentType;
        }

        public IActionResult Index()
        {
            loadDoctorSpecializations();
            loadSortTypes();

            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            List<Doctor> doctors = _doctorRepository.GetAllDoctors();

            return View(doctors);
        }

        public IActionResult More(int doctorId)
        {
            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            ViewBag.UserID = HttpContext.Session.GetInt32("UserID");

            Doctor doctor = _doctorRepository.GetDoctorById(doctorId);
            return View(doctor);
        }

        [HttpGet]
        public IActionResult CreateAppoitment(int userID, int doctorID)
        {
            List<AppointmentType> appointmentTypes = _appointmentType.GetAppoitmentTypes();
            ViewBag.AppoitmentType = new SelectList(appointmentTypes, "TypeId", "Type");

            Appointment appointment = new Appointment();
            appointment.IdDoctor = doctorID;
            appointment.IdUser = userID;

            appointment.AvailableAppoitmentTimes.Add(new AppointmentTime { Id = 1, Time = new DateTime(2010, 10, 10, 15, 0, 0) });
            appointment.AvailableAppoitmentTimes.Add(new AppointmentTime { Id = 1, Time = new DateTime(2025, 10, 10, 15, 0, 0) });

            return View(appointment);
        }

        [HttpPost]
        public IActionResult CreateAppoitment(Appointment appointment)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
