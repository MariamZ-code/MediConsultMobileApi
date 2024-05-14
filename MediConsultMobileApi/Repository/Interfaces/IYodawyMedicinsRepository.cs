using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IYodawyMedicinsRepository
    {
        Task<List<YodawyMedicins>> GetAll();
        Task<YodawyMedicins> GetAllById(int medId);
    }
}
