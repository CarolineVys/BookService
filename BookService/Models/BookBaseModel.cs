using System.ComponentModel.DataAnnotations;

namespace BookService.Models
{
    public class BookBaseModel
    {
        [Required]
        public string Author { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
