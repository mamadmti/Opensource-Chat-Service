using Chat.Domain.Entities.Enums;
using Chat.Domain.Entities;

namespace Chat.Domain.ViewModels
{
    public class UsersProfile
    {
        public string UserName { get; set; }
        public string GroupId { get; set; }
        public long UserId { get; set; }

        public long? UnReadcount { get; set; }

    }
}
