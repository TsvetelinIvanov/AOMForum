using AOMForum.Common;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Infrastructure;
using AOMForum.Web.Models.Messages;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace AOMForum.Web.Hubs
{
    public class MessagesHub : Hub
    {
        private readonly IMessagesService messagesService;

        public MessagesHub(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public async Task WhoIsTyping(string name) => await this.Clients.Others.SendAsync("SayWhoIsTyping", name);

        public async Task SendMessage(string content, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            string senderId = this.Context.User.Id();
            DateTime currentTime = DateTime.UtcNow;
            MessagePartnerModel? partnerModel = await this.messagesService.GetMessagePartnerModelAsync(senderId);
            if (partnerModel == null)
            {
                return;
            }

            bool isCreated = await this.messagesService.CreateAsync(content, senderId, receiverId);
            if (!isCreated)
            {
                return;
            }

            await this.Clients.All.SendAsync(
                "ReceiveMessage",
                new PartnerMessageModel
                {
                    Content = content,
                    CreatedOn = currentTime.ToString(GlobalConstants.UsedDateTimeFormat),
                    SenderId = senderId,
                    SenderUserName = partnerModel.UserName,
                    SenderProfilePictureURL = partnerModel.ProfilePictureURL                    
                });
        }
    }
}