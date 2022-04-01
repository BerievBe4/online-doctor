using Microsoft.AspNetCore.Mvc;
using online_doctor.Models;
using online_doctor.Repositories;
using System;
using System.Collections.Generic;

namespace online_doctor.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentRepository _commentRepository;

        public CommentController(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public IActionResult GetComment(int doctorId)
        {
            List<Comment> comments = _commentRepository.GetCommentsByDoctorId(doctorId);
            ViewBag.Comments = comments;
            return PartialView("PostComment", comments);
        }

        [HttpPost]
        public IActionResult PostComment(string comment, int userId, int doctorId)
        {
            Comment commentToPosted = new Comment();
            commentToPosted.IdUser = userId;
            commentToPosted.IdDoctor = doctorId;
            commentToPosted.PostedDate = DateTime.Now;
            commentToPosted.CommentText = comment;

            _commentRepository.AddComment(commentToPosted);
            ViewBag.Comments = _commentRepository.GetCommentsByDoctorId(doctorId);
            return PartialView("PostComment", ViewBag.Comments);
        }
    }
}
