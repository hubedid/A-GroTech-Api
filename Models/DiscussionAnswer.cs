namespace A_GroTech_Api.Models
{
	public class DiscussionAnswer
	{
        public int Id { get; set; }
		public string Message { get; set; }
		public int Likes { get; set; }
		public User AnsweredBy { get; set; }
		public Discussion Discussion { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public ICollection<DiscussionAnswerImage> DiscussionAnswerImages { get; set; }
		public ICollection<PinnedDiscussionAnswer> PinnedDiscussionAnswers { get; set; }

    }
}
