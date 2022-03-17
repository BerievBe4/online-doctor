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
            return ReturnList<Doctor>("GetAllDoctors", null).ToList<Doctor>();
        }
    }
}
