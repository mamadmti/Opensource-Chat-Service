using Chat.Domain.Entities.Enums;
using SharedLibrary.Enums;

namespace Chat.Domain.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public RecordStatus RecordStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public Guid UserId { get; set; }
        public Guid? ApiKey { get; set; }
    }
}
