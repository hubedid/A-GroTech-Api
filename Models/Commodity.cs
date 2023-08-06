namespace A_GroTech_Api.Models
{
	public class Commodity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
        public CommodityType CommodityType { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public ICollection<Product> Products { get; set; }
		public ICollection<Prediction> Predictions { get; set; }
		public ICollection<CommodityArea> CommodityAreas { get; set; }
    }
}
