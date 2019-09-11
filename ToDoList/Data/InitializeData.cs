using System;
using System.Linq;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList
{
    public static class InitializeData
    {
        public static void Initialize(ListContext context, User userData)
        {
            var isUser = context.Groups.Find(userData.Id);

            var groupCount = context.Groups.Count();
            var groupId = 0;
            if (groupCount != 0)
            {
                groupId = context.Groups.LastOrDefault().Id + 1;
            }
            else
            {
                groupId = 1;
            }

            if (isUser == null)
            {
                context.Groups.AddRange(
                new GroupItem
                {
                    IsPrivate = null,
                    Name = "My task",
                    UserRole = "Admin"
                });

                context.Tasks.AddRange(
                    new TaskItem
                    {
                        Title = "Date",
                        Text = "Buy the floawers",
                        ReleaseDate = DateTime.UtcNow,
                        GroupItemId = groupId
                    },
                    new TaskItem
                    {
                        Title = "buy phone",
                        Text = "Galaxy j1",
                        ReleaseDate = DateTime.UtcNow,
                        GroupItemId = groupId
                    },
                    new TaskItem
                    {
                        Title = "buy car",
                        Text = "bmw 525i",
                        ReleaseDate = DateTime.UtcNow,
                        GroupItemId = groupId
                    });


                context.UsersGroups.AddRange(
                    new UsersGroup
                    {
                        GroupItemId = groupId,
                        UserId = userData.Id
                    });

                context.SaveChanges();
            }
        }
    }
}