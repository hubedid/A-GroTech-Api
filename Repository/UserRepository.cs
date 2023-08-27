using A_GroTech_Api.Data;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public UserRepository(DataContext context, UserManager<User> userManager, IMapper mapper)
		{
			_context = context;
			_userManager = userManager;
			_mapper = mapper;
		}

		public ICollection<Area> GetAreasByUser(string userId)
		{
			var areas = _context.UserAreas
				.Where(ua => ua.User.Id == userId)
				.Select(ua => ua.Area)
				.ToList();
			return _mapper.Map<ICollection<Area>>(areas);

		}

		public ICollection<Notification> GetNotificationsByUser(string userId)
		{
			var notifications = _context.Notifications
				.Where(n => n.User.Id == userId)
				.ToList();
			return _mapper.Map<ICollection<Notification>>(notifications);
		}

		public ICollection<Order> GetOrdersByUser(string userId)
		{
			var orders = _context.Orders
				.Where(o => o.Buyer.Id == userId)
				.Include(o => o.Product)
				.ToList();
			return _mapper.Map<ICollection<Order>>(orders);
		}

		public ICollection<Product> GetProductsByUser(string userId)
		{
			
			var products = _context.Products
				.Where(p => p.User.Id == userId)
				.Include(p => p.Area)
				.Include(p => p.Commodity)
				.ToList();
			return _mapper.Map<ICollection<Product>>(products);
		}

		public User GetUser(string id)
		{
			return _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
		}

		public ICollection<User> GetUsers()
		{
			return _userManager.Users.ToList();
		}
	}
}
