using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class Appointment
    {
        public int IdUser { get; set; }
        public int IdDoctor { get; set; }

        List<AppointmentType> appoitmentTypes { get; set; } = new List<AppointmentType>();
        [Display(Name = "Тип приёма")]
        public int IdType { get; set; }

        [Display(Name = "Доктор")]
        public string FIO { get; set; }
        [Display(Name = "Тип приёма")]
        public string AppointmentType { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Начало приёма приёма")]
        public DateTime AppointedStart { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Окончание приёма")]
        public DateTime AppointedEnd { get; set; }

        [Required]
        [Display(Name = "Номер телефона для связи")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Причина записи")]
        public string ReasonAppointment { get; set; }

        [Display(Name = "Оплачено")]
        public bool PayedFor { get; set; }

        public string ErrorMessage { get; set; }
    }
}
