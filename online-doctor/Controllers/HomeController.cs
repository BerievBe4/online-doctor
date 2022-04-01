using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using online_doctor.Filters;
using online_doctor.Models;
using online_doctor.Repositories;
using online_sdoctor.Filters;
using System;
using System.Collections.Generic;

namespace online_doctor.Controllers
{
    public class HomeController : Controller
    {
        private readonly DoctorRepository _doctorRepository;
        private readonly DoctorSpecializationRepository _doctorSpecializationRepository;
        private readonly AppoitmentTypeRepository _appointmentTypeRepository;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly CommentRepository _commentRepository;

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

        public HomeController(DoctorRepository doctorRepository, DoctorSpecializationRepository doctorSpecializationRepository, AppoitmentTypeRepository appointmentType, AppointmentRepository appointmentRepository, CommentRepository commentRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorSpecializationRepository = doctorSpecializationRepository;
            _appointmentTypeRepository = appointmentType;
            _appointmentRepository = appointmentRepository;
            _commentRepository = commentRepository;
        }

        public IActionResult Index()
        {
            loadDoctorSpecializations();
            loadSortTypes();

            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            List<Doctor> doctors = _doctorRepository.GetAllDoctors();

            return View(doctors);
        }

        [DataFilter]
        [IsExistUser]
        public IActionResult EvaluateDoctor(int rating, int DoctorId)
        {
            _doctorRepository.SetDoctorRating(ViewBag.UserID, DoctorId, rating);

            loadDoctorSpecializations();
            loadSortTypes();

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
            else if (doctorTypeId == 0 && sortTypeId != 0)
                doctors = _doctorRepository.GetAllDoctorsByRating(sortTypeId == 1);
            else if (doctorTypeId != 0 && sortTypeId != 0)
                doctors = _doctorRepository.GetAllDoctorInfoSortingByRatingAndType(doctorTypeId, (sortTypeId == 2));
            else
                doctors = _doctorRepository.GetAllDoctors();

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

            ViewBag.Comments = _commentRepository.GetCommentsByDoctorId(doctorId);

            return View(doctor);
        }

        [IsExistUser]
        [HttpGet]
        public IActionResult CreateAppoitment(int userID, int doctorID)
        {
            List<AppointmentType> appointmentTypes = _appointmentTypeRepository.GetAppoitmentTypes();
            ViewBag.AppoitmentType = new SelectList(appointmentTypes, "TypeId", "Type");

            Appointment appointment = new Appointment();
            appointment.IdDoctor = doctorID;
            appointment.IdUser = userID;
            appointment.AppointedStart = DateTime.Now;

            return View(appointment);
        }

        [IsExistUser]
        [HttpPost]
        public IActionResult CreateAppoitment(Appointment appointment)
        {
            Appointment existAppointment = _appointmentRepository.GetAppointmentByStartTime(appointment.AppointedStart);
            if (existAppointment != null)
            {
                appointment.ErrorMessage = "Такое время уже заннято.";
                return View("CreateAppoitment", appointment);
            }

            appointment.AppointedEnd = appointment.AppointedStart.AddHours(1);
            _appointmentRepository.AddAppointment(appointment);

            return RedirectToAction("Index", "Home");
        }

        [IsExistUser]
        [HttpGet]
        public IActionResult SetPayment(int appointmentId)
        {
            return View();
        }

        [IsExistUser]
        [HttpPost]
        public IActionResult SetPayment(Payment payment)
        {
            _appointmentRepository.SetIsPayment(payment.AppointmentId, true);
            return RedirectToAction("Index");
        }
    }
}
