using A_GroTech_Api.Data;
using A_GroTech_Api.Dto;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class OrderRepository : IOrderRepository
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public OrderRepository(IMapper mapper, DataContext context)
        {
			_mapper = mapper;
			_context = context;
		}
        public bool AddOrder(Order order)
		{
			_context.Add(order);
			return Save();
		}

		public bool DeleteOrder(int id)
		{
			var order = _context.Orders.Find(id);
			_context.Remove(order);
			return Save();
		}

		public Order GetOrder(int id)
		{
			var order = _context.Orders
				.Where(o => o.Id == id)
				.Include(o => o.Product)
				.Include(o => o.Buyer)
				.FirstOrDefault();
			return _mapper.Map<Order>(order);
		}

		public ICollection<Order> GetOrders(PaginationDto paginationDto)
		{
			var orders = _context.Orders
				.Include(o => o.Product)
				.Include(o => o.Buyer)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<Order>>(orders);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateOrder(Order order)
		{
			_context.Update(order);
			return Save();
		}
	}
}
