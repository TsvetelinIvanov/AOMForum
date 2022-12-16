using AOMForum.Common;
using AOMForum.Data.Common.Repositories;
using AOMForum.Data.Models;
using AOMForum.Services.Data.Interfaces;
using AOMForum.Web.Models.Messages;
using Microsoft.EntityFrameworkCore;
using static AOMForum.Common.GlobalConstants;

namespace AOMForum.Services.Data.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public MessagesService(IDeletableEntityRepository<Message> messagesRepository, IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.messagesRepository = messagesRepository;
            this.usersRepository = usersRepository;
        }

        public async Task<MessageInputModel> GetMessageInputModelAsync(string? userId)
        {
            List<MessagePartnerModel> possiblePartners = await this.usersRepository.AllAsNoTracking().Where(u => u.Id != userId).Select(u => new MessagePartnerModel
            {
                Id = u.Id,
                UserName = u.UserName,
                ProfilePictureURL = u.ProfilePictureURL
            }).ToListAsync();

            MessageInputModel inputModel = new MessageInputModel() { MessagePartners = possiblePartners };

            return inputModel;
        }

        public async Task<bool> CreateAsync(string? content, string? senderId, string? receiverId)
        {
            Message message = new Message
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = receiverId                
            };

            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();

            return message.CreatedOn != null;
        }

        public async Task<PartnerAllMessagesModel?> GetPartnerAllMessagesModelAsync(string? userId, string? partnerId)
        {
            ApplicationUser? partnerUser = await this.usersRepository.AllAsNoTracking().Where(u => u.Id == partnerId).FirstOrDefaultAsync();
            if (partnerUser == null)
            {
                return null;
            }

            MessagePartnerModel partnerModel = new MessagePartnerModel()
            {
                Id = partnerUser.Id,
                UserName = partnerUser.UserName,
                ProfilePictureURL = partnerUser.ProfilePictureURL
            };

            List<PartnerMessageModel> messageModels = await this.messagesRepository.All().Include(m => m.Sender)                
                .Where(m => (m.SenderId == partnerId && m.ReceiverId == userId) || (m.SenderId == userId && m.ReceiverId == partnerId))
                .OrderBy(m => m.CreatedOn)
                .Select(m => new PartnerMessageModel
                {
                    Content = m.Content,
                    CreatedOn = m.CreatedOn.ToString(UsedDateTimeFormat),
                    SenderId = m.SenderId,
                    SenderUserName = m.Sender.UserName,
                    SenderProfilePictureURL = m.Sender.ProfilePictureURL
                }).ToListAsync();

            PartnerAllMessagesModel viewModel = new PartnerAllMessagesModel()
            {
                MessagePartner = partnerModel,
                Messages = messageModels
            };

            return viewModel;
        }

        public async Task<IEnumerable<LastMessageModel?>> GetLastMessageModelsAsync(string? userId)
        {
            List<LastMessageModel?> lastMessageModels = new List<LastMessageModel?>();
            List<MessagePartnerModel> partners = await this.GetMessagePartnersAsync(userId);
            foreach (MessagePartnerModel partner in partners)
            {
                LastMessageModel? lastMessageModel = new LastMessageModel()
                {
                    Id = partner.Id,
                    UserName = partner.UserName,
                    ProfilePictureURL = partner.ProfilePictureURL,
                    LastMessage = await this.GetLastMessageAsync(userId, partner.Id),
                    LastMessageActivity = await this.GetLastActivityAsync(userId, partner.Id)
                };

                lastMessageModels.Add(lastMessageModel);
            }

            return lastMessageModels;
        }

        public async Task<MessagePartnerModel?> GetMessagePartnerModelAsync(string? userId)
        {
            ApplicationUser? partnerUser = await this.usersRepository.AllAsNoTracking().Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (partnerUser == null)
            {
                return null;
            }

            MessagePartnerModel partnerModel = new MessagePartnerModel()
            {
                Id = partnerUser.Id,
                UserName = partnerUser.UserName,
                ProfilePictureURL = partnerUser.ProfilePictureURL
            };

            return partnerModel;
        }

        private async Task<List<MessagePartnerModel>> GetMessagePartnersAsync(string? userId)
        {
            IQueryable<ApplicationUser?> senders = this.messagesRepository.All().Include(m => m.Sender)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.Sender);

            IQueryable<ApplicationUser?> receivers = this.messagesRepository.All().Include(m => m.Receiver)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.Receiver);

            List<MessagePartnerModel> partners = await senders.Concat(receivers).Where(u => u.Id != userId).Distinct().Select(u => new MessagePartnerModel()
            {
                Id = u.Id,
                UserName = u.UserName,
                ProfilePictureURL = u.ProfilePictureURL
            }).ToListAsync();

            return partners;
        }

        private async Task<string?> GetLastMessageAsync(string? userId, string? partnerId) => await this.messagesRepository.AllAsNoTracking()
                .Where(m => (m.SenderId == partnerId && m.ReceiverId == userId) || (m.SenderId == userId && m.ReceiverId == partnerId))
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.Content)
                .FirstOrDefaultAsync();

        private async Task<string?> GetLastActivityAsync(string? userId, string? partnerId) => await this.messagesRepository.AllAsNoTracking()
                .Where(m => (m.SenderId == partnerId && m.ReceiverId == userId) || (m.SenderId == userId && m.ReceiverId == partnerId))
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.CreatedOn.ToString(UsedDateTimeFormat))
                .FirstOrDefaultAsync();
    }
}