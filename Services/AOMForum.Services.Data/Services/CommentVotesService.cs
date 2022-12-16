using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models.Enums;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;

namespace AOMForum.Services.Data.Services
{
    public class CommentVotesService : ICommentVotesService
    {
        private readonly IDeletableEntityRepository<CommentVote> commentVotesRepository;

        public CommentVotesService(IDeletableEntityRepository<CommentVote> commentVotesRepository)
        {
            this.commentVotesRepository = commentVotesRepository;
        }

        public async Task VoteAsync(int commentId, string? authorId, bool isUpVote)
        {
            CommentVote? vote = this.commentVotesRepository.All().FirstOrDefault(cv => cv.CommentId == commentId && cv.AuthorId == authorId);
            if (vote != null)
            {
                vote.Type = isUpVote ? VoteType.UpVote : VoteType.DownVote;
            }
            else
            {
                vote = new CommentVote
                {
                    CommentId = commentId,
                    AuthorId = authorId,
                    Type = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                };

                await this.commentVotesRepository.AddAsync(vote);
            }

            await this.commentVotesRepository.SaveChangesAsync();
        }

        public int GetVotes(int commentId)
        {
            int votesCount = this.commentVotesRepository.All().Where(cv => cv.CommentId == commentId).Sum(cv => (int)cv.Type);

            return votesCount;
        }
    }
}