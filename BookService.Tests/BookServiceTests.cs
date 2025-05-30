using AutoMapper;
using BookService.Models;
using BookService.Repositories;
using BookService.Repositories.Data;
using BookService.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace BookService.Tests
{
    public class BookServiceTests
    {
        private IBookService _bookService;

        private Mock<IBookRepository> _repo;
        private Mock<IMapper> _mapper;
        private Mock<IAwsService> _awsService;

        [SetUp]
        public void Setup()
        {
            _repo = new Mock<IBookRepository>();
            _mapper = new Mock<IMapper>();
            _awsService = new Mock<IAwsService>();

            _bookService = new Services.BookService(_repo.Object, _mapper.Object, _awsService.Object);
        }

        [Test]
        public async Task GetAllBooks_ReturnsData()
        {
            _repo.Setup(r => r.GetBooksAsync()).ReturnsAsync(() => TestData.Books);
            _mapper.Setup(m => m.Map<List<BookViewModel>>(TestData.Books)).Returns(() => TestData.BookViewModels);

            var books = await _bookService.GetAllBooks();

            _repo.Verify(r => r.GetBooksAsync(), Times.Once);
            _mapper.Verify(m => m.Map<List<BookViewModel>>(It.IsAny<object>()), Times.Once);
        }

        [Test]
        public async Task AddBook_NecessaryStepsCalled()
        {
            var testBookToAdd = TestData.BookToAdd;

            var bytes = Encoding.UTF8.GetBytes("This is a test file");
            testBookToAdd.Book = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "test.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            _awsService.Setup(s => s.UploadPdfToS3Async(It.IsAny<MemoryStream>(), testBookToAdd.Book.FileName, testBookToAdd.Book.ContentType)).ReturnsAsync(() => "testLink1");
            _awsService.Setup(s => s.SendMessageToAuditQueueAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _repo.Setup(r => r.AddBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
            _mapper.Setup(m => m.Map<Book>(testBookToAdd)).Returns(() => new Book { Author = testBookToAdd.Author, Title = testBookToAdd.Title });

            await _bookService.AddBook(testBookToAdd);

            _awsService.Verify(s => s.UploadPdfToS3Async(It.IsAny<MemoryStream>(), testBookToAdd.Book.FileName, It.IsAny<string>()), Times.Once);
            _repo.Verify(r => r.AddBookAsync(It.IsAny<Book>()), Times.Once);
            _awsService.Verify(s => s.SendMessageToAuditQueueAsync(It.IsAny<string>()), Times.Once);
        }
    }
}