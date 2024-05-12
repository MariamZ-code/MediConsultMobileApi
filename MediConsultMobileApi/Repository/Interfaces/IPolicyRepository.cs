using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IPolicyRepository
    {
        Task<List<Policy>> GetByProgramId(int progId);
        bool ServiceExists(int serviceId);
    }
}
