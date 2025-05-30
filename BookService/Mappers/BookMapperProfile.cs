using AutoMapper;
using BookService.Models;
using BookService.Repositories.Data;

namespace BookService.Mappers
{
    public class BookMapperProfile : Profile
    {
        public BookMapperProfile()
        {
            CreateMap<Book, BookViewModel>();
            CreateMap<BookAddModel, Book>();
        }
    }
}
