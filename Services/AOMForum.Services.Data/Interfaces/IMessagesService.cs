using AOMForum.Web.Models.Messages;

namespace AOMForum.Services.Data.Interfaces
{
    public interface IMessagesService
    {
        Task<MessageInputModel> GetMessageInputModelAsync(string? userId);

        Task<bool> CreateAsync(string? content, string? senderId, string? receiverId);

        Task<PartnerAllMessagesModel?> GetPartnerAllMessagesModelAsync(string? userId, string? partnerId);

        Task<IEnumerable<LastMessageModel?>> GetLastMessageModelsAsync(string? userId);

        Task<MessagePartnerModel?> GetMessagePartnerModelAsync(string? userId);
    }
}