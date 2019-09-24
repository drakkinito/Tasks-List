using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class GroupsTasksViewModel
    {
        public IEnumerable<TaskItem> Tasks { get; set; }
        public IEnumerable<GroupItem> GroupItems { get; set; }
        public IEnumerable<UsersGroup> Users { get; set; }
    }

    //public class UserGroupViewModel : GroupItem
    //{
    //    public int UserId { get; set; }
    //    public string UserName { get; set; }

    //    public List<UserGroupViewModel> GetUser { get; set; }

    //    public string[] userList { get; set; }
    //}

}
