using A_GroTech_Api.Data;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using Microsoft.AspNetCore.Identity;

namespace A_GroTech_Api.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;
		private readonly UserManager<User> _userManager;

		public UserRepository(DataContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
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
