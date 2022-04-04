namespace BookStore.ViewModels
{
    public class CreateBookViewModel
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ISBN { get; set; }

        public decimal Price { get; set; }

        public int BookAuthorId { get; set; }

        public int StoreId { get; set; }

        public List<IFormFile> Pictures { get; set; }
    }
}
