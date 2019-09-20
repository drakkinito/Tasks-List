using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class UsersGroup
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int GroupItemId { get; set; }
        public GroupItem GroupItem { get; set; }
    }
}
