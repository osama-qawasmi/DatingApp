using API.Enitities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>().HasKey(k => new {k.SourceUserId, k.TargetUserId});

            builder.Entity<UserLike>().HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>().HasOne(s => s.TargetUser)
            .WithMany(l => l.LikedByUsers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>().HasOne(s => s.Sender)
            .WithMany(s => s.MessagesSent)
            //.HasForeignKey(s => s.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>().HasOne(r => r.Recipient)
            .WithMany(r => r.MessagesReceived)
            //.HasForeignKey(r => r.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}