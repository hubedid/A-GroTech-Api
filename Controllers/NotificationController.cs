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
	public class NotificationController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ResponseHelper _responseHelper;
		private readonly INotificationRepository _notificationRepository;
		private readonly IUserRepository _userRepository;

		public NotificationController(IMapper mapper,
            ResponseHelper responseHelper,
            INotificationRepository notificationRepository,
			IUserRepository userRepository)
        {
			_mapper = mapper;
			_responseHelper = responseHelper;
			_notificationRepository = notificationRepository;
			_userRepository = userRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<NotificationDto>))]
		public IActionResult GetNotifications()
		{
			try
			{
				var notifications = _mapper.Map<List<NotificationDto>>(_notificationRepository.GetNotifications());
				if(!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if(notifications.Any() != true)
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

		[HttpGet("{notificationId}")]
		[ProducesResponseType(200, Type = typeof(NotificationDto))]
		public IActionResult GetNotification(int notificationId)
		{
			try
			{
				var notification = _mapper.Map<NotificationDto>(_notificationRepository.GetNotification(notificationId));
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (notification == null)
					return NotFound(_responseHelper.Error("No notification found", 404));

				return Ok(_responseHelper.Success("", notification));
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
		public IActionResult CreateNotification([FromBody] NotificationPostDto notificationPostDto)
		{
			try
			{
				var notification = _mapper.Map<Notification>(notificationPostDto);
				var user = _userRepository.GetUser(notificationPostDto.UserId);
				notification.User = user;
				notification.CreatedAt = DateTime.Now;
				notification.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if(!_notificationRepository.AddNotification(notification))
					throw new Exception("Something went wrong in sql execution");

				return Ok(_responseHelper.Success("Notification created successfully"));
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

		[HttpPut("{notificationId}")]
		[ProducesResponseType(204)]
		public IActionResult UpdateNotification(int notificationId, [FromBody] NotificationPutDto notificationPutDto)
		{
			try
			{
				var notification = _notificationRepository.GetNotification(notificationId);
				if (notification == null)
					return NotFound(_responseHelper.Error("No notification found", 404));

				notification = _mapper.Map(notificationPutDto, notification);
				notification.UpdatedAt = DateTime.Now;

				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_notificationRepository.UpdateNotification(notification))
					throw new Exception("Something went wrong in sql execution");

				return Ok(_responseHelper.Success("Notification updated successfully"));
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

		[HttpDelete("{notificationId}")]
		[ProducesResponseType(204)]
		public IActionResult DeleteNotification(int notificationId)
		{
			try
			{
				if(!_notificationRepository.DeleteNotification(notificationId))
					throw new Exception("Something went wrong in sql execution");

				return Ok(_responseHelper.Success("Notification deleted successfully"));
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

		[HttpGet("{notificationId}/images")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<ImageDto>))]
		public IActionResult GetDiscussionAnswerImages(int notificationId)
		{
			try
			{
				var images = _mapper.Map<List<ImageDto>>(_notificationRepository.GetNotificationImages(notificationId));
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

		[HttpPost("{notificationId}/images")]
		[ProducesResponseType(204)]
		public IActionResult AddDiscussionAnswerImage(int notificationId, [FromBody] List<ImagePostDto> imagePostDto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(_responseHelper.Error(ModelState.Select(ex => ex.Value?.Errors).FirstOrDefault()?.Select(e => e.ErrorMessage).FirstOrDefault()?.ToString()));

				if (!_notificationRepository.AddNotificationImage(notificationId, imagePostDto))
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

		[HttpDelete("{notificationId}/images")]
		[ProducesResponseType(204)]
		public IActionResult DeleteDiscussionAnswerImage(int notificationId, [Required][FromQuery] int imageId)
		{
			try
			{
				if (!_notificationRepository.RemoveNotificationImage(notificationId, imageId))
					throw new Exception("Something went wrong in deleting Notification image");

				return Ok(_responseHelper.Success("Notification image deleted successfuly"));
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
