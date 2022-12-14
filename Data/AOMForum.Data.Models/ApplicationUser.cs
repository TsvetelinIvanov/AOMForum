using AOMForum.Data.Common.Models;
using AOMForum.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AOMForum.Common.DataConstants.ApplicationUser;

namespace AOMForum.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            
            this.Posts = new HashSet<Post>();
            this.PostReports = new HashSet<PostReport>();
            this.PostVotes= new HashSet<PostVote>();
            this.Comments = new HashSet<Comment>();
            this.CommentReports = new HashSet<CommentReport>();
            this.CommentVotes= new HashSet<CommentVote>();
            this.Relationships = new HashSet<Relationship>();
            this.SentMessages = new HashSet<Message>();
            this.ReceivedMessages = new HashSet<Message>();

            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(SecondNameMaxLength, MinimumLength = SecondNameMinLength)]
        public string SecondName { get; set; }

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; }

        public int Age { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public GenderType Gender { get; set; }

        [Required]
        [StringLength(BiographyMaxLength, MinimumLength = BiographyMinLength)]
        public string Biography { get; set; }

        public string? ProfilePictureURL { get; set; }
        
        public DateTime CreatedOn { get; set; } //Audit info

        public DateTime? ModifiedOn { get; set; } //Audit info
        
        public bool IsDeleted { get; set; } //Deletable entity

        public DateTime? DeletedOn { get; set; } //Deletable entity

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<PostVote> PostVotes { get; set; }

        public virtual ICollection<PostReport> PostReports { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<CommentVote> CommentVotes { get; set; }

        public virtual ICollection<CommentReport> CommentReports { get; set; }

        public virtual ICollection<Relationship> Relationships { get; set; }

        [InverseProperty("Sender")]
        public virtual ICollection<Message> SentMessages { get; set; }

        [InverseProperty("Receiver")]
        public virtual ICollection<Message> ReceivedMessages { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}