using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto
{
	public class ProductReviewDto
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public int Likes { get; set; }
		public UserDto ReviewedBy { get; set; }
		public int ProductId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
