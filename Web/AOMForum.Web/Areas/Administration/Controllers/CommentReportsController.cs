using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.CommentReports;

namespace AOMForum.Web.Areas.Administration.Controllers
{    
    public class CommentReportsController : AdministrationController
    {
        private readonly ICommentReportsService commentReportsService;

        public CommentReportsController(ICommentReportsService commentReportsService)
        {
            this.commentReportsService = commentReportsService;
        }

        // GET: Administration/CommentReports
        public async Task<IActionResult> Index()
        {
            IEnumerable<CommentReportListViewModel> viewModels = await this.commentReportsService.GetCommentReportListViewModelsAsync();

            return this.View(viewModels);
        }

        // GET: Administration/CommentReports/Details/1
        public async Task<IActionResult> Details(int id)
        {
            CommentReportDetailsViewModel? viewModel = await this.commentReportsService.GetCommentReportDetailsViewModelAsync(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }        

        // GET: Administration/CommentReports/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            CommentReportDeleteModel? deleteModel = await this.commentReportsService.GetCommentReportDeleteModelAsync(id);
            if (deleteModel == null)
            {
                return this.NotFound();
            }

            return this.View(deleteModel);
        }

        // POST: Administration/CommentReports/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await this.commentReportsService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}