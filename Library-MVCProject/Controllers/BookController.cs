using Library_MVCProject.Models;
using Library_MVCProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Library_MVCProject.Controllers
{
    public class BookController : Controller
    {
        static List<Book> _books = new List<Book>()      // A static list to store books
        {

            new Book
            {
                Id = 1,
                Title = "Crime And Punishment",
                Genre = "Psychological",
                PublishDate = new DateTime(1866, 01, 01),
                ISBN = "9786054439706",
                CopiesAvailable = 5602369,
                Irl = "https://img.kitapyurdu.com/v1/getImage/fn:293342/wh:true/wi:800",
                AuthorId = 1,
            },
            new Book
            {
                Id = 2,
                Title = "Les Misérables",
                Genre = "Historical Novel",
                PublishDate = new DateTime(1862, 01, 01),
                ISBN = "9781909621497",
                CopiesAvailable = 3605369,
                Irl = "https://wordsworth-editions.com/wp-content/uploads/2024/02/9781853260858.jpg",
                AuthorId=2,
            },
            new Book
            {
                Id = 3,
                Title = "The Grapes of Wrath",
                Genre = "Social Criticism",
                PublishDate = new DateTime(1939, 04, 14),
                ISBN = "9789750531170",
                CopiesAvailable = 5602369,
                Irl = "https://upload.wikimedia.org/wikipedia/commons/a/ad/The_Grapes_of_Wrath_%281939_1st_ed_cover%29.jpg",
                AuthorId = 3,
            }
        };
        // Since I use the same code in 4 different places, we make this code more useful by including it in the method.
        private void PopulateAuthors()     
        {
            // Create a list of authors as SelectListItems and assign to ViewBag
            List<SelectListItem> values = (from x in AuthorController._authors.ToList()  
                                           select new SelectListItem
                                           {
                                               Text = x.FullName,
                                               Value = x.Id.ToString()
                                           }).ToList();
            ViewBag.Authors = values;
        }
        public IActionResult List()        // Method to display the list of books
        {

            var bookList = _books.Where(x => x.IsDeleted == false).ToList();      // Only return books that are not marked as deleted

            return View(bookList);
        }
        public IActionResult Details(int id)    // Method to display details of a specific book by id
        {
            BookViewModel model = new BookViewModel();

            var book = _books.Find(x => x.Id == id);

            ViewBag.authorfname = AuthorController._authors.Where(x => x.Id == book.AuthorId).First().FullName;

            // Assign book details to the ViewModel

            model.Title = book.Title;
            model.Genre = book.Genre;
            model.PublishDate = book.PublishDate;
            model.ISBN = book.ISBN;
            model.CopiesAvailable = book.CopiesAvailable;
            model.Irl = book.Irl;



            return View(model);
        }

        [HttpGet]    // GET method to render the 'Create' form
        public IActionResult Create()
        {

            PopulateAuthors();
            return View();
        }
        [HttpPost]      // POST method to handle form submission and create a new book
        public IActionResult Create(BookViewModel formData)
        {

            if (!ModelState.IsValid)     // Check if the model state is valid 
            {
                PopulateAuthors();
                return View(formData);
            }
            int maxId = _books.Max(x => x.Id);       // Find the maximum id to assign a new unique id to the newly created book
            // Create a new book with the form data
            var newBook = new Book()
            {
                Id = maxId + 1,
                AuthorId = formData.AuthorId,
                Title = formData.Title,
                Genre = formData.Genre,
                PublishDate = formData.PublishDate,
                ISBN = formData.ISBN,
                CopiesAvailable = formData.CopiesAvailable,
                Irl = formData.Irl,

            };
            _books.Add(newBook);

            return RedirectToAction("List");        // After successful creation, redirect to the list of books
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            PopulateAuthors();

            var book = _books.Find(x => x.Id == id);

            var viewModel = new BookViewModel()      // Create a view model to pass the existing book's data to the view
            {
                Id = book.Id,
                AuthorId = book.AuthorId,
                Title = book.Title,
                Genre = book.Genre,
                PublishDate = book.PublishDate,
                ISBN = book.ISBN,
                CopiesAvailable = book.CopiesAvailable,
                Irl = book.Irl,

            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel formData)
        {


            if (!ModelState.IsValid)             // Check if the form data is valid before proceeding with the update
            {
                PopulateAuthors();
                return View(formData);
            }

            // Find the existing book and update its details with the new form data

            var book = _books.Find(x => x.Id == formData.Id);

            book.AuthorId = formData.AuthorId;
            book.Title = formData.Title;
            book.Genre = formData.Genre;
            book.PublishDate = formData.PublishDate;
            book.ISBN = formData.ISBN;
            book.CopiesAvailable = formData.CopiesAvailable;
            book.Irl = formData.Irl;

            return RedirectToAction("List");        // After successful update, redirect to the list of books
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var book = _books.Find(x => x.Id == id);
            book.IsDeleted = true;      // Mark the book as deleted

            return RedirectToAction("List");
        }
    }
}
