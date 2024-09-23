using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Library_MVCProject.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Author is required.")]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title cannot be longer than 50 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Genre is required.")]
        public string Genre { get; set; }
        [Required(ErrorMessage = "Publish Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public DateTime PublishDate { get; set; }
        [Required(ErrorMessage = "ISBN is required.")]
        public string ISBN { get; set; }
        [Required(ErrorMessage = "Copies Available is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Copies Available cannot be negative.")]
        public int CopiesAvailable { get; set; }
        [Required(ErrorMessage = "URL is required.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? Irl { get; set; }
    }
}
