namespace AOMForum.Services.Data.Interfaces
{
    public interface IPostVotesService
    {
        int GetVotes(int postId);

        Task VoteAsync(int postId, string? authorId, bool isUpVote);
    }
}