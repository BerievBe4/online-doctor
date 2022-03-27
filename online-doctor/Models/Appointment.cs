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

        [Required]
        [Display(Name = "Дата и время приёма")]
        public DateTime AppointedStart { get; set; }
        public DateTime AppointedEnd { get; set; }

        [Display(Name = "Доступное время")]
        public List<AppointmentTime> AvailableAppoitmentTimes { get; set; } = new List<AppointmentTime>();
        public int SelectedTime { get; set; }

        [Required]
        [Display(Name = "Номер телефона для связи")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Причина записи")]
        public string ReasonAppointment { get; set; }

        public bool PayedFor { get; set; }

        public string ErrorMessage { get; set; }
    }
}
