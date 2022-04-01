using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using online_doctor.Models;
using online_doctor.Repositories;
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

        public IActionResult Index()
        {
            ViewBag.RoleID = HttpContext.Session.GetInt32("RoleID");
            ViewBag.UserID = HttpContext.Session.GetInt32("UserID");
            ViewBag.Login = HttpContext.Session.GetString("Login");

            List<Section> sections = _articleRepository.GetAllSections();
            return View(sections);
        }

        public IActionResult SetApproved(int articleId)
        {
            _articleRepository.SetIsApprovedArticle(articleId, true);
            return RedirectToAction("Index");
        }

        public IActionResult Subsection(int sectionId)
        {
            List<Subsection> subsections = _articleRepository.GetAllSubsectionsBySectionId(sectionId);
            return View(subsections);
        }

        public IActionResult Articles(int subsectionId)
        {
            List<Article> articles = _articleRepository.GetAllArticlesBySubsectionId(subsectionId);
            return View(articles);
        }

        public IActionResult ArticleDetail(int articleId)
        {
            Article article = _articleRepository.GetArticleById(articleId);
            return View(article);
        }

        [HttpGet]
        public IActionResult AddSection()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSection(Section section)
        {
            _articleRepository.AddSection(section.SectionName);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddSubsection()
        {
            Subsection subsection = new Subsection();
            subsection.Sections = _articleRepository.GetAllSections();

            return View(subsection);
        }

        [HttpPost]
        public IActionResult AddSubsection(Subsection subsection)
        {
            _articleRepository.AddSubsection(subsection.IdSection, subsection.SubsectionName);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddArticle()
        {
            Article article = new Article();
            article.Sections = _articleRepository.GetAllSections();
            article.Subsections = _articleRepository.GetAllSubsections();

            return View(article);
        }

        [HttpPost]
        public IActionResult AddArticle(Article article)
        {
            _articleRepository.AddArticle(article);
            return RedirectToAction("Index");
        }
    }
}
