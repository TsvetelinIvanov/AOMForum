using AOMForum.Web.Models.UserRelationships;

namespace AOMForum.Services.Data.Interfaces
{
    public interface IUsersService
    {
        Task<UserPostsIndexViewModel?> GetUserPostsIndexViewModelAsync(string? id, string? currentUserId);

        Task<UserCommentsIndexViewModel?> GetUserCommentsIndexViewModelAsync(string? id, string? currentUserId);

        Task<UserFollowersIndexViewModel?> GetUserFollowersIndexViewModelAsync(string? id, string? currentUserId);

        Task<UserFollowingsIndexViewModel?> GetUserFollowingsIndexViewModelAsync(string? id, string? currentUserId);

        Task<UserDetailsViewModel?> GetUserDetailsViewModelAsync(string? id, string? currentUserId);

        Task<bool> FollowAsync(string userId, string followerId);

        Task<bool> UnfollowAsync(string userId, string followerId);

        Task<bool> DeleteAsync(string id);

        Task<bool> IsUsernameUsedAsync(string username);

        Task<bool> IsDeletedAsync(string username);

        Task<int> GetTotalCountAsync();

        Task<bool> IsFollowedAlreadyAsync(string id, string followerId);

        Task<int> GetFollowersCountAsync(string id);

        Task<int> GetFollowingsCountAsync(string id);

        Task<IEnumerable<AdminViewModel>> GetAdminsAsync();
    }
}