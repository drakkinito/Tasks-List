using System;
using System.Linq;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList
{
    public static class InitializeData
    {
        public static void Initialize(ListContext context)
        {
            if (!context.Tasks.Any())
            {
                context.Tasks.AddRange(
                new TaskItem
                {
                    Title = "Hello0",
                    Text = "my first task0",
                    ReleaseDate = DateTime.Now
                },
                new TaskItem
                {
                    Title = "Hello1",
                    Text = "my first task2",
                    ReleaseDate = DateTime.Now
                },
                new TaskItem
                {
                    Title = "Hello2",
                    Text = "my first task2",
                    ReleaseDate = DateTime.Now
                }
                );
                context.SaveChanges();
            }
        }
    }
}