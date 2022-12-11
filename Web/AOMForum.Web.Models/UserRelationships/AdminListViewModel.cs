using System.ComponentModel.DataAnnotations;
using static AOMForum.Common.DisplayNames.ApplicationUser;

namespace AOMForum.Web.Models.UserRelationships
{
    public class AdminListViewModel
    {
        public string? Id { get; init; }

        [Display(Name = DisplayUserName)]
        public string? UserName { get; init; }

        [Display(Name = DisplayEmail)]
        public string? Email { get; init; }

        [Display(Name = DisplayFirstName)]
        public string? FirstName { get; init; }

        [Display(Name = DisplaySecondName)]
        public string? SecondName { get; init; }

        [Display(Name = DisplayLastName)]
        public string? LastName { get; init; }

        [Display(Name = DisplayFullName)]
        public string? FullName => this.FirstName + " " + this.SecondName + " " + this.LastName;

        [Display(Name = DisplayAge)]
        public int Age { get; init; }

        [Display(Name = DisplayBirthDate)]
        public string? BirthDate { get; init; }

        [Display(Name = DisplayGender)]
        public string? Gender { get; init; }

        [Display(Name = DisplayBiography)]
        public string? Biography { get; init; }

        [Display(Name = DisplayProfilePictureURL)]
        public string? ProfilePictureURL { get; init; }
    }
}