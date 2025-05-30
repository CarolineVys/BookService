using BookService.Models;
using BookService.Repositories.Data;

namespace BookService.Tests
{
    public static class TestData
    {
        public static List<Book> Books = new List<Book>()
        {
            new Book
            {
                Id = 1,
                Author = "testAuthor1",
                Title = "testTitle1",
                Link = "testLink1"
            },
            new Book
            {
                Id = 2,
                Author = "testAuthor2",
                Title = "testTitle2",
                Link = "testLink2"
            }
        };

        public static List<BookViewModel> BookViewModels = new List<BookViewModel>
        {
            new BookViewModel
            {
                Author = "testAuthor1",
                Title = "testTitle1",
                Link = "testLink1"
            },
            new BookViewModel
            {
                Author = "testAuthor2",
                Title = "testTitle2",
                Link = "testLink2"
            }
        };

        public static BookAddModel BookToAdd = new BookAddModel
        {
            Author = "testAuthor1",
            Title = "testTitle1"
        };
    }
}
