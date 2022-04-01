using System;

namespace online_doctor.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        public int IdUser { get; set; }
        public string UserName { get; set; }

        public int IdDoctor { get; set; }

        public DateTime PostedDate { get; set; }

        public string CommentText { get; set; }
    }
}
