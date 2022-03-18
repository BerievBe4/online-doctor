using System;
using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Фамилия имя отчество")]
        public string FIO { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail адрес")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime Birthday { get; set; }
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string UserPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Повторение пароля")]
        public string RepeatUserPassword { get; set; }

        public int IdRole { get; set; }

        public string ErrorMessage { get; set; }
    }
}
