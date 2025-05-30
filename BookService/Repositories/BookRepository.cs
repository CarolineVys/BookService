using BookService.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace BookService.Repositories
{
    public class BookRepository : IBookRepository
    {
        private BookContext _context;

        public BookRepository(BookContext context)
        {
            _context = context;
        }

        public Task<List<Book>> GetBooksAsync()
        {
            return _context.Books.ToListAsync();
        }

        public Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            return _context.SaveChangesAsync();
        }
    }
}
