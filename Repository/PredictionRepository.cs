using A_GroTech_Api.Data;
using A_GroTech_Api.Interfaces;
using A_GroTech_Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace A_GroTech_Api.Repository
{
	public class PredictionRepository : IPredictionRepository
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;

		public PredictionRepository(DataContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}
        public bool AddPrediction(Prediction prediction)
		{
			_context.Add(prediction);
			return Save();
		}

		public bool DeletePrediction(int id)
		{
			var prediction = _context.Predictions.Find(id);
			_context.Remove(prediction);
			return Save();
		}

		public Prediction GetPrediction(int id)
		{
			var prediction = _context.Predictions.Include(p => p.Commodity)
				.Include(p => p.Area)
				.Where(p => p.Id == id)
				.FirstOrDefault();
			return _mapper.Map<Prediction>(prediction);
		}

		public ICollection<Prediction> GetPredictions()
		{
			var predictions = _context.Predictions
				.Include(p => p.Commodity)
				.Include(p => p.Area)
				.ToList();
			return _mapper.Map<ICollection<Prediction>>(predictions);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdatePrediction(Prediction prediction)
		{
			_context.Update(prediction);
			return Save();
		}
	}
}
