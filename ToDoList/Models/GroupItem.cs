using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class GroupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsPrivate { get; set; }
        public string UserRole { get; set; }

        public List<UsersGroup> UsersGroups { get; set; }

        public List<TaskItem> TaskItems { get; set; }

        public GroupItem()
        {
            UsersGroups = new List<UsersGroup>();
            TaskItems = new List<TaskItem>();
        }

    }
}

