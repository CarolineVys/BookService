using BookService.Models;
using BookService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromForm] BookAddModel addBookModel)
        {
            await _bookService.AddBook(addBookModel);
            return Ok();
        }
    }
}