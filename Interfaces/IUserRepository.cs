using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IUserRepository
	{
		ICollection<User> GetUsers();
	}
}
