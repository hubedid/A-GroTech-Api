namespace A_GroTech_Api.Models
{
	public class Discussion
	{
        public int Id { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
        public User User { get; set; }
        public bool IsSolved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<DiscussionAnswer> DiscussionAnswers { get; set; }
        public ICollection<DiscussionImage> DiscussionImages { get; set; }
        public ICollection<PinnedDiscussionAnswer> PinnedDiscussionAnswers { get; set; }

    }
}
