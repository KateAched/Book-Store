using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Store
    {
        [Required]

        public int Id { get; set; }

        [Required]

        public string? Name { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
