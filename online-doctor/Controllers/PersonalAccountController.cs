using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_doctor.Filters;
using online_doctor.Models;
using online_doctor.Repositories;
using online_sdoctor.Filters;
using Syncfusion.XlsIO;
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
        private readonly AppointmentRepository _appointmentRepository;

        private readonly IHostingEnvironment _hostingEnvironment;

        public PersonalAccountController(UserRepository userRepository, DoctorRepository doctorRepository, DoctorSpecializationRepository doctorSpecializationRepository, DayOfWeekRepository dayOfWeekRepository, AppointmentRepository appointmentRepository, IHostingEnvironment hostingEnvironment)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
            _doctorSpecializationRepository = doctorSpecializationRepository;
            _dayOfWeekRepository = dayOfWeekRepository;
            _appointmentRepository = appointmentRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [DataFilter]
        [IsExistUser]
        public IActionResult Index()
        {
            List<Appointment> appointments = new List<Appointment>();

            if (ViewBag.RoleID == 2)
                appointments = _appointmentRepository.GetAppointmentsByUserId(ViewBag.UserID);
            if (ViewBag.RoleID == 3)
                appointments = _appointmentRepository.GetAppointmentsByDoctorId(ViewBag.UserID);

            return View(appointments);
        }

        [DataFilter]
        [IsExistUser]
        public IActionResult AppointmentDetail(int appointmentId)
        {
            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");

            Appointment appointment = _appointmentRepository.GetAppointmentById(appointmentId);
            return View(appointment);
        }

        [IsExistUser]
        public IActionResult GetAppointmentDetailsInExcel(int appointmentId)
        {
            Appointment appointment = _appointmentRepository.GetAppointmentById(appointmentId);

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2007;

                // Create a workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

                // Data
                worksheet.Range["A1:B1"].Merge();
                worksheet.Range["A1:B1"].VerticalAlignment = ExcelVAlign.VAlignCenter;
                worksheet.Range["A1:B1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["A1"].CellStyle.Font.Bold = true;
                worksheet.Range["A1"].Text = "The Best Doctor";

                worksheet.Range["A1:B7"].BorderAround();
                worksheet.Range["A2"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A3"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A4"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A5"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A6"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A7"].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;

                worksheet.Range["A2"].Text = "Доктор:";
                worksheet.Range["A3"].Text = "Тип записи:";
                worksheet.Range["A4"].Text = "Причина записи:";
                worksheet.Range["A5"].Text = "Начало приёма:";
                worksheet.Range["A6"].Text = "Окончание приёма:";
                worksheet.Range["A7"].Text = "Оплачено:";

                worksheet.Range["B2"].Text = appointment.FIO;
                worksheet.Range["B3"].Text = appointment.AppointmentType;
                worksheet.Range["B4"].Text = appointment.ReasonAppointment;
                worksheet.Range["B5"].Text = appointment.AppointedStart.ToString();
                worksheet.Range["B6"].Text = appointment.AppointedEnd.ToString();
                worksheet.Range["B7"].Text = appointment.PayedFor.ToString();

                //Fit column width to data
                worksheet.UsedRange.AutofitColumns();

                // Save the Excel workbook in MemoryStream
                MemoryStream stream = new MemoryStream();

                workbook.SaveAs(stream);

                stream.Position = 0;

                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = "Output.xlsx";
                return fileStreamResult;
            }
        }

        [IsAuthorizedDoctor]
        [HttpGet]
        public IActionResult ChangeDoctorInformation(int doctorId)
        {
            Doctor doctor = _doctorRepository.GetDoctorById(doctorId);
            List<DoctorSpecialization> doctorSpecializations = _doctorSpecializationRepository.GetDoctorSpecializations();
            doctorSpecializations.RemoveAt(0);

            doctor.DoctorSpecializations = doctorSpecializations;

            return View(doctor);
        }

        [IsAuthorizedDoctor]
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

        [IsAuthorizedDoctor]
        [HttpGet]
        public IActionResult ChangeDoctorShedule(int doctorId)
        {
            DoctorWorkingHours doctorWorkingHours = new DoctorWorkingHours();
            doctorWorkingHours.DayOfWeeks = _dayOfWeekRepository.GetDayOfWeeks();
            doctorWorkingHours.DoctorId = doctorId;

            return View(doctorWorkingHours);
        }

        [IsAuthorizedDoctor]
        [HttpPost]
        public IActionResult ChangeDoctorShedule(DoctorWorkingHours doctorWorkingHours)
        {
            if (doctorWorkingHours.StartHour > doctorWorkingHours.EndHour)
            {
                doctorWorkingHours.ErrorMessage = "Неккоректное время";
                return View("ChangeDoctorShedule", doctorWorkingHours);
            }

            DoctorWorkingHours existDoctorWorkingHours = _doctorRepository.GetDoctorWorkingHouseByDayofWeekId(doctorWorkingHours.DoctorId, doctorWorkingHours.IdDayOfWeek);
            if (existDoctorWorkingHours != null)
            {
                _doctorRepository.UpdateDoctorWorkingHourByDayOfWeekId(doctorWorkingHours);
                return RedirectToAction("Index", "PersonalAccount");
            }

            _doctorRepository.AddDoctorWorkingHouse(doctorWorkingHours);
            return RedirectToAction("Index", "PersonalAccount");
        }
    }
}
