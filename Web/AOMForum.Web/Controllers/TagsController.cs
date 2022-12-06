using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AOMForum.Data;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Tags;
using static AOMForum.Common.DataConstants.Tag;
using Microsoft.AspNetCore.Authorization;

namespace AOMForum.Web.Controllers
{
    public class TagsController : BaseController
    {
        private readonly ITagsService tagsService;

        public TagsController(ITagsService tagsService)
        {
            this.tagsService = tagsService;
        }

        // GET: Tags
        public async Task<IActionResult> Index(int page = 1, string? search = null)
        {
            int skip = (page - 1) * TagsPerPage;
            int tagsCount = await this.tagsService.GetTagsCountAsync(search);
            IEnumerable<TagListViewModel> tagModels = await this.tagsService.GetAllTagListViewModelsAsync(search, skip, TagsPerPage);

            TagsAllViewModel viewModel = this.tagsService.GetTagsAllViewModel(tagsCount, TagsPerPage, tagModels, page, search);

            return this.View(viewModel);
        }

        // GET: Tags/Details/1
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            TagDetailsViewModel? viewModel = await this.tagsService.GetTagDetailsViewModelAsync(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}