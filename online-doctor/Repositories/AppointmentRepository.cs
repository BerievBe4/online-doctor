using Dapper;
using online_doctor.Models;
using online_doctor.Providers;
using System;
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
            return ReturnList<Appointment>("GetAppointmentsByDoctorId", param).ToList<Appointment>();
        }

        public void AddAppointment(Appointment appointment)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DoctorId", appointment.IdDoctor);
            param.Add("@UserId", appointment.IdUser);
            param.Add("@TypeId", appointment.IdType);
            param.Add("@AppointedStart", appointment.AppointedStart);
            param.Add("@AppointedEnd", appointment.AppointedEnd);
            ExecuteWithoutReturn("AddAppointment", param);
        }

        public Appointment GetAppointmentById(int AppointmentId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppointmentId", AppointmentId);
            return ReturnList<Appointment>("GetAppointmentById", param).FirstOrDefault<Appointment>();
        }

        public Appointment GetAppointmentByStartTime(DateTime AppointedStart)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppointedStart", AppointedStart);
            return ReturnList<Appointment>("GetAppointmentByStartTime", param).FirstOrDefault<Appointment>();
        }

        public void SetIsPayment(int appointmentId, bool isPayment)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@AppointmentId", appointmentId);
            param.Add("@isPayment", isPayment);
            ExecuteWithoutReturn("SetIsPayments", param);
        }
    }
}
