using System;
using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string DoctorPassword { get; set; }

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

        public string ErrorMessage { get; set; }
    }
}
