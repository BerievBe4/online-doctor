using Dapper;
using online_doctor.Models;
using online_doctor.Providers;
using online_doctor.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace online_doctor.Views.Admin
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

        public List<Section> AddSection(string sectionName)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SectionName", sectionName);
            return ReturnList<Section>("AddSection", param).ToList<Section>();
        }


        public List<Section> GetAllSubsections()
        {
            return ReturnList<Section>("GetAllSubections", null).ToList<Section>();
        }

        public List<Section> GetSubsectionById(int subsectionId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionId", subsectionId);
            return ReturnList<Section>("GetSubsectionById", param).ToList<Section>();
        }

        public List<Section> AddSubsection(int sectionId, string subsectionName)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionName", subsectionName);
            param.Add("@SectionId", sectionId);
            return ReturnList<Section>("AddSubsection", param).ToList<Section>();
        }


        public List<Section> GetAllArticles()
        {
            return ReturnList<Section>("GetAllArticles", null).ToList<Section>();
        }

        public List<Section> GetArticleById(int articleId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionId", articleId);
            return ReturnList<Section>("GetArticleById", param).ToList<Section>();
        }

        public List<Section> AddArticle (int sectionId, string subsectionName)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SubsectionName", subsectionName);
            param.Add("@SectionId", sectionId);
            return ReturnList<Section>("AddSubsection", param).ToList<Section>();
        }
    }
}
