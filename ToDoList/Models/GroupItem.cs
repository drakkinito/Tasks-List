using System.Collections.Generic;

namespace ToDoList.Models
{
    public class GroupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsPrivate { get; set; }
        public int AdminUserId { get; set; }

        public IEnumerable<UsersGroup> Users { get; set; }

        public IEnumerable<TaskItem> TaskItems { get; set; }

        public GroupItem()
        {
            //Users = new List<UsersGroup>();
            //TaskItems = new List<TaskItem>();
        }
    }
}
