namespace AOMForum.Web.Models.Messages
{
    public class UserWithMessagesModel
    {
        public MessageWithUserModel? User { get; init; }

        public IEnumerable<UserWithMessagesModel> Messages { get; set; } = new  HashSet<UserWithMessagesModel>();
    }
}