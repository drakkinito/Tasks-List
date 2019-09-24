using System;
using System.Collections.Generic;
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

        public bool? isAssign { get; set; }

        public int GroupItemId { get; set; }
        public GroupItem GroupItem { get; set; }

        public int? UserId { get; set; }
        public User Users { get; set; }
    }
}
