using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IProviderSpecialtyRepository
    {
        IQueryable<ProviderSpecialty> GetProviderSpecialties();
        ProviderSpecialty GetProviderSpecialtiesByProviderId(int? providerId);
        List<ProviderSpecialtyCountDTO> GetCountOfProviders();
    }
}
