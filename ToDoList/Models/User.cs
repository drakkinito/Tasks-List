using System.Collections.Generic;

namespace ToDoList.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<TaskItem> TaskItem { get; set; }
    }
}
