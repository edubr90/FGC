using Microsoft.EntityFrameworkCore;
using FCG.Domain.Entities;

namespace FCG.Infrastructure.Persistence
{
    public class FcgDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<UserGame> UserGames { get; set; } = null!;

        public FcgDbContext(DbContextOptions<FcgDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Email).IsUnique();
                b.Property(u => u.Name).IsRequired().HasMaxLength(200);
                b.Property(u => u.Email).IsRequired().HasMaxLength(320);
                b.Property(u => u.PasswordHash).IsRequired();
                b.Property(u => u.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<Game>(b =>
            {
                b.HasKey(g => g.Id);
                b.Property(g => g.Title).IsRequired().HasMaxLength(250);
                b.Property(g => g.Description).HasMaxLength(2000);
                b.Property(g => g.Price).HasColumnType("decimal(10,2)");
                b.Property(g => g.Developer).HasMaxLength(200);
            });

            modelBuilder.Entity<UserGame>(b =>
            {
                b.HasKey(ug => new { ug.UserId, ug.GameId });
                b.HasOne(ug => ug.User).WithMany(u => u.Library).HasForeignKey(ug => ug.UserId);
                b.HasOne(ug => ug.Game).WithMany().HasForeignKey(ug => ug.GameId);
                b.Property(ug => ug.AcquiredAt).IsRequired();
            });
        }
    }
}
