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
    }
}
