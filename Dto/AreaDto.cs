namespace A_GroTech_Api.Dto
{
	public class AreaDto
	{
		public int Id { get; set; }
		public string Provinsi { get; set; }
		public string Kota { get; set; }
		public string Kecamatan { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
