using BookService.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace BookService.Repositories
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
    }
}
