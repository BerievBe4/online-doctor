using System.Data;

namespace online_doctor.Providers
{
    public interface IDbConnectionProvider
    {
        IDbConnection Open();
    }
}
