namespace Chat.Domain.Entities
{
    public class Groups:BaseEntity
    {
        public string GroupName { get; set; }
        public virtual ICollection<UserGroups> UserGroups { get; set; }
        public virtual ICollection<ChatHistory> ChatHistory { get; set; }

    }
}
