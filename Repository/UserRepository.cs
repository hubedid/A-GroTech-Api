using A_GroTech_Api.Data;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;

		public UserRepository(DataContext context)
		{
			_context = context;
		}
		public ICollection<User> GetUsers()
		{
			return _context.Users.OrderBy(x => x.Id).ToList();
		}
	}
}
