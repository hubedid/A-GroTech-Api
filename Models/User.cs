using Microsoft.AspNetCore.Identity;

namespace A_GroTech_Api.Models
{
	public class User : IdentityUser
	{
        public string Name { get; set; }
        public string Address { get; set; }
        public string Picture { get; set; }
        public string NotificationToken { get; set; }
        public int Status { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
        public ICollection<DiscussionAnswer> DiscussionAnswers { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<UserArea> UserAreas { get; set; }
    }
}
