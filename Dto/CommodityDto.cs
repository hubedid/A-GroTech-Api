using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto
{
	public class CommodityDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public CommodityType CommodityType { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
