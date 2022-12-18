using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AOMForum.Web.Models.Posts;
using AOMForum.Web.Infrastructure;
using static AOMForum.Common.DataConstants.Post;

namespace AOMForum.Web.Controllers
{
    [Authorize]
    public class PostsController : BaseController
    {
        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        // GET: Posts
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, string? search = null)
        {
            int skip = (page - 1) * PostsPerPage;
            int postsCount = await this.postsService.GetPostsCountAsync(search);
            IEnumerable<PostListViewModel> postModels = await this.postsService.GetAllPostListViewModelsAsync(search, skip, PostsPerPage);

            PostsAllViewModel viewModel = this.postsService.GetPostsAllViewModel(postsCount, PostsPerPage, postModels, page, search);
            
            return this.View(viewModel);
        }

        // GET: Posts/Details/1
        public async Task<IActionResult> Details(int id)
        {
            PostDetailsViewModel? viewModel = await this.postsService.GetPostDetailsViewModelAsync(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create()
        {
            PostInputModel inputModel = await this.postsService.GetPostInputModelAsync();

            return View(inputModel);
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                inputModel = await this.postsService.GetPostInputModelAsync();
                return this.View(inputModel);
            }

            int postId = await this.postsService.CreateAsync(inputModel.Title, inputModel.Content, inputModel.ImageUrl, this.User.Id(), inputModel.CategoryId, inputModel.TagIds);

            return this.RedirectToAction(nameof(Details), new { id = postId });
        }

        // GET: Posts/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            PostEditModel? editModel = await this.postsService.GetPostEditModelAsync(id);
            if (editModel == null)
            {
                return this.NotFound();
            }

            string? authorId = await this.postsService.GetAuthorIdAsync(editModel.Id);
            if (authorId != null && authorId != this.User.Id() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            return this.View(editModel);
        }

        // POST: Posts/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditModel? editModel)
        {
            if (editModel == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                editModel = await this.postsService.GetPostEditModelAsync(editModel.Id);
                if (editModel == null)
                {
                    return this.NotFound();
                }

                return this.View(editModel);
            }

            string? authorId = await this.postsService.GetAuthorIdAsync(editModel.Id);
            if (authorId != null && authorId != this.User.Id() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            bool isEdited = await this.postsService.EditAsync(editModel.Id, editModel.Title, editModel.Content, editModel.ImageUrl, editModel.CategoryId, editModel.TagIds);
            if (!isEdited)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Details), new { id = editModel.Id });
        }

        // GET: Posts/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            PostDeleteModel? deleteModel = await this.postsService.GetPostDeleteModelAsync(id);
            if (deleteModel == null)
            {
                return this.NotFound();
            }

            string? authorId = await this.postsService.GetAuthorIdAsync(deleteModel.Id);
            if (authorId != null && authorId != this.User.Id() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            return this.View(deleteModel);
        }

        // POST: Posts/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            PostDeleteModel? deleteModel = await this.postsService.GetPostDeleteModelAsync(id);
            if (deleteModel == null)
            {
                return this.NotFound();
            }

            string? authorId = await this.postsService.GetAuthorIdAsync(deleteModel.Id);
            if (authorId != null && authorId != this.User.Id() && !this.User.IsAdministrator())
            {
                return this.Unauthorized();
            }

            bool isDeleted = await this.postsService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}