using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.UserRelationships;
using AOMForum.Web.Infrastructure;

namespace AOMForum.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Users/Index
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserListViewModel> viewModels = await this.usersService.GetUserListViewModelsAsync();
            
            return this.View(viewModels);
        }

        // GET: Users/AdminIndex
        public async Task<IActionResult> AdminIndex()
        {
            IEnumerable<AdminListViewModel> viewModels = await this.usersService.GetAdminListViewModelsAsync();

            return this.View(viewModels);
        }

        // GET: Users/PostsIndex/id
        public async Task<IActionResult> PostsIndex(string? id)
        {
            UserPostsIndexViewModel? viewModel = await this.usersService.GetUserPostsIndexViewModelAsync(id, this.User.Id());
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // GET: Users/CommentsIndex/id
        public async Task<IActionResult> CommentsIndex(string? id)
        {
            UserCommentsIndexViewModel? viewModel = await this.usersService.GetUserCommentsIndexViewModelAsync(id, this.User.Id());
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // GET: Users/FollowersIndex/id
        public async Task<IActionResult> FollowersIndex(string? id)
        {
            UserFollowersIndexViewModel? viewModel = await this.usersService.GetUserFollowersIndexViewModelAsync(id, this.User.Id());
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // GET: Users/FollowingsIndex/id
        public async Task<IActionResult> FollowingsIndex(string? id)
        {
            UserFollowingsIndexViewModel? viewModel = await this.usersService.GetUserFollowingsIndexViewModelAsync(id, this.User.Id());
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // GET: Users/Details/id
        public async Task<IActionResult> Details(string? id)
        {
            UserDetailsViewModel? viewModel = await this.usersService.GetUserDetailsViewModelAsync(id, this.User.Id());
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // POST: Users/Follow/id
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Follow(string? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            string followerId = this.User.Id();
            if (followerId == id)
            {
                return this.BadRequest();
            }

            bool isFollower = await this.usersService.FollowAsync(id, followerId);
            if (!isFollower)
            {
                return this.BadRequest();
            }

            return this.Ok(isFollower);
        }

        // POST: Users/Follow/id
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Unfollow(string? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            string followerId = this.User.Id();
            if (followerId == id)
            {
                return this.BadRequest();
            }

            bool isNotFollower = await this.usersService.UnfollowAsync(id, followerId);
            if (!isNotFollower)
            {
                return this.BadRequest();
            }

            return this.Ok(isNotFollower);
        }
    }
}