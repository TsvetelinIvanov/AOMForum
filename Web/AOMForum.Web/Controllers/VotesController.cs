using Microsoft.AspNetCore.Mvc;
using AOMForum.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AOMForum.Web.Infrastructure;
using AOMForum.Web.Models.PostVotes;

namespace AOMForum.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IPostVotesService postVotesService;

        public VotesController(IPostVotesService postVotesService)
        {
            this.postVotesService = postVotesService;
        }

        // POST /api/postvotes
        // Request body: {"postId":1,"isUpVote":true}
        // Response body: {"votesCount":1}
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<VoteResponseModel>> Post(PostVoteInputModel inputModel)
        {            
            await this.postVotesService.VoteAsync(inputModel.PostId, this.User.Id(), inputModel.IsUpVote);
            int votesCount = this.postVotesService.GetVotes(inputModel.PostId);

            return new VoteResponseModel { VotesCount = votesCount };
        }
    }
}