using A_GroTech_Api.Dto;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IOrderRepository
	{
		ICollection<Order> GetOrders(PaginationDto paginationDto);
		Order GetOrder(int id);
		bool AddOrder(Order order);
		bool UpdateOrder(Order order);
		bool DeleteOrder(int id);
		bool Save();
	}
}
