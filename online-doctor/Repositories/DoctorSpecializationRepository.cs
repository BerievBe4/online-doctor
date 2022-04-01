using Dapper;
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

        public DoctorSpecialization GetDoctorSpecializationsByName(string doctorSpecializations)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DoctorSpecializations", doctorSpecializations);
            return ReturnList<DoctorSpecialization>("GetDoctorSpecializationsByName", param).FirstOrDefault<DoctorSpecialization>();
        }

        public void AddDoctorSpecialization(DoctorSpecialization doctorSpecialization)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DoctorSpecializations", doctorSpecialization.DoctorType);
            ExecuteWithoutReturn("AddDoctorSpecialization", param);
        }
    }
}
