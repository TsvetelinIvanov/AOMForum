using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.PostReports;
using AOMForum.Web.Infrastructure;

namespace AOMForum.Web.Controllers
{
    public class PostReportsController : BaseController
    {
        private readonly IPostReportsService postReportsService;

        public PostReportsController(IPostReportsService postReportsService)
        {
            this.postReportsService = postReportsService;
        }

        // GET: PostReports/Create/1
        public async Task<IActionResult> Create(int postId)
        {
            PostReportInputModel? inputModel = await this.postReportsService.GetPostReportInputModelAsync(postId);
            if (inputModel == null)
            {
                return this.NotFound();
            }

            return this.View(inputModel);
        }

        // POST: PostReports/Create/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostReportInputModel? inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            bool isCreated = await this.postReportsService.CreateAsync(inputModel.Content, inputModel.PostId, this.User.Id());
            if (!isCreated)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Posts", new { id = inputModel.PostId });
        }
    }
}