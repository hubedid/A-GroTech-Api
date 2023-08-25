namespace A_GroTech_Api.Models
{
	public class ProductReview
	{
        public int Id { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
        public User ReviewedBy { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ProductReviewImage> ProductReviewImages { get; set; }
    }
}
