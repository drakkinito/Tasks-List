using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
    }
}
