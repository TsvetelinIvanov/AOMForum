using AOMForum.Common;
using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Home;
using AOMForum.Web.Models.UserRelationships;
using Microsoft.EntityFrameworkCore;

namespace AOMForum.Services.Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<Relationship> relationshipsRepository;
        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository, IDeletableEntityRepository<Post> postsRepository, IDeletableEntityRepository<Comment> commentsRepository, IDeletableEntityRepository<Relationship> relationshipsRepository, IDeletableEntityRepository<ApplicationRole> rolesRepository)
        {
            this.usersRepository = usersRepository;
            this.postsRepository = postsRepository;
            this.commentsRepository = commentsRepository;
            this.relationshipsRepository = relationshipsRepository;
            this.rolesRepository = rolesRepository;
        }

        public async Task<UserPostsIndexViewModel?> GetUserPostsIndexViewModelAsync(string? id, string? currentUserId)
        {
            ApplicationUser? user = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .Include(u => u.Relationships)
                .AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            IEnumerable<int> postIds = user.Posts.Select(p => p.Id);
            IEnumerable<UserPostViewModel> postModels = await this.postsRepository.All()
                .Include(p => p.Votes)
                .Include(p => p.Comments)
                .Include(p => p.Category)
                .Include(p => p.Tags).ThenInclude(pt => pt.Tag)
                .AsNoTracking().Where(p => postIds.Contains(p.Id)).Select(p => new UserPostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    VotesCount = p.Votes.Sum(pv => (int)pv.Type),
                    CommentsCount = p.Comments.Count(),
                    Category = new CategoryInUserPostViewModel()
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    },
                    Tags = p.Tags.Select(pt => pt.Tag).Select(t => new TagInUserPostViewModel()
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                }).ToListAsync();

            IEnumerable<string?> followingIds = await this.relationshipsRepository.AllAsNoTracking()
                .Where(r => r.FollowerId == user.Id)
                .Select(r => r.LeaderId)
                .Where(i => i != null)
                .ToListAsync();

            UserPostsIndexViewModel viewModel = new UserPostsIndexViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePictureURL = user.ProfilePictureURL,
                PostsCount = user.Posts.Count(),
                CommentsCount = user.Comments.Count(),
                VotesCount = user.PostVotes.Sum(pv => (int)pv.Type) + user.CommentVotes.Sum(cv => (int)cv.Type),
                IsFollowed = user.Relationships.Any(r => r.LeaderId == user.Id && r.FollowerId == currentUserId),
                FollowersCount = user.Relationships.Count(r => r.LeaderId == user.Id),
                FollowingsCount = followingIds.Count(),
                Posts = postModels                
            };

            return viewModel;
        }

        public async Task<UserCommentsIndexViewModel?> GetUserCommentsIndexViewModelAsync(string? id, string? currentUserId)
        {
            ApplicationUser? user = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .Include(u => u.Relationships)
                .AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            IEnumerable<int> commentIds = user.Comments.Select(c => c.Id);
            IEnumerable<UserCommentViewModel> commentModels = await this.commentsRepository.All()
                .Include(c => c.Votes)
                .Include(c => c.Post).ThenInclude(p => p.Category)
                .AsNoTracking().Where(c => commentIds.Contains(c.Id)).Select(c => new UserCommentViewModel()
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    PostTitle = c.Post.Title,
                    Content = c.Content,
                    PostCategoryId = c.Post.CategoryId,
                    PostCategoryName = c.Post.Category.Name,
                    VotesCount = c.Votes.Sum(cv => (int)cv.Type)
                }).ToListAsync();

            IEnumerable<string?> followingIds = await this.relationshipsRepository.AllAsNoTracking()
                .Where(r => r.FollowerId == user.Id)
                .Select(r => r.LeaderId)
                .Where(i => i != null)
                .ToListAsync();

            UserCommentsIndexViewModel viewModel = new UserCommentsIndexViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePictureURL = user.ProfilePictureURL,
                PostsCount = user.Posts.Count(),
                CommentsCount = user.Comments.Count(),
                VotesCount = user.PostVotes.Sum(pv => (int)pv.Type) + user.CommentVotes.Sum(cv => (int)cv.Type),
                IsFollowed = user.Relationships.Any(r => r.LeaderId == user.Id && r.FollowerId == currentUserId),
                FollowersCount = user.Relationships.Count(r => r.LeaderId == user.Id),
                FollowingsCount = followingIds.Count(),
                Comments = commentModels
            };

            return viewModel;
        }

        public async Task<UserFollowersIndexViewModel?> GetUserFollowersIndexViewModelAsync(string? id, string? currentUserId)
        {
            ApplicationUser? user = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .Include(u => u.Relationships)
                .AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            IEnumerable<string?> followerIds = await this.relationshipsRepository.AllAsNoTracking()
                .Where(r => r.LeaderId == user.Id)
                .Select(r => r.FollowerId)
                .Where(i => i != null)
                .ToListAsync();
            IEnumerable<UserFollowerViewModel> followers = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .AsNoTracking().Where(u => followerIds.Contains(u.Id)).Select(u => new UserFollowerViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    ProfilePictureURL = u.ProfilePictureURL,
                    PostsCount = u.Posts.Count(),
                    CommentsCount = u.Comments.Count(),
                    VotesCount = u.PostVotes.Sum(pv => (int)pv.Type) + u.CommentVotes.Sum(cv => (int)cv.Type)
                }).ToListAsync();

            IEnumerable<string?> followingIds = await this.relationshipsRepository.AllAsNoTracking()
                .Where(r => r.FollowerId == user.Id)
                .Select(r => r.LeaderId)
                .Where(i => i != null)
                .ToListAsync();

            UserFollowersIndexViewModel viewModel = new UserFollowersIndexViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePictureURL = user.ProfilePictureURL,
                PostsCount = user.Posts.Count(),
                CommentsCount = user.Comments.Count(),
                VotesCount = user.PostVotes.Sum(pv => (int)pv.Type) + user.CommentVotes.Sum(cv => (int)cv.Type),
                IsFollowed = user.Relationships.Any(r => r.LeaderId == user.Id && r.FollowerId == currentUserId),
                FollowersCount = user.Relationships.Count(r => r.LeaderId == user.Id),
                FollowingsCount = followingIds.Count(),
                Followers = followers
            };

            return viewModel;
        }

        public async Task<UserFollowingsIndexViewModel?> GetUserFollowingsIndexViewModelAsync(string? id, string? currentUserId)
        {
            ApplicationUser? user = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .Include(u => u.Relationships)
                .AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            IEnumerable<string?> followingIds = await this.relationshipsRepository.AllAsNoTracking()
                .Where(r => r.FollowerId == user.Id)
                .Select(r => r.LeaderId)
                .Where(i => i != null)
                .ToListAsync();
            IEnumerable<UserFollowingViewModel> followings = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .AsNoTracking().Where(u => followingIds.Contains(u.Id)).Select(u => new UserFollowingViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    ProfilePictureURL = u.ProfilePictureURL,
                    PostsCount = u.Posts.Count(),
                    CommentsCount = u.Comments.Count(),
                    VotesCount = u.PostVotes.Sum(pv => (int)pv.Type) + u.CommentVotes.Sum(cv => (int)cv.Type)
                }).ToListAsync();

            UserFollowingsIndexViewModel viewModel = new UserFollowingsIndexViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePictureURL = user.ProfilePictureURL,
                PostsCount = user.Posts.Count(),
                CommentsCount = user.Comments.Count(),
                VotesCount = user.PostVotes.Sum(pv => (int)pv.Type) + user.CommentVotes.Sum(cv => (int)cv.Type),
                IsFollowed = user.Relationships.Any(r => r.LeaderId == user.Id && r.FollowerId == currentUserId),
                FollowersCount = user.Relationships.Count(r => r.LeaderId == user.Id),
                FollowingsCount = followingIds.Count(),
                Followings = followings
            };

            return viewModel;
        }

        public async Task<UserDetailsViewModel?> GetUserDetailsViewModelAsync(string? id, string? currentUserId)
        {
            ApplicationUser? user = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .Include(u => u.Relationships)
                .AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            IEnumerable<int> postIds = user.Posts.Select(p => p.Id);
            IEnumerable<UserPostViewModel> postModels = await this.postsRepository.All()
                .Include(p => p.Votes)
                .Include(p => p.Comments)
                .Include(p => p.Category)
                .Include(p => p.Tags).ThenInclude(pt => pt.Tag)
                .AsNoTracking().Where(p => postIds.Contains(p.Id)).Select(p => new UserPostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    VotesCount = p.Votes.Sum(pv => (int)pv.Type),
                    CommentsCount = p.Comments.Count(),
                    Category = new CategoryInUserPostViewModel()
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    },
                    Tags = p.Tags.Select(pt => pt.Tag).Select(t => new TagInUserPostViewModel()
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                }).ToListAsync();

            IEnumerable<int> commentIds = user.Comments.Select(c => c.Id);
            IEnumerable<UserCommentViewModel> commentModels = await this.commentsRepository.All()
                .Include(c => c.Votes)
                .Include(c => c.Post).ThenInclude(p => p.Category)
                .AsNoTracking().Where(c => commentIds.Contains(c.Id)).Select(c => new UserCommentViewModel()
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    PostTitle = c.Post.Title,
                    Content = c.Content,
                    PostCategoryId = c.Post.CategoryId,
                    PostCategoryName = c.Post.Category.Name,
                    VotesCount = c.Votes.Sum(cv => (int)cv.Type)
                }).ToListAsync();

            IEnumerable<string?> followerIds = await this.relationshipsRepository.AllAsNoTracking()
                .Where(r => r.LeaderId == user.Id)
                .Select(r => r.FollowerId)
                .Where(i => i != null)
                .ToListAsync();
            IEnumerable<UserFollowerViewModel> followers = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .AsNoTracking().Where(u => followerIds.Contains(u.Id)).Select(u => new UserFollowerViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    ProfilePictureURL = u.ProfilePictureURL,
                    PostsCount = u.Posts.Count(),
                    CommentsCount = u.Comments.Count(),
                    VotesCount = u.PostVotes.Sum(pv => (int)pv.Type) + u.CommentVotes.Sum(cv => (int)cv.Type)
                }).ToListAsync();

            IEnumerable<string?> followingIds = await this.relationshipsRepository.AllAsNoTracking()
                .Where(r => r.FollowerId == user.Id)
                .Select(r => r.LeaderId)
                .Where(i => i != null)
                .ToListAsync();
            IEnumerable<UserFollowingViewModel> followings = await this.usersRepository.All()
                .Include(u => u.Posts)
                .Include(u => u.PostVotes)
                .Include(u => u.Comments)
                .Include(u => u.CommentVotes)
                .AsNoTracking().Where(u => followingIds.Contains(u.Id)).Select(u => new UserFollowingViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    ProfilePictureURL = u.ProfilePictureURL,
                    PostsCount = u.Posts.Count(),
                    CommentsCount = u.Comments.Count(),
                    VotesCount = u.PostVotes.Sum(pv => (int)pv.Type) + u.CommentVotes.Sum(cv => (int)cv.Type)
                }).ToListAsync();

            UserDetailsViewModel viewModel = new UserDetailsViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePictureURL = user.ProfilePictureURL,
                PostsCount = user.Posts.Count(),
                CommentsCount = user.Comments.Count(),
                VotesCount = user.PostVotes.Sum(pv => (int)pv.Type) + user.CommentVotes.Sum(cv => (int)cv.Type),
                IsFollowed = user.Relationships.Any(r => r.LeaderId == user.Id && r.FollowerId == currentUserId),
                FollowersCount = user.Relationships.Count(r => r.LeaderId == user.Id),
                //FollowingsCount = user.Relationships.Count(r => r.FollowerId == user.Id),
                FollowingsCount = followingIds.Count(),
                Posts = postModels,
                Comments = commentModels,
                Followers = followers,
                Followings = followings
            };

            return viewModel;
        }

        public async Task<bool> FollowAsync(string leaderId, string followerId)
        {
            bool isLeaderExisting = this.usersRepository.AllAsNoTracking().Any(u => u.Id == leaderId);
            if (!isLeaderExisting)
            {
                return false;
            }

            bool isRelationshipExisting = this.relationshipsRepository.AllAsNoTracking().Any(r => r.LeaderId == leaderId && r.FollowerId == followerId);
            if (isRelationshipExisting)
            {
                return false;
            }

            Relationship relationship = new Relationship()
            {
                LeaderId = leaderId,
                FollowerId = followerId
            };

            await this.relationshipsRepository.AddAsync(relationship);
            await this.relationshipsRepository.SaveChangesAsync();            

            return true;
        }

        public async Task<bool> UnfollowAsync(string leaderId, string followerId)
        {
            bool isLeaderExisting = await this.usersRepository.AllAsNoTracking().AnyAsync(u => u.Id == leaderId);
            if (!isLeaderExisting)
            {
                return false;
            }

            Relationship? relationship = await this.relationshipsRepository.All().Where(r => r.LeaderId == leaderId && r.FollowerId == followerId).FirstOrDefaultAsync();
            if (relationship == null)
            {                
                return false;
            }

            this.relationshipsRepository.Delete(relationship);
            await this.relationshipsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUserListViewModelsAsync()
        {
            IEnumerable<UserListViewModel> userModels = await this.usersRepository.AllAsNoTracking().Select(u => new UserListViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                LastName = u.LastName,
                Gender = u.Gender == GenderType.Male ? "Мъж" : u.Gender == GenderType.Female ? "Жена" : u.Gender == GenderType.Other ? "Друг" : "Неизвестен",
                BirthDate = u.BirthDate.ToString(GlobalConstants.UsedDateFormat),
                Biography = u.Biography,
                ProfilePictureURL = u.ProfilePictureURL
            }).ToListAsync();

            return userModels;
        }

        public async Task<IEnumerable<AdminListViewModel>> GetAdminListViewModelsAsync()
        {
            string? adminRoleId = await this.rolesRepository.AllAsNoTracking().Where(r => r.Name == GlobalConstants.AdministratorRoleName).Select(r => r.Id).FirstOrDefaultAsync();

            List<AdminListViewModel> admins = await this.usersRepository.AllAsNoTracking()
                .Where(u => u.Roles.Select(r => r.RoleId).FirstOrDefault() == adminRoleId).Select(u => new AdminListViewModel()
                {
                    Id = u.Id,
                    UserName= u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    SecondName = u.SecondName,
                    LastName = u.LastName,
                    Gender = u.Gender == GenderType.Male ? "Мъж" : u.Gender == GenderType.Female ? "Жена" : u.Gender == GenderType.Other ? "Друг" : "Неизвестен",
                    BirthDate = u.BirthDate.ToString(GlobalConstants.UsedDateFormat),
                    Biography = u.Biography,
                    ProfilePictureURL = u.ProfilePictureURL
                }).ToListAsync();

            return admins;
        }

        public async Task<UserDeleteModel?> GetUserDeleteModelAsync(string id)
        {
            ApplicationUser? user = await this.usersRepository.AllAsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            UserDeleteModel deleteModel = new UserDeleteModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                BirthDate = user.BirthDate.ToString(GlobalConstants.UsedDateFormat),
                ProfilePictureURL = user.ProfilePictureURL
            };

            return deleteModel;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            ApplicationUser? user = await this.usersRepository.All().Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }

            this.usersRepository.Delete(user);
            await this.usersRepository.SaveChangesAsync();

            return user.IsDeleted;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUserListViewModelsForDeletedAsync()
        {
            IEnumerable<UserListViewModel> userModels = await this.usersRepository.AllAsNoTrackingWithDeleted().Where(u => u.IsDeleted).Select(u => new UserListViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                SecondName = u.SecondName,
                LastName = u.LastName,
                Gender = u.Gender.ToString(),
                BirthDate = u.BirthDate.ToString(GlobalConstants.UsedDateFormat),
                Biography = u.Biography,
                ProfilePictureURL = u.ProfilePictureURL
            }).ToListAsync();

            return userModels;
        }

        public async Task<UserDeleteModel?> GetUserDeleteModelForDeletedAsync(string id)
        {
            ApplicationUser? user = await this.usersRepository.AllAsNoTrackingWithDeleted().Where(u => u.Id == id && u.IsDeleted).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            UserDeleteModel deleteModel = new UserDeleteModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                BirthDate = user.BirthDate.ToString(GlobalConstants.UsedDateFormat),
                ProfilePictureURL = user.ProfilePictureURL
            };

            return deleteModel;
        }

        public async Task<bool> UndeleteAsync(string id)
        {
            ApplicationUser? user = await this.usersRepository.AllWithDeleted().Where(u => u.Id == id && u.IsDeleted).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;
            }

            this.usersRepository.Undelete(user);
            await this.usersRepository.SaveChangesAsync();

            return user.IsDeleted == false;
        }

        public async Task<HomeViewModel> GetHomeViewModelAsync()
        {
            int postsCount = await this.postsRepository.AllAsNoTracking().CountAsync();
            int usersCount = await this.usersRepository.AllAsNoTracking().CountAsync();

            string? adminRoleId = await this.rolesRepository.AllAsNoTracking().Where(r => r.Name == GlobalConstants.AdministratorRoleName).Select(r => r.Id).FirstOrDefaultAsync();
            int adminsCount = await this.usersRepository.AllAsNoTracking().Where(u => u.Roles.Select(r => r.RoleId).FirstOrDefault() == adminRoleId).CountAsync();

            HomeViewModel homeViewModel = new HomeViewModel()
            {
                PostsCount = postsCount,
                UsersCount = usersCount,
                AdminsCount = adminsCount
            };

            return homeViewModel;
        }

        public async Task<bool> IsUsernameUsedAsync(string username) => await this.usersRepository.AllAsNoTracking().AnyAsync(u => u.UserName == username);

        public async Task<bool> IsDeletedAsync(string username) => await this.usersRepository.AllAsNoTrackingWithDeleted().AnyAsync(u => u.UserName == username && u.IsDeleted);

        public async Task<int> GetTotalCountAsync() => await this.usersRepository.AllAsNoTracking().CountAsync();

        public async Task<bool> IsFollowedAlreadyAsync(string leaderId, string followerId) => await this.relationshipsRepository.AllAsNoTracking().AnyAsync(r => r.LeaderId == leaderId && r.FollowerId == followerId);

        public async Task<int> GetFollowersCountAsync(string id) => await this.relationshipsRepository.AllAsNoTracking().Where(r => r.LeaderId == id).CountAsync();

        public async Task<int> GetFollowingsCountAsync(string id) => await this.relationshipsRepository.AllAsNoTracking().Where(r => r.FollowerId == id)
                .CountAsync();
    }
}