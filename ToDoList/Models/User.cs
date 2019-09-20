using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoList.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<UsersGroup> UsersGroups { get; set; }
        public static object Claims { get; internal set; }
        public List<TaskItem> TaskItems { get; set; }
        public User()
        {
            UsersGroups = new List<UsersGroup>();
            TaskItems = new List<TaskItem>();
        }

    }
}
