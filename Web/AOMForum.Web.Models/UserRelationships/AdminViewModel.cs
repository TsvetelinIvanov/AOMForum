using AOMForum.Data.Models.Enums;

namespace AOMForum.Web.Models.UserRelationships
{
    public class AdminViewModel
    {
        public string? Id { get; init; }

        public string? FirstName { get; init; }
        
        public string? SecondName { get; init; }
        
        public string? LastName { get; init; }
        
        public int Age { get; init; }
        
        public string? BirthDate { get; init; }

        public string? Gender { get; init; }
        
        public string? Biography { get; init; }

        public string? ProfilePictureURL { get; init; }
    }
}