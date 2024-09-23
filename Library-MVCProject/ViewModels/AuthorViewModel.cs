using System.ComponentModel.DataAnnotations;

namespace Library_MVCProject.ViewModels
{
    public class AuthorViewModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }
        public string? Irl { get; set; }
        [Required(ErrorMessage = "Bio is required.")]
        public string Bio { get; set; }
        [Required(ErrorMessage = "Job is required.")]
        public string Job { get; set; }
        public string? Marriages { get; set; }
    }
}
