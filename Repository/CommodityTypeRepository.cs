using A_GroTech_Api.Data;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Repository
{
	public class CommodityTypeRepository : ICommodityTypeRepository
	{
		private readonly DataContext _context;

		public CommodityTypeRepository(DataContext context)
        {
			_context = context;
		}
        public bool AddCommodityType(CommodityType commodityType)
		{
			_context.Add(commodityType);
			return Save();
		}

		public bool DeleteCommodityType(int id)
		{
			var commodityType = _context.CommodityTypes.Find(id);
			_context.Remove(commodityType);
			return Save();
		}

		public CommodityType GetCommodityType(int id)
		{
			var commodityType = _context.CommodityTypes.Find(id);
			return commodityType;
		}

		public ICollection<CommodityType> GetCommodityTypes()
		{
			var commodityTypes = _context.CommodityTypes.ToList();
			return commodityTypes;
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateCommodityType(CommodityType commodityType)
		{
			_context.Update(commodityType);
			return Save();
		}
	}
}
