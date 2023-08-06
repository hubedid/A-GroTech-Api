namespace A_GroTech_Api.Models
{
	public class Area
	{
		public int Id { get; set; }
        public string Provinsi { get; set; }
		public string Kota { get; set; }
        public string Kecamatan { get; set; }
        public string Latitude { get; set; }
		public string Longitude { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<UserArea> UserAreas { get; set; }
        public ICollection<CommodityArea> CommodityAreas { get; set; }
        public ICollection<Prediction> Predictions { get; set; }
    }
}
