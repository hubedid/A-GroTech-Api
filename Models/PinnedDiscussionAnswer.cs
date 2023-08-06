namespace A_GroTech_Api.Models
{
	public class PinnedDiscussionAnswer
	{
        public int DiscussionId { get; set; }
		public int DiscussionAnswerId { get; set; }
		public Discussion Discussion { get; set; }
		public DiscussionAnswer DiscussionAnswer { get; set; }
	}
}
