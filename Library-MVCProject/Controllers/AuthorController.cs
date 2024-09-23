using Library_MVCProject.Models;
using Library_MVCProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Library_MVCProject.Controllers
{
    public class AuthorController : Controller
    {
        public static List<Author> _authors = new List<Author>()   // A static list to store authors
        {
            new Author
            {
                Id = 1,
                FirstName = "Fyodor",
                LastName =  "Dostoyevski",
                Job = "Writer, Journalist, Translator, Military Engineer, Political Prisoner",
                Marriages = "Anna Dostoyevskaya (m. 1867–1881), Mariya Dmitriyevna İsayeva (m. 1857–1864)",
                DateOfBirth = new DateTime(1821, 11, 11),
                Irl = "https://upload.wikimedia.org/wikipedia/commons/7/78/Vasily_Perov_-_%D0%9F%D0%BE%D1%80%D1%82%D1%80%D0%B5%D1%82_%D0%A4.%D0%9C.%D0%94%D0%BE%D1%81%D1%82%D0%BE%D0%B5%D0%B2%D1%81%D0%BA%D0%BE%D0%B3%D0%BE_-_Google_Art_Project.jpg",
                Bio = "Fyodor Dostoevsky (1821–1881) was a Russian novelist and philosopher, widely regarded as one of the greatest literary figures in history. His works delve deeply into human psychology, morality, and existentialism, often exploring themes of suffering, guilt, and redemption. Notable novels like Crime and Punishment, The Brothers Karamazov, and The Idiot have left a lasting impact on world literature. Dostoevsky's own turbulent life, marked by poverty, exile, and personal struggles, profoundly influenced his writing. His exploration of the human condition continues to resonate with readers worldwide."
            },
            new Author
            {
                Id = 2,
                FirstName = "Victor",
                LastName =  "Hugo",
                Job = "Writer, Poet, Playwirght, Politician, Activist, Artist",
                Marriages = "Adèle Foucher (1822-1868)",
                DateOfBirth = new DateTime(1802, 02, 26),
                Irl = "https://upload.wikimedia.org/wikipedia/commons/e/e6/Victor_Hugo_by_%C3%89tienne_Carjat_1876_-_full.jpg",
                Bio = "Victor Hugo (1802–1885) was a French poet, novelist, and playwright, best known for his epic works Les Misérables and The Hunchback of Notre-Dame. A leading figure of the Romantic movement, Hugo’s writing often focused on themes of social justice, human rights, and the struggles of the poor. He was also deeply involved in French politics, advocating for democracy and freedom. Exiled for his political views, Hugo spent nearly two decades abroad, during which he wrote some of his most influential works. His literary legacy has had a profound impact on French and world literature."
            },
            new Author
            {
                Id = 3,
                FirstName = "John",
                LastName =  "Steinbeck",
                Job = "Writer, Journalist, Laborer, Film Script Writer, Marine Biologist (Amateur)",
                Marriages = "Elaine Anderson Steinbeck (m. 1950–1968), Gwyndolyn Conger (m. 1943–1948), Carol Henning (m. 1930–1943)",
                DateOfBirth = new DateTime(1902, 02, 27),
                Irl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d7/John_Steinbeck_1939_%28cropped%29.jpg/800px-John_Steinbeck_1939_%28cropped%29.jpg",
                Bio = "John Steinbeck (1902–1968) was an American author known for his powerful depiction of working-class life and social issues in the early 20th century. His most famous works include The Grapes of Wrath, which portrays the hardships of the Great Depression, Of Mice and Men, and East of Eden. Steinbeck's writing often explores themes of human dignity, injustice, and resilience, especially among marginalized groups. He won the Nobel Prize in Literature in 1962 for his contributions to American literature. His works remain influential for their emotional depth and social commentary."
            }
        };

        public IActionResult List()   // Method to display the list of authors
        {
            var authorList = _authors.Where(x => x.IsDeleted == false).ToList();      // Only return authors that are not deleted

            return View(authorList);
        }
        public IActionResult Details(int id)    // Method to display details of a specific author by id
        {
            var author = _authors.Find(x => x.Id == id);

            return View(author);
        }
        [HttpGet]    // GET method to render the 'Create' form
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]       // POST method to handle the form submission and create a new author
        public IActionResult Create(AuthorViewModel formData)
        {
            if (!ModelState.IsValid)     // Check if the model state is valid
            {
                return View(formData);
            }
            int maxId = _authors.Max(x => x.Id);    // Find the maximum id to assign a new unique id to the newly created author

            var newAuthor = new Author()
            {
                Id = maxId + 1,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Job = formData.Job,
                Marriages = formData.Marriages,
                DateOfBirth = formData.DateOfBirth,
                Irl = formData.Irl,
                Bio = formData.Bio,
            };
            _authors.Add(newAuthor);

            return RedirectToAction("List");   // After successful creation, redirect to the list of authors
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _authors.Find(x => x.Id == id);

            var viewModel = new AuthorViewModel()     // Create a view model to pass the existing author's data to the view
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Job = author.Job,
                Marriages = author.Marriages,
                DateOfBirth = author.DateOfBirth,
                Irl = author.Irl,
                Bio = author.Bio,
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(AuthorViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);

            }

            // Find the existing author and update their details with the new form data

            var author = _authors.Find(x => x.Id == formData.Id);

            author.FirstName = formData.FirstName;
            author.LastName = formData.LastName;
            author.Job = formData.Job;
            author.Marriages = formData.Marriages;
            author.DateOfBirth = formData.DateOfBirth;
            author.Irl = formData.Irl;
            author.Bio = formData.Bio;

            return RedirectToAction("List");    // After successful update, redirect to the list of authors
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var author = _authors.Find(x => x.Id == id);

            author.IsDeleted = true;        // Mark the author as deleted

            return RedirectToAction("List");
        }
    }
}
