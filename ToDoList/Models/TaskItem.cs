using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Empty Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Empty Text")]
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
