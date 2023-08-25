using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IImageRepository
	{
		Image GetImage(int id);
		bool UpdateImage(Image image);
		bool Save();
    }
}
