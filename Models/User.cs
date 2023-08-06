namespace A_GroTech_Api.Models
{
	public class User
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public long Phone { get; set; }
        public string Picture { get; set; }
        public string ActivationToken { get; set; }
        public string NotificationToken { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
        public DateTime VerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
        public ICollection<DiscussionAnswer> DiscussionAnswers { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<UserArea> UserAreas { get; set; }
    }
}
