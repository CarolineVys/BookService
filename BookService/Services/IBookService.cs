using BookService.Models;

namespace BookService.Services
{
    public interface IBookService
    {
        public Task<List<BookViewModel>> GetAllBooks();
        public Task AddBook(BookAddModel book);
    }
}
