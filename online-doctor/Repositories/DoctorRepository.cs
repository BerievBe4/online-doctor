using Dapper;
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

        public void RegistrationDoctor(Doctor doctor)
        {
            DynamicParameters param = new DynamicParameters();
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            param.Add("@Login", doctor.Login);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(doctor.DoctorPassword, salt);
            param.Add("@DoctorPassword", hashedPassword);

            ExecuteWithoutReturn("RegistrationDoctor", param);
        }

        public void EditDoctor(Doctor doctor)
        {
            return;
        }

        public Doctor GetDoctorById(int doctorId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DoctorId", doctorId);
            return ReturnList<Doctor>("GetDoctorById", param).FirstOrDefault<Doctor>();
        }

        public Doctor GetDoctorByLogin(string Login)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@login", Login);
            return ReturnList<Doctor>("GetDoctorByLogin", param).FirstOrDefault<Doctor>();
        }
    }
}
