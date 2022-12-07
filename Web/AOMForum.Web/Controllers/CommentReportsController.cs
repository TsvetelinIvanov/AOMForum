using System;
using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.CommentReports;
using AOMForum.Web.Infrastructure;

namespace AOMForum.Web.Controllers
{
    public class CommentReportsController : BaseController
    {
        private readonly ICommentReportsService commentReportsService;

        public CommentReportsController(ICommentReportsService commentReportsService)
        {
            this.commentReportsService = commentReportsService;
        }

        // GET: CommentReports/Create/1
        public async Task<IActionResult> Create(int commentId)
        {
            CommentReportInputModel? inputModel = await this.commentReportsService.GetCommentReportInputModelAsync(commentId);
            if (inputModel == null)
            {
                return this.NotFound();
            }

            return this.View(inputModel);
        }

        // POST: CommentReports/Create/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentReportInputModel? inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            bool isCreated = await this.commentReportsService.CreateAsync(inputModel.Content, inputModel.CommentId, this.User.Id());
            if (!isCreated)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Posts", new { id = inputModel.CommentId });
        }
    }
}