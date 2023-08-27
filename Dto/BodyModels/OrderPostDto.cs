namespace A_GroTech_Api.Dto.BodyModels
{
	public class OrderPostDto
	{
		public int Id { get; set; }
		public string BuyerId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public string Notes { get; set; }
		public string Status { get; set; }
	}
}
