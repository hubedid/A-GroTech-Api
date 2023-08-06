namespace A_GroTech_Api.Models
{
	public class DiscussionImage
	{
        public int DiscussionId { get; set; }
		public int ImageId { get; set; }
		public Discussion Discussion { get; set; }
		public Image Image { get; set; }
	}
}
