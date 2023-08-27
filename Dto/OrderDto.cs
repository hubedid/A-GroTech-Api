using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto
{
	public class OrderDto
	{
		public int Id { get; set; }
		public UserDto Buyer { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public string Notes { get; set; }
		public string Status { get; set; }
	}
}
