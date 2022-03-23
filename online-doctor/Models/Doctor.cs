using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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

        [Display(Name = "Image")]
        public string Photo { get; set; }
        [Display(Name = "Фотография")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Обо мне")]
        public string About { get; set; }
        [Display(Name = "Образование")]
        public string Education { get; set; }
        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Рейтинг")]
        public float? Rating { get; set; }
        [Display(Name = "Специализация")]
        public string DoctorType { get; set; }

        [Display(Name = "Специализация")]
        public List<DoctorSpecialization> DoctorSpecializations { get; set; } = new List<DoctorSpecialization>();
        public int IdDocType { get; set; }

        public string ErrorMessage { get; set; }
    }
}
