using Microsoft.AspNetCore.Mvc;
using online_doctor.Filters;
using online_doctor.Models;
using online_doctor.Repositories;
using online_sdoctor.Filters;
using System.Collections.Generic;

namespace online_doctor.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleRepository _articleRepository;

        public ArticleController(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [DataFilter]
        public IActionResult Index()
        {
            List<Section> sections = _articleRepository.GetAllSections();
            return View(sections);
        }

        [IsAuthorizedAdmin]
        public IActionResult SetApproved(int articleId)
        {
            _articleRepository.SetIsApprovedArticle(articleId, true);
            return RedirectToAction("Index");
        }

        [IsExistUser]
        public IActionResult Subsection(int sectionId)
        {
            List<Subsection> subsections = _articleRepository.GetAllSubsectionsBySectionId(sectionId);
            return View(subsections);
        }

        [IsAuthorizedAdmin]
        public IActionResult Articles(int subsectionId)
        {
            List<Article> articles = _articleRepository.GetAllArticlesBySubsectionId(subsectionId);
            return View(articles);
        }

        [IsExistUser]
        public IActionResult ArticleDetail(int articleId)
        {
            Article article = _articleRepository.GetArticleById(articleId);
            return View(article);
        }

        [IsAuthorizedAdmin]
        [HttpGet]
        public IActionResult AddSection()
        {
            return View();
        }

        [IsAuthorizedAdmin]
        [HttpPost]
        public IActionResult AddSection(Section section)
        {
            _articleRepository.AddSection(section.SectionName);
            return RedirectToAction("Index");
        }

        [IsAuthorizedAdmin]
        [HttpGet]
        public IActionResult AddSubsection()
        {
            Subsection subsection = new Subsection();
            subsection.Sections = _articleRepository.GetAllSections();

            return View(subsection);
        }

        [IsAuthorizedAdmin]
        [HttpPost]
        public IActionResult AddSubsection(Subsection subsection)
        {
            _articleRepository.AddSubsection(subsection.IdSection, subsection.SubsectionName);
            return RedirectToAction("Index");
        }

        [IsAuthorizedAdmin]
        [HttpGet]
        public IActionResult AddArticle()
        {
            Article article = new Article();
            article.Sections = _articleRepository.GetAllSections();
            article.Subsections = _articleRepository.GetAllSubsections();

            return View(article);
        }

        [IsAuthorizedAdmin]
        [HttpPost]
        public IActionResult AddArticle(Article article)
        {
            _articleRepository.AddArticle(article);
            return RedirectToAction("Index");
        }
    }
}
