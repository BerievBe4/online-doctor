using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class DoctorWorkingHours
    {
        public int DoctorId { get; set; }

        [Required]
        [Display(Name = "Время начала работы")]
        public TimeSpan StartHour { get; set; }
        [Required]
        [Display(Name = "Время окончания работы")]
        public TimeSpan EndHour { get; set; }

        [Required]
        [Display(Name = "День недели")]
        public List<DayOfWeek> DayOfWeeks { get; set; } = new List<DayOfWeek>();
        public int IdDayOfWeek { get; set; }

        public string Day { get; set; }

        public string ErrorMessage { get; set; }
    }
}
