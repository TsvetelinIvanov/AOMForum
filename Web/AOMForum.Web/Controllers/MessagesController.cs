using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Messages;
using AOMForum.Web.Infrastructure;

namespace AOMForum.Web.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IMessagesService messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            messagesService = messagesService;
        }

        // GET: Messages
        public IActionResult Index(){
            
            return this.View();
        }

        // GET: Messages/PartnerMessages/id
        public async Task<IActionResult> PartnerMessages(string? id)
        {
            PartnerAllMessagesModel? viewModel = await this.messagesService.GetPartnerAllMessagesModelAsync(this.User.Id(), id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return View(viewModel);
        }

        // GET: Messages/Create
        public async Task<IActionResult> Create()
        {
            MessageInputModel? inputModel = await this.messagesService.GetMessageInputModelAsync(this.User.Id());

            return View(inputModel);
        }

        // POST: Messages/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageInputModel? inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                if (inputModel == null)
                {
                    inputModel = await this.messagesService.GetMessageInputModelAsync(this.User.Id());
                }
                else
                {
                    MessageInputModel? newInputModel = await this.messagesService.GetMessageInputModelAsync(this.User.Id());
                    inputModel.MessagePartners = newInputModel != null ? newInputModel.MessagePartners : new List<MessagePartnerModel>();
                }

                return this.View(inputModel);
            }

            bool isCreated = await this.messagesService.CreateAsync(inputModel.Content, this.User.Id(), inputModel.ReceiverId);
            if (!isCreated)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(PartnerMessages), new { id = inputModel.ReceiverId });
        }
    }
}