using A_GroTech_Api.Dto;
using A_GroTech_Api.Dto.BodyModels;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface INotificationRepository
	{
		ICollection<Notification> GetNotifications(PaginationDto paginationDto);
		Notification GetNotification(int id);
		bool AddNotification(Notification notification);
		bool UpdateNotification(Notification notification);
		bool DeleteNotification(int id);
		ICollection<Image> GetNotificationImages(int notificationId);
		bool AddNotificationImage(int notificationId, List<ImagePostDto> image);
		bool RemoveNotificationImage(int notificationId, int ImageId);
		bool Save();
	}
}
