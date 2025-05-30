using AutoMapper;
using BookService.Models;
using BookService.Repositories;
using BookService.Repositories.Data;
using System.Text.Json;

namespace BookService.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IAwsService _awsService;

        public BookService(IBookRepository bookRepository, IMapper mapper, IAwsService awsService)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _awsService = awsService;
        }

        public async Task<List<BookViewModel>> GetAllBooks()
        {
            var books = await _bookRepository.GetBooksAsync();
            return _mapper.Map<List<BookViewModel>>(books);
        }

        public async Task AddBook(BookAddModel addBookModel)
        {
            var book = _mapper.Map<Book>(addBookModel);
            using (var memoryStream = new MemoryStream())
            {
                await addBookModel.Book.CopyToAsync(memoryStream);
                book.Link = await _awsService.UploadPdfToS3Async(memoryStream, addBookModel.Book.FileName, addBookModel.Book.ContentType);
            }
            await _bookRepository.AddBookAsync(book);

            var auditModel = new AuditBookAdded
            {
                Id = book.Id,
                Timestamp = DateTime.UtcNow,
                Name = book.Title,
                Url = book.Link
            };
            await _awsService.SendMessageToAuditQueueAsync(JsonSerializer.Serialize(auditModel));
        }
    }
}
