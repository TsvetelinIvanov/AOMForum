namespace AOMForum.Web.Models.CommentVotes
{
    public class CommentVoteInputModel
    {
        public int CommentId { get; init; }

        public bool IsUpVote { get; init; }
    }
}