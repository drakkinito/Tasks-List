using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class ModelGroupAndTask
    {
        public IEnumerable<TaskItem> Tasks { get; set; }
        public IEnumerable<UsersGroup> UsersGroups { get; set; }
    }
}
