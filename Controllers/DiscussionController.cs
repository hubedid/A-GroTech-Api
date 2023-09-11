using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Helpers;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace A_GroTech_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin, User")]
	public class DiscussionController : Controller
	{
		private readonly ResponseHelper _responseHelper;
		private readonly IDiscussionRepository _discussionRepository;
		private readonly IMapper _mapper;
		private readonly IDiscussionAnswerRepository _discussionAnswerRepository;
		private readonly IUserRepository _userRepository;

		public DiscussionController(
			ResponseHelper responseHelper, 
			IDiscussionRepository discussionRepository, 
			IMapper mapper,
			IDiscussionAnswerRepository discussionAnswerRepository,
			IUserRepository userRepository
			)
        {
			_responseHelper = responseHelper;
			_discussionRepository = discussionRepository;
			_mapper = mapper;
			_discussionAnswerRepository = discussionAnswerRepository;
			_userRepository = userRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<DiscussionDto>))]
		[ProducesResponseType(400)]
		public IActionResult GetDiscussions([FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var discussions = _mapper.Map<List<DiscussionDto>>(_discussionRepository.GetDiscussions(paginationDto));
				if(!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if(discussions.Any() != true)
					return Ok(_responseHelper.Success("No discussions found"));
				return Ok(_responseHelper.Success("",discussions));
			}catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			}
			catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpGet("{discussionId}")]
		[ProducesResponseType(200, Type = typeof(DiscussionDto))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult GetDiscussion(int discussionId)
		{
			try
			{
				var discussion = _mapper.Map<DiscussionDto>(_discussionRepository.GetDiscussion(discussionId));
				if(!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if(discussion == null)
					return NotFound(_responseHelper.Error("No discussion found"));
				return Ok(_responseHelper.Success("", discussion));
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

		[HttpGet("{discussionId}/answers")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<DiscussionAnswerDto>))]
		public IActionResult GetDiscussionAnswersByDiscussionId(int discussionId, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var discussionAnswers = _mapper.Map<List<DiscussionAnswerDto>>(_discussionAnswerRepository.GetDiscussionAnswersByDiscussionId(discussionId, paginationDto));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (discussionAnswers.Any() != true)
					return NotFound(_responseHelper.Error("No discussion answers found", 404));
				return Ok(_responseHelper.Success("", discussionAnswers));
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

		[HttpGet("search")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<DiscussionDto>))]
		public IActionResult SearchDiscussions([FromQuery] string search, [FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var discussions = _mapper.Map<List<DiscussionDto>>(_discussionRepository.SearchDiscussions(search, paginationDto));
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

		[HttpGet("{discussionId}/pinned-answers")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<DiscussionAnswerDto>))]
		public IActionResult GetDiscussionPinnedAnswersByDiscussionId(int discussionId)
		{
			try
			{
				var discussionAnswers = _mapper.Map<List<DiscussionAnswerDto>>(_discussionRepository.GetPinnedAnswer(discussionId));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (discussionAnswers.Any() != true)
					return NotFound(_responseHelper.Error("No discussion answers found", 404));
				return Ok(_responseHelper.Success("", discussionAnswers));
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

		[HttpPatch("{discussionId}/pinned-answer")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult AddPinnedAnswerToDiscussion(int discussionId, [Required][FromQuery] int answerId)
		{
			try
			{
				if (!_discussionRepository.AddPinnedAnswer(discussionId, answerId))
				{
					throw new Exception("Adding pinned answer to discussion failed on save.");
				}
				return Ok(_responseHelper.Success("Pinned answer added to discussion successfully"));
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

		[HttpDelete("{discussionId}/pinned-answer")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult RemovePinnedAnswerFromDiscussion(int discussionId, [Required][FromQuery] int answerId)
		{
			try
			{
				if (!_discussionRepository.RemovePinnedAnswer(discussionId, answerId))
				{
					throw new Exception("Removing pinned answer from discussion failed on save.");
				}
				return Ok(_responseHelper.Success("Pinned answer removed from discussion successfully"));
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

		[HttpGet("{discussionId}/images")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ImageDto>))]
		public IActionResult GetDiscussionImagesByDiscussionId(int discussionId)
		{
			try
			{
				var images = _mapper.Map<List<ImageDto>>(_discussionRepository.GetDiscussionImage(discussionId));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (images.Any() != true)
					return NotFound(_responseHelper.Error("No images found", 404));
				return Ok(_responseHelper.Success("", images));
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

		[HttpPost("{discussionId}/images")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult CreateDiscussionImage(int discussionId, [FromBody] List<ImagePostDto> imagePostDto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_discussionRepository.AddDiscussionImage(discussionId, imagePostDto))
				{
					throw new Exception("Creating an image failed on save.");
				}
				return Ok(_responseHelper.Success("Image created successfully"));
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

		[HttpDelete("{discussionId}/images")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult DeleteDiscussionImage(int discussionId, [Required][FromQuery] int imageId)
		{
			try
			{
				if (!_discussionRepository.RemoveDiscussionImage(discussionId, imageId))
				{
					throw new Exception("Deleting an image failed on save.");
				}
				return Ok(_responseHelper.Success("Image deleted successfully"));
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

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult CreateDiscussion([FromBody] DiscussionPostDto discussionPostDto)
		{
			try
			{
				var discussion = _mapper.Map<Discussion>(discussionPostDto);
				discussion.User = _userRepository.GetUser(discussionPostDto.CreatedById);
				discussion.CreatedAt = DateTime.Now;
				discussion.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_discussionRepository.CreateDiscussion(discussion))
				{
					throw new Exception("Creating an discussion failed on save.");
				}
				return Ok(_responseHelper.Success("Discussion created successfully"));
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

		[HttpPut("{discussionId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult UpdateDiscussion(int discussionId, [FromBody] DiscussionPutDto discussionPutDto)
		{
			try
			{
				var discussion = _discussionRepository.GetDiscussion(discussionId);
				if (discussion == null)
					return NotFound(_responseHelper.Error("No discussion found", 404));

				_mapper.Map(discussionPutDto, discussion);
				discussion.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_discussionRepository.UpdateDiscussion(discussion))
					throw new Exception("Updating an discussion failed on save.");

				return Ok(_responseHelper.Success("Discussion updated successfully"));
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

		[HttpDelete("{discussionId}")]
		[ProducesResponseType(200, Type = typeof(ApiResponse))]
		[ProducesResponseType(400)]
		public IActionResult DeleteDiscussion(int discussionId)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (!_discussionRepository.DeleteDiscussion(discussionId))
					throw new Exception("Something went wrong in deleting Discussion");
				return Ok(_responseHelper.Success("Discussion deleted successfully"));
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


		[HttpPatch("{discussionId}/like")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult AddLikesToDiscussion(int discussionId)
		{
			try
			{
				if (!_discussionRepository.AddLike(discussionId))
				{
					throw new Exception("Adding likes to discussion failed on save.");
				}
				return Ok(_responseHelper.Success("Likes added to discussion successfully"));
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

		[HttpDelete("{discussionId}/like")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult RemoveLikesFromDiscussion(int discussionId)
		{
			try
			{
				if (!_discussionRepository.RemoveLike(discussionId))
				{
					throw new Exception("Removing likes from discussion failed on save.");
				}
				return Ok(_responseHelper.Success("Likes removed from discussion successfully"));
			} catch (SqlException ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong in sql execution", 500, ex.Message));
			} catch (Exception ex)
			{
				return StatusCode(500, _responseHelper.Error("Something went wrong", 500, ex.Message));
			}
		}

		[HttpPatch("{discussionId}/solved")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult AddSolvedToDiscussion(int discussionId)
		{
			try
			{
				if (!_discussionRepository.AddSolved(discussionId))
				{
					throw new Exception("Adding solved to discussion failed on save.");
				}
				return Ok(_responseHelper.Success("Solved added to discussion successfully"));
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

		[HttpDelete("{discussionId}/solved")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult RemoveSolvedFromDiscussion(int discussionId)
		{
			try
			{
				if (!_discussionRepository.RemoveSolved(discussionId))
				{
					throw new Exception("Removing solved from discussion failed on save.");
				}
				return Ok(_responseHelper.Success("Solved removed from discussion successfully"));
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
