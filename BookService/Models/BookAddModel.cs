using System.ComponentModel.DataAnnotations;

namespace BookService.Models
{
    public class BookAddModel : BookBaseModel
    {
        [Required]
        public IFormFile Book { get; set; }
    }
}
