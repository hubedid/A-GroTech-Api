using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IAreaRepository
	{
		ICollection<Area> GetAreas();
		Area GetArea(int id);
	}
}
