using System;
using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class Doctor
    {
        [Display(Name = "Фамилия имя отчество")]
        public string FIO { get; set; }
        public string Email { get; set; }
        public string PathToPhoto { get; set; }
        public string About { get; set; }
        public string Education { get; set; }
        public DateTime Birthday { get; set; }

        [Display(Name = "Рейтинг")]
        public float? Rating { get; set; }
        [Display(Name = "Специализация")]
        public string DoctorType { get; set; }
    }
}
