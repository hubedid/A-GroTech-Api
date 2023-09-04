using A_GroTech_Api.Data;
using A_GroTech_Api.Dto;
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

		public bool DeleteDiscussionByUser(string userId)
		{
			var discussions = _context.Discussions.Where(d => d.User.Id == userId).ToList();
			if (discussions != null)
			{
				foreach (var item in discussions)
				{
					var discussionImage = _context.DiscussionImages.Where(di => di.Discussion.Id == item.Id).ToList();
					if (discussionImage != null)
					{
						foreach (var image in discussionImage)
						{
							_context.Remove(image);
						}
					}
					var discussionAnswer = _context.DiscussionAnswers.Where(da => da.Discussion.Id == item.Id).ToList();
					if (discussionAnswer != null)
					{
						foreach (var answer in discussionAnswer)
						{
							var answerImage = _context.DiscussionAnswerImages.Where(dai => dai.DiscussionAnswer.Id == answer.Id).ToList();
							if (answerImage != null)
							{
								foreach (var image in answerImage)
								{
									_context.Remove(image);
								}
							}
							var pinnedAnswer = _context.PinnedDiscussionAnswers.Where(pda => pda.DiscussionAnswer.Id == answer.Id).ToList();
							if (pinnedAnswer != null)
							{
								foreach (var pinned in pinnedAnswer)
								{
									_context.Remove(pinned);
								}
							}
							_context.Remove(answer);
						}
					}
					_context.Remove(item);
				}
			}
			return Save();
		}

		public ICollection<Area> GetAreasByUser(string userId, PaginationDto paginationDto)
		{
			var areas = _context.UserAreas
				.Where(ua => ua.User.Id == userId)
				.Select(ua => ua.Area)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<Area>>(areas);

		}

		public ICollection<Notification> GetNotificationsByUser(string userId, PaginationDto paginationDto)
		{
			var notifications = _context.Notifications
				.Where(n => n.User.Id == userId)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<Notification>>(notifications);
		}

		public ICollection<Order> GetOrdersByUser(string userId, PaginationDto paginationDto)
		{
			var orders = _context.Orders
				.Where(o => o.Buyer.Id == userId)
				.Include(o => o.Product)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<Order>>(orders);
		}

		public ICollection<Product> GetProductsByUser(string userId, PaginationDto paginationDto)
		{
			
			var products = _context.Products
				.Where(p => p.User.Id == userId)
				.Include(p => p.Area)
				.Include(p => p.Commodity)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
			return _mapper.Map<ICollection<Product>>(products);
		}

		public User GetUser(string id)
		{
			return _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
		}

		public ICollection<User> GetUsers(PaginationDto paginationDto)
		{
			return _userManager.Users
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved >= 0 ? true : false;
		}
	}
}
