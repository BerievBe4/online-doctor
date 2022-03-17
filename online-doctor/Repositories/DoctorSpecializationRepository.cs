using online_doctor.Models;
using online_doctor.Providers;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Repositories
{
    public class DoctorSpecializationRepository : Repository
    {
        public DoctorSpecializationRepository(IDbConnectionProvider connectionProvider) :
            base(connectionProvider)
        { }

        public List<DoctorSpecialization> GetDoctorSpecializations()
        {
            return ReturnList<DoctorSpecialization>("GetAllDoctorTypes", null).ToList<DoctorSpecialization>();
        }
    }
}
