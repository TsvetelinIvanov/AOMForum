using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Comments;
using AOMForum.Web.Infrastructure;

namespace AOMForum.Web.Controllers
{
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        // GET: Comments/Details/1
        public async Task<IActionResult> Details(int id)
        {
            CommentDetailsViewModel? viewModel = await this.commentsService.GetCommentDetailsViewModelAsync(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // GET: Comments/Create
        //public IActionResult Create()
        //{
        //    return View(new CommentInputModel());
        //}

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Details", "Posts", new { id = inputModel.PostId });
            }

            int commentId = await this.commentsService.CreateAsync(inputModel.Content, /*inputModel.ParentId, */inputModel.PostId, this.User.Id());
            
            return this.RedirectToAction(nameof(Details), new { id = commentId });
        }

        // GET: Comments/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            CommentEditModel? editModel = await this.commentsService.GetCommentEditModelAsync(id);
            if (editModel == null)
            {
                return this.NotFound();
            }

            string? authorId = await this.commentsService.GetAuthorIdAsync(editModel.Id);
            if (authorId != null && authorId != this.User.Id() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            return this.View(editModel);
        }

        // POST: Comments/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CommentEditModel? editModel)
        {
            if (editModel == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                editModel = await this.commentsService.GetCommentEditModelAsync(editModel.Id);
                if (editModel == null)
                {
                    return this.NotFound();
                }

                return this.View(editModel);
            }

            string? authorId = await this.commentsService.GetAuthorIdAsync(editModel.Id);
            if (authorId != null && authorId != this.User.Id() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            bool isEdited = await this.commentsService.EditAsync(editModel.Id, editModel.Content);
            if (!isEdited)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Details), new { id = editModel.Id });
        }

        // GET: Comments/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            CommentDeleteModel? deleteModel = await this.commentsService.GetCommentDeleteModelAsync(id);
            if (deleteModel == null)
            {
                return this.NotFound();
            }

            string? authorId = await this.commentsService.GetAuthorIdAsync(deleteModel.Id);
            if (authorId != null && authorId != this.User.Id() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            return this.View(deleteModel);
        }

        // POST: Comments/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CommentDeleteModel? deleteModel = await this.commentsService.GetCommentDeleteModelAsync(id);
            if (deleteModel == null)
            {
                return this.NotFound();
            }

            string? authorId = await this.commentsService.GetAuthorIdAsync(deleteModel.Id);
            if (authorId != null && authorId != this.User.Id() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            bool isDeleted = await this.commentsService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Posts", new { id = deleteModel.PostId });
        }
    }
}