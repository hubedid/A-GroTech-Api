namespace A_GroTech_Api.Models
{
	public class Product
	{
        public int Id { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }
        public string PriceUnit { get; set; }
        public long Price { get; set; }
        public long Stock { get; set; }
        public Commodity Commodity { get; set; }
        public Area Area { get; set; }
        public User User { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductReview> ProductReviews { get; set; }

	}
}
