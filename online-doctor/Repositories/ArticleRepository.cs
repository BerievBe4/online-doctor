using Dapper;
using online_doctor.Models;
using online_doctor.Providers;
using online_doctor.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Repositories
{
    public class ArticleRepository : Repository
    {
        public ArticleRepository(IDbConnectionProvider connectionProvider) :
            base(connectionProvider)
        { }

        public List<Section> GetAllSections()
        {
            return ReturnList<Section>("GetAllSections", null).ToList<Section>();
        }

        public List<Section> GetSectionById(int sectionId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SectionId", sectionId);
            return ReturnList<Section>("GetSectionById", null).ToList<Section>();
        }

        public void AddSection(string sectionName)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SectionName", sectionName);
            ExecuteWithoutReturn("AddSection", param);
        }


        public List<Subsection> GetAllSubsections()
        {
            return ReturnList<Subsection>("GetAllSubections", null).ToList<Subsection>();
        }

        public List<Subsection> GetSubsectionById(int subsectionId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionId", subsectionId);
            return ReturnList<Subsection>("GetSubsectionById", param).ToList<Subsection>();
        }

        public List<Subsection> GetAllSubsectionsBySectionId(int sectionId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SectionId", sectionId);
            return ReturnList<Subsection>("GetAllSubsectionsBySectionId", param).ToList<Subsection>();
        }

        public void AddSubsection(int sectionId, string subsectionName)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionName", subsectionName);
            param.Add("@SectionId", sectionId);
            ExecuteWithoutReturn("AddSubsection", param);
        }


        public Article GetArticleById(int articleId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ArticleId", articleId);
            return ReturnList<Article>("GetArticleById", param).FirstOrDefault<Article>();
        }

        public List<Article> GetAllArticlesBySubsectionId(int subsectionId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionId", subsectionId);
            return ReturnList<Article>("GetAllArticlesBySubsectionId", param).ToList<Article>();
        }

        public void SetIsApprovedArticle(int articleId, bool isApproved)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionId", articleId);
            param.Add("@isApproved", isApproved);
            ExecuteWithoutReturn("SetIsApprovedArticle", param);
        }

        public void AddArticle(Article article)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionId", article.IdSubsection);
            param.Add("@ArticleName", article.ArticleName);
            param.Add("@ArticleText", article.ArticleText);
            param.Add("@Authors", article.Authors);
            ExecuteWithoutReturn("AddArticle", param);
        }
    }
}