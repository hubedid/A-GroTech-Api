using A_GroTech_Api.Models;

namespace A_GroTech_Api.Dto.BodyModels
{
	public class CommodityPostDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int CommodityTypeId { get; set; }
	}
}
