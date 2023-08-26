using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IAreaRepository
	{
		ICollection<Area> GetAreas();
		Area GetArea(int id);
		bool AddArea(Area area);
		bool UpdateArea(Area area);
		bool DeleteArea(int id);
		bool Save();
	}
}
