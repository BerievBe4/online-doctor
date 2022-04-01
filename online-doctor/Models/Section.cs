using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class Section
    {
        public int SectionId { get; set; }

        [Required]
        [Display(Name = "Название секции")]
        public string SectionName { get; set; }

        public string ErrorMessage { get; set; }
    }
}
