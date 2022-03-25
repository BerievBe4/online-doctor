using online_doctor.Models;
using online_doctor.Providers;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Repositories
{
    public class AppoitmentTypeRepository : Repository
    {
        public AppoitmentTypeRepository(IDbConnectionProvider connectionProvider) :
                base(connectionProvider)
        { }

        public List<AppointmentType> GetAppoitmentTypes()
        {
            return ReturnList<AppointmentType>("GetAppoitmentTypes", null).ToList<AppointmentType>();
        }
    }
}
