using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using AOMForum.Web.Models.CommentVotes;

namespace AOMForum.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentVotesController : ControllerBase
    {
        private readonly ICommentVotesService commentVotesService;

        public CommentVotesController(ICommentVotesService commentVotesService)
        {
            this.commentVotesService = commentVotesService;
        }

        // POST /api/postvotes
        // Request body: {"commentId":1,"isUpVote":true}
        // Response body: {"votesCount":1}
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentVotesCountModel>> Post(CommentVoteInputModel inputModel)
        {
            await this.commentVotesService.VoteAsync(inputModel.CommentId, this.User.Id(), inputModel.IsUpVote);
            int votesCount = this.commentVotesService.GetVotes(inputModel.CommentId);

            return new CommentVotesCountModel { VotesCount = votesCount };
        }
    }
}