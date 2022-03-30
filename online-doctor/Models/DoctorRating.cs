using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace online_doctor.Models
{
    public class DoctorRating
    {
        public int IdUser { get; set; }
        public int IdDoctor { get; set; }

        public float? Rating { get; set; }
    }
}
