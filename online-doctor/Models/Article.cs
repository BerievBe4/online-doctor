namespace online_doctor.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        public string ArticleName { get; set; }
        public string ArticleText { get; set; }
        public string Authors { get; set; }

        public bool Approved { get; set; }
        public int IdSubsection { get; set; }

        public string ErrorMessage { get; set; }
    }
}