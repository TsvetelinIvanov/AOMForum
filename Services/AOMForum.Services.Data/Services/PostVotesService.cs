using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Data.Models.Enums;
using AOMForum.Services.Data.Interfaces;

namespace AOMForum.Services.Data.Services
{
    public class PostVotesService : IPostVotesService
    {
        private readonly IDeletableEntityRepository<PostVote> postVotesRepository;

        public PostVotesService(IDeletableEntityRepository<PostVote> postVotesRepository)
        {
            this.postVotesRepository = postVotesRepository;
        }

        public int GetVotes(int postId)
        {
            int votesCount = this.postVotesRepository.All().Where(pv => pv.PostId == postId).Sum(pv => (int)pv.Type);

            return votesCount;
        }

        public async Task VoteAsync(int postId, string? authorId, bool isUpVote)
        {
            PostVote? vote = this.postVotesRepository.All().FirstOrDefault(pv => pv.PostId == postId && pv.AuthorId == authorId);
            if (vote != null)
            {
                vote.Type = isUpVote ? VoteType.UpVote : VoteType.DownVote;
            }
            else
            {
                vote = new PostVote
                {
                    PostId = postId,
                    AuthorId = authorId,
                    Type = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                };

                await this.postVotesRepository.AddAsync(vote);
            }

            await this.postVotesRepository.SaveChangesAsync();
        }
    }
}