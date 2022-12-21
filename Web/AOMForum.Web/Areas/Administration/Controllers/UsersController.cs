using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.UserRelationships;

namespace AOMForum.Web.Areas.Administration.Controllers
{    
    public class UsersController : AdministrationController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Administration/Users
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserListViewModel> viewModels = await this.usersService.GetUserListViewModelsAsync();

            return this.View(viewModels);
        }

        // GET: Administration/Users
        public async Task<IActionResult> DeletedIndex()
        {
            IEnumerable<UserListViewModel> viewModels = await this.usersService.GetUserListViewModelsForDeletedAsync();

            return this.View(viewModels);
        }

        // GET: Administration/Users/Delete/1
        public async Task<IActionResult> Delete(string id)
        {
            UserDeleteModel? deleteModel = await this.usersService.GetUserDeleteModelAsync(id);
            if (deleteModel == null)
            {
                return this.NotFound();
            }

            return this.View(deleteModel);
        }

        // POST: Administration/Users/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool isDeleted = await this.usersService.DeleteAsync(id);
            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Index));
        }

        // GET: Administration/Users/Undelete/1
        public async Task<IActionResult> Undelete(string id)
        {
            UserDeleteModel? deleteModel = await this.usersService.GetUserDeleteModelForDeletedAsync(id);
            if (deleteModel == null)
            {
                return this.NotFound();
            }

            return this.View(deleteModel);
        }

        // POST: Administration/Users/Undelete/1
        [HttpPost, ActionName("Undelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UndeleteConfirmed(string id)
        {
            bool isRestored = await this.usersService.UndeleteAsync(id);
            if (!isRestored)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}