namespace A_GroTech_Api.Models
{
	public class ProductImage
	{
        public int ProductId { get; set; }
		public int ImageId { get; set; }
		public Product Product { get; set; }
		public Image Image { get; set; }
    }
}
