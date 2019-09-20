using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class AddUserViewModel
    {
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        public string Name { get; set; }
    }
}
