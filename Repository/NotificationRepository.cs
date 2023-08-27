using A_GroTech_Api.Data;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public NotificationRepository(DataContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}
        public bool AddNotification(Notification notification)
		{
			_context.Add(notification);
			return Save();
		}

		public bool AddNotificationImage(int notificationId, List<ImagePostDto> image)
		{
			var notification = _context.Notifications.Where(d => d.Id == notificationId).FirstOrDefault();
			foreach (var item in image)
			{
				var imageMap = _mapper.Map<Image>(item);
				imageMap.CreatedAt = DateTime.Now;
				imageMap.UpdatedAt = DateTime.Now;
				var notificationImage = new NotificationImage
				{
					Notification = notification,
					Image = imageMap
				};
				_context.Add(notificationImage);
			}
			return Save();
		}

		public bool DeleteNotification(int id)
		{
			var notification = _context.Notifications.Find(id);
			_context.Remove(notification);
			return Save();
		}

		public Notification GetNotification(int id)
		{
			var notification = _context.Notifications
				.Where(n => n.Id == id)
				.Include(n => n.User)
				.FirstOrDefault();
			return _mapper.Map<Notification>(notification);
		}

		public ICollection<Image> GetNotificationImages(int notificationId)
		{
			var images = _context.NotificationImages
				.Where(di => di.Notification.Id == notificationId)
				.Select(di => di.Image)
				.ToList();
			return _mapper.Map<ICollection<Image>>(images);
		}

		public ICollection<Notification> GetNotifications()
		{
			var notifications = _context.Notifications
				.Include(n => n.User)
				.ToList();
			return _mapper.Map<ICollection<Notification>>(notifications);
		}

		public bool RemoveNotificationImage(int notificationId, int ImageId)
		{
			var image = _context.Images.Where(i => i.Id == ImageId).FirstOrDefault();
			var notificationImage = _context.NotificationImages.Where(di => di.Image.Id == image.Id && di.Notification.Id == notificationId).FirstOrDefault();
			_context.Remove(notificationImage);
			_context.Remove(image);
			return Save();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateNotification(Notification notification)
		{
			_context.Update(notification);
			return Save();
		}
	}
}
