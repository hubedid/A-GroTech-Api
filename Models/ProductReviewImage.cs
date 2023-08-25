namespace A_GroTech_Api.Models
{
	public class ProductReviewImage
	{
		public int ProductReviewId { get; set; }
		public int ImageId { get; set; }
		public ProductReview ProductReview { get; set; }
		public Image Image { get; set; }
	}
}
