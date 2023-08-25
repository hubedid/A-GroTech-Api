namespace A_GroTech_Api.Models
{
	public class Image
	{
        public int Id { get; set; }
		public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
        public ICollection<NotificationImage> NotificationImages { get; set; }
		public ICollection<DiscussionImage> DiscussionImages { get; set; }
        public ICollection<DiscussionAnswerImage> DiscussionAnswerImages { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductReviewImage> ProductReviewImages { get; set; }
    }
}
