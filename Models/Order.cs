namespace A_GroTech_Api.Models
{
	public class Order
	{
		public int Id { get; set; }
		public User Buyer { get; set; }
		public Product Product { get; set; }
		public int Quantity { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
