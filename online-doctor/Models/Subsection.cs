using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class Subsection
    {
        public int SubsectionId { get; set; }

        [Required]
        [Display(Name = "Название подсекции")]
        public string SubsectionName { get; set; }

        [Display(Name = "Секции")]
        public List<Section> Sections = new List<Section>();
        public int IdSection { get; set; }

        public string ErrorMessage { get; set; }
    }
}
