using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class DoctorSpecialization
    {
        public int DocTypeId { get; set; }

        [Required]
        [Display(Name = "Специализация")]
        public string DoctorType { get; set; }

        public string ErrorMessage { get; set; }
    }
}
