using System;

namespace online_doctor.Models
{
    public class DoctorWorkingHours
    {
        public int DoctorId { get; set; }

        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }

        public string StringStartHour { get; set; }
        public string StringEndHour { get; set; }

        public int DayOfWeekId { get; set; }
    }
}
