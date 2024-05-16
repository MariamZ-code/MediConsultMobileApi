using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IYodawyMedicinsRepository
    {
        Task<List<YodawyMedicins>> GetAll();
   
        Task<YodawyMedicins> GetById(int medId);
        Task<bool> MedicinsExists(int medId);
    }
}
