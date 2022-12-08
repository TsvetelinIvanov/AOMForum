namespace AOMForum.Services.Data.Interfaces
{
    public interface ICommentVotesService
    {
        int GetVotes(int commentId);

        Task VoteAsync(int commentId, string? authorId, bool isUpVote);
    }
}