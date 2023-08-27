namespace A_GroTech_Api.Dto
{
	public class PredictionDto
	{
		public int Id { get; set; }
		public long Price { get; set; }
		public DateTime Date { get; set; }
		public int CommodityId { get; set; }
		public int AreaId { get; set; }
	}
}
