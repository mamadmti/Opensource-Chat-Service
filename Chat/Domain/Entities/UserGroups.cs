namespace Chat.Domain.Entities
{
    public class UserGroups:BaseEntity
    {
        public Users Users { get; set; }
        public long UsersId { get; set; }


        public Groups Groups { get; set; }
        public long GroupsId { get; set;}
    }
}
