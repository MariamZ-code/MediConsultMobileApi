using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ICityRepository
    {
        IQueryable<AppSelectorGovernmentCity> GetCity(int? govId);
    }
}
