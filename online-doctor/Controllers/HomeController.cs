using Microsoft.AspNetCore.Http;
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
        private readonly AppoitmentTypeRepository _appointmentTypeRepository;
        private readonly AppointmentRepository _appointmentRepository;

        void loadDoctorSpecializations()
        {
            List<DoctorSpecialization> doctorSpecializations = _doctorSpecializationRepository.GetDoctorSpecializations();
            doctorSpecializations.RemoveAt(0);
            doctorSpecializations.Insert(0, new DoctorSpecialization { DocTypeId = 0, DoctorType = "-" });

            ViewBag.doctorSpecializations = new SelectList(doctorSpecializations, "DocTypeId", "DoctorType");
        }
        void loadSortTypes()
        {
            List<SortType> sortTypes = new List<SortType>();
            sortTypes.Add(new SortType { Id = 0, Type = "-" });
            sortTypes.Add(new SortType { Id = 1, Type = "Возрастание" });
            sortTypes.Add(new SortType { Id = 2, Type = "Убывание" });

            ViewBag.sortTypes = new SelectList(sortTypes, "Id", "Type");
        }

        public HomeController(DoctorRepository doctorRepository, DoctorSpecializationRepository doctorSpecializationRepository, AppoitmentTypeRepository appointmentType, AppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorSpecializationRepository = doctorSpecializationRepository;
            _appointmentTypeRepository = appointmentType;
            _appointmentRepository = appointmentRepository;
        }

        public IActionResult Index()
        {
            loadDoctorSpecializations();
            loadSortTypes();

            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            List<Doctor> doctors = _doctorRepository.GetAllDoctors();

            return View(doctors);
        }

        public IActionResult EvaluateDoctor(int rating, int DoctorId)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserID");
            _doctorRepository.SetDoctorRating(userId, DoctorId, rating);

            loadDoctorSpecializations();
            loadSortTypes();

            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            List<Doctor> doctors = _doctorRepository.GetAllDoctors();

            return View("Index", doctors);
        }

        public IActionResult SortDoctor(int doctorTypeId, int sortTypeId)
        {
            loadDoctorSpecializations();
            loadSortTypes();

            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            List<Doctor> doctors = new List<Doctor>();

            if (doctorTypeId != 0 && sortTypeId == 0)
                doctors = _doctorRepository.GetAllDoctorsByType(doctorTypeId);
            if (doctorTypeId == 0 && sortTypeId != 0)
                doctors = _doctorRepository.GetAllDoctorsByRating(sortTypeId == 2);
            if (doctorTypeId != 0 && sortTypeId != 0)
                doctors = _doctorRepository.GetAllDoctorInfoSortingByRatingAndType(doctorTypeId, (sortTypeId == 2));

            return View("Index", doctors);
        }

        public IActionResult More(int doctorId)
        {
            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            ViewBag.UserID = HttpContext.Session.GetInt32("UserID");
            int? userId = (int?)HttpContext.Session.GetInt32("UserID");

            Doctor doctor = _doctorRepository.GetDoctorById(doctorId);

            // TODO Exception
            doctor.DoctorWorkingHours = _doctorRepository.GetDoctorWorkingHours(doctorId);

            if (userId != null)
                ViewBag.Rating = _doctorRepository.GetDoctorRatingByUserIdAndDoctorId((int)userId, doctorId);

            return View(doctor);
        }

        [HttpGet]
        public IActionResult CreateAppoitment(int userID, int doctorID)
        {
            List<AppointmentType> appointmentTypes = _appointmentTypeRepository.GetAppoitmentTypes();
            ViewBag.AppoitmentType = new SelectList(appointmentTypes, "TypeId", "Type");

            Appointment appointment = new Appointment();
            appointment.IdDoctor = doctorID;
            appointment.IdUser = userID;

            return View(appointment);
        }

        [HttpPost]
        public IActionResult CreateAppoitment(Appointment appointment)
        {
            // TODO validate appointment
            appointment.AppointedEnd = appointment.AppointedStart.AddHours(1);
            _appointmentRepository.AddAppointment(appointment);

            return RedirectToAction("Index", "Home");
        }
    }
}
