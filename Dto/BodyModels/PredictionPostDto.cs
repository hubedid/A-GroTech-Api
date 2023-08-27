namespace A_GroTech_Api.Dto.BodyModels
{
	public class PredictionPostDto
	{
		public long Price { get; set; }
		public DateTime Date { get; set; }
		public int CommodityId { get; set; }
		public int AreaId { get; set; }
	}
}
