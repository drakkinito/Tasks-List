using System.Collections.Generic;

namespace ToDoList.ViewModels
{
    public class CreateGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsPrivate { get; set; }
        public string UserRole { get; set; }
        public IEnumerable<int> Users { get; set; }
    }
}
