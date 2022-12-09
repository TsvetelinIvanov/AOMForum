namespace AOMForum.Web.Models.Messages
{
    public class PartnerAllMessagesModel
    {
        public MessagePartnerModel? MessagePartner { get; set; }

        public IEnumerable<PartnerMessageModel> Messages { get; set; } = new  HashSet<PartnerMessageModel>();
    }
}