namespace BookStore.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public Guid BookId { get; set; }

        public Book? Book { get; set; }

        public int Count { get; set; }
    }
}
