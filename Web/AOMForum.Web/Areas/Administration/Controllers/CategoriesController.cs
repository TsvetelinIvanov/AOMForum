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
using AOMForum.Web.Models.Categories;
using static Humanizer.On;

namespace AOMForum.Web.Areas.Administration.Controllers
{
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        // GET: Administration/Categories
        public async Task<IActionResult> Index(string? search = null)
        {
            CategoriesAllViewModel viewModel = await this.categoriesService.GetAllViewModelAsync(search);

            return this.View(viewModel);
        }

        // GET: Administration/Categories/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Categories/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            int categoyId = await this.categoriesService.CreateAsync(inputModel.Name, inputModel.Description, inputModel.ImageUrl);

            return this.RedirectToAction("Details", "Categories", new { id = categoyId, area = "" });
        }

        // GET: Administration/Categories/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            CategoryEditModel? model = await this.categoriesService.GetEditModelAsync(id);
            if (model == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        // POST: Administration/Categories/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            bool isEdited = await this.categoriesService.EditAsync(model.Id, model.Name, model.Description, model.ImageUrl);
            if (!isEdited)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("Details", "Categories", new { id = model.Id, area = "" });
        }

        // GET: Administration/Categories/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            CategoryDeleteModel? model = await this.categoriesService.GetDeleteModelAsync(id);
            if (model == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        // POST: Administration/Categories/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await this.categoriesService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}