using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace online_doctor.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        [Required]
        [Display(Name = "Название статьи")]
        public string ArticleName { get; set; }
        [Required]
        [Display(Name = "Текст статьи")]
        public string ArticleText { get; set; }
        [Required]
        [Display(Name = "Авторы")]
        public string Authors { get; set; }

        public bool Approved { get; set; }

        [Display(Name = "Секция")]
        public List<Section> Sections = new List<Section>();
        [Display(Name = "Секция")]
        public int IdSection { get; set; }

        [Display(Name = "Подсекция")]
        public List<Subsection> Subsections = new List<Subsection>();
        public int IdSubsection { get; set; }

        public string ErrorMessage { get; set; }
    }
}