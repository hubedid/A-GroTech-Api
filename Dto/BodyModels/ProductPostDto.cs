namespace A_GroTech_Api.Dto.BodyModels
{
	public class ProductPostDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string PriceUnit { get; set; }
		public long Price { get; set; }
		public long Stock { get; set; }
		public int CommodityId { get; set; }
		public int AreaId { get; set; }
		public string OwnerId { get; set; }
		public int Status { get; set; }
	}
}
