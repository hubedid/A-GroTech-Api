namespace A_GroTech_Api.Dto.BodyModels
{
	public class ProductReviewPostDto
	{
		public string Message { get; set; }
		public int Likes { get; set; }
		public string ReviewedById { get; set; }
		public int ProductId { get; set; }
	}
}
