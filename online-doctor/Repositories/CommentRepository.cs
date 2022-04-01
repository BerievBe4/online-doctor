using Dapper;
using online_doctor.Models;
using online_doctor.Providers;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Repositories
{
    public class CommentRepository : Repository
{
    public CommentRepository(IDbConnectionProvider connectionProvider) :
        base(connectionProvider)
    { }

    public void AddComment(Comment comment)
    {
        DynamicParameters param = new DynamicParameters();
        param.Add("@IdUser", comment.IdUser);
        param.Add("@IdDoctor", comment.IdDoctor);
        //param.Add("@PostedDate", comment.PostedDate);
        param.Add("@CommentContent", comment.CommentText);
        ExecuteWithoutReturn("AddComment", param);
    }

    public List<Comment> GetCommentsByDoctorId(int doctorId)
    {
        DynamicParameters param = new DynamicParameters();
        param.Add("@DoctorId", doctorId);
        return ReturnList<Comment>("GetCommentsByDoctorId", param).ToList<Comment>();
    }
}
}
