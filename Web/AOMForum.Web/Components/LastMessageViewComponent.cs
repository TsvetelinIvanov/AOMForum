using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Infrastructure;
using AOMForum.Web.Models.Messages;
using Microsoft.AspNetCore.Mvc;

namespace AOMForum.Web.Components
{
    [ViewComponent(Name = "LastMessage")]
    public class LastMessageViewComponent : ViewComponent
    {
        private readonly IMessagesService messagesService;

        public LastMessageViewComponent(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<LastMessageModel?> lastMessageModels = await this.messagesService.GetLastMessageModelsAsync(this.UserClaimsPrincipal.Id());

            return this.View(lastMessageModels);
        }
    }
}