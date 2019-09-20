using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ListContext : DbContext
    {
        public ListContext(DbContextOptions<ListContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<GroupItem> Groups { get; set; }
        public DbSet<UsersGroup> UsersGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersGroup>()
              .HasKey(t => new { t.UserId, t.GroupItemId});

            modelBuilder.Entity<UsersGroup>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UsersGroups)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UsersGroup>()
                .HasOne(sc => sc.GroupItem)
                .WithMany(c => c.Users)
                .HasForeignKey(sc => sc.GroupItemId);
        }
    }
}
