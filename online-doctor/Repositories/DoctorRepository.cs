using online_doctor.Models;
using online_doctor.Providers;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Repositories
{
    public class DoctorRepository : Repository
    {
        public DoctorRepository(IDbConnectionProvider connectionProvider) :
            base(connectionProvider)
        { }

        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = ReturnList<Doctor>("GetAllDoctors", null).ToList<Doctor>();
            foreach (var doctor in doctors)
                doctor.Rating = doctor.Rating == null ? 0.0f : doctor.Rating;

            return doctors;
        }
    }
}
