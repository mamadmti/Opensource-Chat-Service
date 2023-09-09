using Chat.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Chat.Domain.Contexts
{
    public class ChatDbContext: DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
 
        }
        public DbSet<ChatHistory> ChatHistories { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<Users> Users { get; set; }

    }
}
