using A_GroTech_Api.Dto;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin, User")]
	public class UserController : Controller
	{
		private readonly ResponseHelper _responseHelper;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IDiscussionRepository _discussionRepository;
		private readonly UserManager<User> _userManager;

		public UserController(
			ResponseHelper responseHelper, 
			IUserRepository userRepository, 
			IMapper mapper,
			IDiscussionRepository discussionRepository, 
			UserManager<User> userManager
			)
		{
			_responseHelper = responseHelper;
			_userRepository = userRepository;
			_mapper = mapper;
			_discussionRepository = discussionRepository;
			_userManager = userManager;
		}
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
		[Authorize(Roles = "Admin")]
		public IActionResult GetUsers([FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers(paginationDto));

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if(users.Any() != true)
					return NotFound(_responseHelper.Error("No users found", 404));

				return Ok(_responseHelper.Success("", users));
			}catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{userId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
		[ProducesResponseType(400)]
		/*[Authorize]*/
		public IActionResult GetUsers(string userId)
		{
			try
			{
				var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

				if (!ModelState.IsValid)
				{
					/*ModelState.AddModelError("message", "Something went wrong updating category");*/
					if (!ModelState.IsValid)
						return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				}
				if(user == null)
					return NotFound(_responseHelper.Error("User does not exist", 404));

				return Ok(_responseHelper.Success("", user));

			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpDelete("{userId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		[Authorize(Roles = "Admin, User")]
		public async Task<IActionResult> DeleteUser(string userId)
		{
			try
			{
				var user = _userRepository.GetUser(userId);
				if (user == null)
					return NotFound(_responseHelper.Error("User does not exist", 404));
				await _userManager.DeleteAsync(user);
				return Ok(_responseHelper.Success("User deleted successfuly"));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{userId}/discussions")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<DiscussionDto>))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult GetDiscussionsByUserWhoCreated(string userId, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var discussions = _mapper.Map<List<DiscussionDto>>(_discussionRepository.GetDiscussionsByUserWhoCreated(userId, paginationDto));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (discussions.Any() != true)
					return NotFound(_responseHelper.Error("No discussions found", 404));
				return Ok(_responseHelper.Success("", discussions));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpDelete("{userId}/discussions")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		[Authorize(Roles = "Admin, User")]
		public IActionResult DeleteDiscussionsByUserWhoCreated(string userId, [FromQuery]PaginationDto paginationDto)
		{
			try
			{
				var discussions = _discussionRepository.GetDiscussionsByUserWhoCreated(userId, paginationDto);
				if (discussions.Any() != true)
					return NotFound(_responseHelper.Error("No discussions found", 404));
				_userRepository.DeleteDiscussionByUser(userId);
				return Ok(_responseHelper.Success($"All {userId} Discussions deleted successfuly"));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{userId}/notifications")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<NotificationDto>))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult GetNotificationsByUser(string userId, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var notifications = _mapper.Map<List<NotificationDto>>(_userRepository.GetNotificationsByUser(userId, paginationDto));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (notifications.Any() != true)
					return NotFound(_responseHelper.Error("No notifications found", 404));
				return Ok(_responseHelper.Success("", notifications));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{userId}/areas")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<AreaDto>))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult GetAreasByUser(string userId, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var areas = _mapper.Map<List<AreaDto>>(_userRepository.GetAreasByUser(userId, paginationDto));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (areas.Any() != true)
					return NotFound(_responseHelper.Error("No areas found", 404));
				return Ok(_responseHelper.Success("", areas));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{userId}/products")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult GetProductsByUser(string userId, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var products = _mapper.Map<List<ProductDto>>(_userRepository.GetProductsByUser(userId, paginationDto));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (products.Any() != true)
					return NotFound(_responseHelper.Error("No products found", 404));
				return Ok(_responseHelper.Success("", products));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{userId}/orders")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<OrderDto>))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult GetOrdersByUser(string userId, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var orders = _mapper.Map<List<OrderDto>>(_userRepository.GetOrdersByUser(userId, paginationDto));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (orders.Any() != true)
					return NotFound(_responseHelper.Error("No orders found", 404));
				return Ok(_responseHelper.Success("", orders));
			}
			catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}
	}
}
