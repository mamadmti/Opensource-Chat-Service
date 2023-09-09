using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities
{
    public class UsersConnections:BaseEntity
    {
        public string UserConnectionId { get; set; }
        public Users Users { get; set; }
        public long UsersId { get; set; }
    }
}
