using Dapper;
using online_doctor.Models;
using online_doctor.Providers;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Repositories
{
    public class AppointmentRepository : Repository
    {
        public AppointmentRepository(IDbConnectionProvider connectionProvider) :
            base(connectionProvider)
        { }

        public List<Appointment> GetAppointmentsByUserId(int UserId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", UserId);
            return ReturnList<Appointment>("GetAppointmentsByUserId", param).ToList<Appointment>();
        }

        public List<Appointment> GetAppointmentsByDoctorId(int DoctorId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DoctorId", DoctorId);
            return ReturnList<Appointment>("GetAppointmentsByUserId", param).ToList<Appointment>();
        }

        public List<Appointment> AddAppointment(Appointment appointment)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DoctorId", appointment.IdDoctor);
            param.Add("@UserId", appointment.IdUser);
            param.Add("@TypeId", appointment.IdType);
            param.Add("@AppointedStart", appointment.AppointedStart);
            param.Add("@AppointedEnd", appointment.AppointedEnd);
            return ReturnList<Appointment>("GetAppointmentsByUserId", param).ToList<Appointment>();
        }
    }
}
