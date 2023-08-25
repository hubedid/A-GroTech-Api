using A_GroTech_Api.Dto;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using A_GroTech_Api.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly ResponseHelper _responseHelper;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IDiscussionRepository _discussionRepository;

		public UserController(
			ResponseHelper responseHelper, 
			IUserRepository userRepository, 
			IMapper mapper,
			IDiscussionRepository discussionRepository
			)
		{
			_responseHelper = responseHelper;
			_userRepository = userRepository;
			_mapper = mapper;
			_discussionRepository = discussionRepository;
		}
		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
		/*[Authorize]*/
		public IActionResult GetUsers()
		{
			try
			{
				var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());

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

		[HttpGet("{userId}/discussions")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<DiscussionDto>))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult GetDiscussionsByUserWhoCreated(string userId)
		{
			try
			{
				var discussions = _mapper.Map<List<DiscussionDto>>(_discussionRepository.GetDiscussionsByUserWhoCreated(userId));
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
	}
}
