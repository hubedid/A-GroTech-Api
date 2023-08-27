using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface ICommodityRepository
	{
		ICollection<Commodity> GetCommodities();
		Commodity GetCommodity(int id);
		ICollection<Area> GetCommodityAreas(int commodityId);
		bool AddCommodityAreaById(int commodityId, int areaId);
		bool AddCommodityArea(int commodityId, Area area);
		bool AddCommodity(Commodity commodity);
		bool UpdateCommodity(Commodity commodity);
		bool DeleteCommodity(int id);
		bool Save();
	}
}
