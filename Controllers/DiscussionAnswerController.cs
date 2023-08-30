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
	public class DiscussionAnswerController : Controller
	{
		private readonly ResponseHelper _responseHelper;
		private readonly IDiscussionAnswerRepository _discussionAnswerRepository;
		private readonly IMapper _mapper;
		private readonly IDiscussionRepository _discussionRepository;
		private readonly IUserRepository _userRepository;

		public DiscussionAnswerController(ResponseHelper responseHelper,
			IDiscussionAnswerRepository discussionAnswerRepository,
			IMapper mapper,
			IDiscussionRepository discussionRepository,
			IUserRepository userRepository)
		{
			_responseHelper = responseHelper;
			_discussionAnswerRepository = discussionAnswerRepository;
			_mapper = mapper;
			_discussionRepository = discussionRepository;
			_userRepository = userRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<DiscussionAnswerDto>))]
		public IActionResult GetDiscussionAnswers([FromQuery] PaginationDto paginationDto)
		{
			try
			{
				var discussionAnswers = _mapper.Map<List<DiscussionAnswerDto>>(_discussionAnswerRepository.GetDiscussionAnswers(paginationDto));
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

		[HttpPost]
		[ProducesResponseType(204)]
		public IActionResult AddDiscussionAnswer([Required][FromQuery] int dicussionId, [FromBody] DiscussionAnswerPostDto discussionAnswerPostDto)
		{
			try
			{
				var discussionAnswer = _mapper.Map<DiscussionAnswer>(discussionAnswerPostDto);
				var discussion = _discussionRepository.GetDiscussion(dicussionId);
				discussionAnswer.Discussion = discussion;
				discussionAnswer.AnsweredBy = _userRepository.GetUser(discussionAnswerPostDto.CreatedById);
				discussion.CreatedAt = DateTime.Now;
				discussion.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_discussionAnswerRepository.AddDiscussionAnswer(discussionAnswer))
					throw new Exception("Something went wrong in adding discussion answer");

				return Ok(_responseHelper.Success("Discussion answer added successfuly"));
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

		[HttpGet("{discussionAnswerId}")]
		[ProducesResponseType(200, Type = typeof(DiscussionAnswerDto))]
		public IActionResult GetDiscussionAnswer(int discussionAnswerId)
		{
			try
			{
				var discussionAnswer = _mapper.Map<DiscussionAnswerDto>(_discussionAnswerRepository.GetDiscussionAnswer(discussionAnswerId));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (discussionAnswer == null)
					return NotFound(_responseHelper.Error("No discussion answer found", 404));
				return Ok(_responseHelper.Success("", discussionAnswer));
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

		[HttpPut("{discussionAnswerId}")]
		[ProducesResponseType(204)]
		public IActionResult UpdateDiscussionAnswer(int discussionAnswerId, [FromBody] DiscussionAnswerPutDto discussionAnswerPutDto)
		{
			try
			{
				var discussionAnswer = _discussionAnswerRepository.GetDiscussionAnswer(discussionAnswerId);
				if (discussionAnswer == null)
					return NotFound(_responseHelper.Error("No discussion answer found", 404));

				discussionAnswer = _mapper.Map(discussionAnswerPutDto, discussionAnswer);
				discussionAnswer.UpdatedAt = DateTime.Now;
				if (!_discussionAnswerRepository.UpdateDiscussionAnswer(discussionAnswer))
					throw new Exception("Something went wrong in updating discussion answer");

				return Ok(_responseHelper.Success("Discussion answer updated successfuly"));
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

		[HttpDelete("{discussionAnswerId}")]
		[ProducesResponseType(204)]
		public IActionResult DeleteDiscussionAnswer(int discussionAnswerId)
		{
			try
			{
				if (!_discussionAnswerRepository.DeleteDiscussionAnswer(discussionAnswerId))
					throw new Exception("Something went wrong in deleting discussion answer");

				return Ok(_responseHelper.Success("Discussion answer deleted successfuly"));
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

		[HttpGet("{discussionAnswerId}/images")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ImageDto>))]
		public IActionResult GetDiscussionAnswerImages(int discussionAnswerId)
		{
			try
			{
				var images = _mapper.Map<List<ImageDto>>(_discussionAnswerRepository.GetDiscussionAnswerImages(discussionAnswerId));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));
				if (images == null)
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

		[HttpPost("{discussionAnswerId}/images")]
		[ProducesResponseType(204)]
		public IActionResult AddDiscussionAnswerImage(int discussionAnswerId, [FromBody] List<ImagePostDto> imagePostDto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_discussionAnswerRepository.AddDiscussionAnswerImage(discussionAnswerId, imagePostDto))
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

		[HttpDelete("{discussionAnswerId}/images")]
		[ProducesResponseType(204)]
		public IActionResult DeleteDiscussionAnswerImage(int discussionAnswerId, [Required][FromQuery] int imageId)
		{
			try
			{
				if (!_discussionAnswerRepository.RemoveDiscussionAnswerImage(discussionAnswerId, imageId))
					throw new Exception("Something went wrong in deleting discussion answer image");

				return Ok(_responseHelper.Success("Discussion answer image deleted successfuly"));
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
