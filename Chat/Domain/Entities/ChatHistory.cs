using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities
{
    public class ChatHistory:BaseEntity
    {
        public string Message { get; set; }
    
        public Users Users { get; set; }
        [ForeignKey("Users")]
        public long? ToUserId { get; set; }

        public Groups Groups { get; set; }
        [ForeignKey("Groups")]
        public long? ToGroupId { get; set; }

    }
}
