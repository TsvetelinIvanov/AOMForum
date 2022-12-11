using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Tags;
using static AOMForum.Common.DataConstants.Tag;

namespace AOMForum.Web.Areas.Administration.Controllers
{
    public class TagsController : AdministrationController
    {
        private readonly ITagsService tagsService;

        public TagsController(ITagsService tagsService)
        {
            this.tagsService = tagsService;
        }

        // GET: Administration/Tags
        public async Task<IActionResult> Index(int page = 1, string? search = null)
        {
            int skip = (page - 1) * TagsPerPage;
            int tagsCount = await this.tagsService.GetTagsCountAsync(search);
            IEnumerable<TagListViewModel> tagModels = await this.tagsService.GetAllTagListViewModelsAsync(search, skip, TagsPerPage);

            TagsAllViewModel viewModel = this.tagsService.GetTagsAllViewModel(tagsCount, TagsPerPage, tagModels, page, search);

            return this.View(viewModel);
        }        

        // GET: Administration/Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administration/Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            int tagId = await this.tagsService.CreateAsync(inputModel.Name);

            return this.RedirectToAction("Details", "Tags", new { id = tagId, area = "" });
        }

        // GET: Administration/Tags/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            TagDeleteModel? model = await this.tagsService.GetDeleteModelAsync(id);
            if (model == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        // POST: Administration/Tags/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await this.tagsService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}