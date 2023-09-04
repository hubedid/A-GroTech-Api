using A_GroTech_Api.Dto;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IUserRepository
	{
		ICollection<User> GetUsers(PaginationDto paginationDto);
		User GetUser(string id);
		ICollection<Notification> GetNotificationsByUser(string userId, PaginationDto paginationDto);
		ICollection<Area> GetAreasByUser(string userId, PaginationDto paginationDto);
		ICollection<Product> GetProductsByUser(string userId, PaginationDto paginationDto);
		ICollection<Order> GetOrdersByUser(string userId, PaginationDto paginationDto);
		bool DeleteDiscussionByUser(string userId);
		bool Save();

	}
}
