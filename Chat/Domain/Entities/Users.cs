using Chat.Domain.Entities.Enums;

namespace Chat.Domain.Entities
{
    public class Users:BaseEntity
    {
        
        public UserLevel UserLevel { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public DateTime? VerificationCreatedAt { get; set; }
        public DateTime? LastTryAt { get; set; }


		public string? TemporaryValidationText { get; set; }      
        public long OverAllTryCount { get; set; }




        public virtual ICollection<UserGroups> UserGroups { get; set; }
        public virtual ICollection<ChatHistory> ChatHistory { get; set; }
        public virtual ICollection<UsersConnections> UsersConnections { get; set; }

    }
}
