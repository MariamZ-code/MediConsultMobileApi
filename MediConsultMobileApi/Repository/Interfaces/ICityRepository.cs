using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ICityRepository
    {
        List<AppSelectorGovernmentCity> GetCity(int? govId);
    }
}
