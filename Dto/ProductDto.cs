using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PriceUnit { get; set; }
		public long Price { get; set; }
		public long Stock { get; set; }
		public int CommodityId { get; set; }
		public int AreaId { get; set; }
		public string OwnerId { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
