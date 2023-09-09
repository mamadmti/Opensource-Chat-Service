using Chat.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Domain.Entities.Enums;
using SharedLibrary.Enums;

namespace Chat.Helper
{
    public class messagehistory
    {
        public string Message { get; set; }
        public long? ToUserId { get; set; }
        public long? ToGroupId { get; set; }
        public DateTime CreateAt { get; set; }
        public string ShortCreateAt { get; set; }
        public Guid UserId { get; set; }
        public long Id { get; set; }
        public RecordStatus RecordStatus { get; set; }

        public long? UnreadCount { get; set; }
        public long? TotalCount { get; set; }

    }


}
