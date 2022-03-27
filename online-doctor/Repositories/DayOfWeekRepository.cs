using online_doctor.Providers;
using online_doctor.Models;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Repositories
{
    public class DayOfWeekRepository : Repository
    {
        public DayOfWeekRepository(IDbConnectionProvider connectionProvider) :
            base(connectionProvider)
        { }

        public List<DayOfWeek> GetDayOfWeeks()
        {
            List<DayOfWeek> dayOfWeeks = ReturnList<DayOfWeek>("GetDayOfWeeks", null).ToList<DayOfWeek>();
            return dayOfWeeks;
        }
    }
}
