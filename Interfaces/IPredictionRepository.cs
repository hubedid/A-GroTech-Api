using A_GroTech_Api.Models;

namespace A_GroTech_Api.Interfaces
{
	public interface IPredictionRepository
	{
		ICollection<Prediction> GetPredictions();
		Prediction GetPrediction(int id);
		bool AddPrediction(Prediction prediction);
		bool UpdatePrediction(Prediction prediction);
		bool DeletePrediction(int id);
		bool Save();
	}
}
