using A_GroTech_Api.Data;
using A_GroTech_Api.Dto;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class CommodityRepository : ICommodityRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public CommodityRepository(DataContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}
        public bool AddCommodity(Commodity commodity)
		{
			_context.Add(commodity);
			return Save();
		}

		public bool AddCommodityArea(int commodityId, Area area)
		{
			var commodity = _context.Commodities.Find(commodityId);
			var commodityArea = new CommodityArea
			{
				Commodity = commodity, 
				Area = area 
			};
			_context.Add(commodityArea);
			return Save();
		}

		public bool AddCommodityAreaById(int commodityId, int areaId)
		{
			var commodity = _context.Commodities.Find(commodityId);
			var area = _context.Areas.Find(areaId);
			var commodityArea = new CommodityArea 
			{ 
				Commodity = commodity, 
				Area = area 
			};
			_context.Add(commodityArea);
			return Save();
		}

		public bool DeleteCommodity(int id)
		{
			var commodity = _context.Commodities.Find(id);
			_context.Remove(commodity);
			return Save();
		}

		public ICollection<Commodity> GetCommodities(PaginationDto paginationDto)
		{
			return _context.Commodities
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.ToList();
		}

		public Commodity GetCommodity(int id)
		{
			return _context.Commodities.Find(id);
		}

		public ICollection<Area> GetCommodityAreas(int commodityId, PaginationDto paginationDto)
		{
			var commdityArea = _context.CommodityAreas
				.Where(ca => ca.CommodityId == commodityId)
				.Include(ca => ca.Area)
				.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
				.Take(paginationDto.PageSize)
				.Select(ca => ca.Area)
				.ToList();
			return _mapper.Map<ICollection<Area>>(commdityArea);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateCommodity(Commodity commodity)
		{
			_context.Update(commodity);
			return Save();
		}
	}
}
