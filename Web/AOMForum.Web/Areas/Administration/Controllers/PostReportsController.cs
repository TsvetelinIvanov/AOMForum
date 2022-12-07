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
using AOMForum.Web.Models.PostReports;

namespace AOMForum.Web.Areas.Administration.Controllers
{   
    public class PostReportsController : AdministrationController
    {
        private readonly IPostReportsService postReportsService;

        public PostReportsController(IPostReportsService postReportsService)
        {
            this.postReportsService = postReportsService;
        }

        // GET: Administration/PostReports
        public async Task<IActionResult> Index()
        {
            IEnumerable<PostReportListViewModel> viewModels = await this.postReportsService.GetPostReportListViewModelsAsync();

            return this.View(viewModels);
        }

        // GET: Administration/PostReports/Details/1
        public async Task<IActionResult> Details(int id)
        {
            PostReportDetailsViewModel? viewModel = await this.postReportsService.GetPostReportDetailsViewModelAsync(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        
        // GET: Administration/PostReports/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            PostReportDeleteModel? deleteModel = await this.postReportsService.GetPostReportDeleteModelAsync(id);
            if (deleteModel == null)
            {
                return this.NotFound();
            }

            return this.View(deleteModel);
        }

        // POST: Administration/PostReports/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await this.postReportsService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Index));
        }        
    }
}