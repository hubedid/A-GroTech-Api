using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
		[HttpGet]
		[ProducesResponseType(200,Type = typeof(IEnumerable<User>))]
		public ActionResult GetUsers()
		{
			var users = _userRepository.GetUsers();

			if(!ModelState.IsValid) 
				return BadRequest(ModelState);

			return Ok(users);
		}
    }
}
