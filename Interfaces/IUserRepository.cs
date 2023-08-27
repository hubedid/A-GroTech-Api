using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IUserRepository
	{
		ICollection<User> GetUsers();
		User GetUser(string id);
		ICollection<Notification> GetNotificationsByUser(string userId);
		ICollection<Area> GetAreasByUser(string userId);
		ICollection<Product> GetProductsByUser(string userId);
		ICollection<Order> GetOrdersByUser(string userId);
	}
}
