using A_GroTech_Api.Dto;
using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IPredictionRepository
	{
		ICollection<Prediction> GetPredictions(PaginationDto paginationDto);
		Prediction GetPrediction(int id);
		bool AddPrediction(Prediction prediction);
		bool UpdatePrediction(Prediction prediction);
		bool DeletePrediction(int id);
		bool Save();
	}
}
