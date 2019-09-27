using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList
{
    public static class InitializeData
    {
        public static void Initialize(ListContext context, User userData)
        {
            if (userData != null)
            {
                GroupItem groupDefault = new GroupItem() { IsPrivate = null, Name = "My task", AdminUserId = userData.Id };
                context.Groups.Add(groupDefault);
                context.SaveChanges();

                //TaskItem t1 = new TaskItem
                //{ Title = "Date", Text = "Buy the floawers", ReleaseDate = DateTime.UtcNow, GroupItemId = groupDefault.Id };
                //TaskItem t2 = new TaskItem
                //{ Title = "buy phone", Text = "Galaxy j1", ReleaseDate = DateTime.UtcNow, GroupItemId = groupDefault.Id };
                //TaskItem t3 = new TaskItem
                //{ Title = "buy car", Text = "bmw 525i", ReleaseDate = DateTime.UtcNow, GroupItemId = groupDefault.Id };

                //context.Tasks.AddRange(new List<TaskItem> { t1, t2, t3 });

                context.SaveChanges();

                context.UsersGroups.AddRange(new UsersGroup { UserId = userData.Id, GroupItemId = groupDefault.Id, });

                context.SaveChanges();
            }
        }
    }
}