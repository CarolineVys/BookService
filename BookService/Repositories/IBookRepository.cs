using BookService.Repositories.Data;

namespace BookService.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksAsync();
        Task AddBookAsync(Book book);
    }
}
