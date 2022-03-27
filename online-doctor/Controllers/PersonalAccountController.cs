﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_doctor.Models;
using online_doctor.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace online_doctor.Controllers
{
    public class PersonalAccountController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly DoctorRepository _doctorRepository;
        private readonly DoctorSpecializationRepository _doctorSpecializationRepository;
        private readonly DayOfWeekRepository _dayOfWeekRepository;

        private readonly IHostingEnvironment _hostingEnvironment;

        public PersonalAccountController(UserRepository userRepository, DoctorRepository doctorRepository, DoctorSpecializationRepository doctorSpecializationRepository, DayOfWeekRepository dayOfWeekRepository, IHostingEnvironment hostingEnvironment)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
            _doctorSpecializationRepository = doctorSpecializationRepository;
            _dayOfWeekRepository = dayOfWeekRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            ViewBag.UserID = HttpContext.Session.GetInt32("UserID");
            ViewBag.Login = HttpContext.Session.GetString("Login");

            return View();
        }

        //[is doctor]
        [HttpGet]
        public IActionResult ChangeDoctorInformation(int doctorId)
        {
            Doctor doctor = _doctorRepository.GetDoctorById(doctorId);
            List<DoctorSpecialization> doctorSpecializations = _doctorSpecializationRepository.GetDoctorSpecializations();
            doctorSpecializations.RemoveAt(0);

            doctor.DoctorSpecializations = doctorSpecializations;

            return View(doctor);
        }

        [HttpPost]
        public IActionResult ChangeDoctorInformation(Doctor doctor)
        {
            if (doctor.ImageFile != null)
            {
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(doctor.ImageFile.FileName);
                string extention = Path.GetExtension(doctor.ImageFile.FileName);
                string safeFileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
                string path = Path.Combine(wwwRootPath + "/images", safeFileName);
                doctor.Photo = safeFileName;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    doctor.ImageFile.CopyTo(fileStream);
                }
            }
            else
            {
                doctor.Photo = "noImage.jpg";
            }

            _doctorRepository.EditDoctor(doctor);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ChangeDoctorShedule(int doctorId)
        {
            DoctorWorkingHours doctorWorkingHours = new DoctorWorkingHours();
            doctorWorkingHours.DayOfWeeks = _dayOfWeekRepository.GetDayOfWeeks();
            doctorWorkingHours.DoctorId = doctorId;

            return View(doctorWorkingHours);
        }

        [HttpPost]
        public IActionResult ChangeDoctorShedule(DoctorWorkingHours doctorWorkingHours)
        {
            DoctorWorkingHours existDoctorWorkingHours = _doctorRepository.GetDoctorWorkingHouseByDayofWeekId(doctorWorkingHours.DoctorId, doctorWorkingHours.IdDayOfWeek);
            if (existDoctorWorkingHours.StartHour > existDoctorWorkingHours.EndHour)
            {
                existDoctorWorkingHours.ErrorMessage = "Неккоректное время";
                return View("ChangeDoctorShedule", existDoctorWorkingHours);
            }

            if (existDoctorWorkingHours != null)
            {
                _doctorRepository.UpdateDoctorWorkingHourByDayOfWeekId(doctorWorkingHours);

                return RedirectToAction("Index", "PersonalAccount");
            }

            _doctorRepository.AddDoctorWorkingHouse(existDoctorWorkingHours);
            return RedirectToAction("Index", "PersonalAccount");
        }
    }
}
