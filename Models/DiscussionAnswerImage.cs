namespace A_GroTech_Api.Models
{
	public class DiscussionAnswerImage
	{
        public int DiscussionAnswerId { get; set; }
		public int ImageId { get; set; }
		public DiscussionAnswer DiscussionAnswer { get; set; }
		public Image Image { get; set; }
	}
}
