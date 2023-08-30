using A_GroTech_Api.Dto;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface ICommodityTypeRepository
	{
		ICollection<CommodityType> GetCommodityTypes(PaginationDto paginationDto);
		CommodityType GetCommodityType(int id);
		bool AddCommodityType(CommodityType commodityType);
		bool UpdateCommodityType(CommodityType commodityType);
		bool DeleteCommodityType(int id);
		bool Save();
	}
}
