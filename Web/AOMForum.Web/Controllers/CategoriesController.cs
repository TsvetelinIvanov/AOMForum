using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Categories;
using Microsoft.AspNetCore.Authorization;

namespace AOMForum.Web.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string? search = null)
        {
            CategoriesAllViewModel viewModel = await this.categoriesService.GetAllViewModelAsync(search);

            return this.View(viewModel);
        }

        // GET: Categories/Details/1
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            CategoryDetailsViewModel? viewModel = await this.categoriesService.GetDetailsViewModelAsync(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}