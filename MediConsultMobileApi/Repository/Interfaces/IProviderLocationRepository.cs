using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IProviderLocationRepository
    {
        IQueryable<ProviderLocation> GetProviderLocations();
        ProviderLocation GetProviderLocationsByLocationId(int? locationId);
        ProviderLocation GetProviderLocationsByProviderId(int providerId ,int locationId);
        IQueryable<ProviderLocation> GetProviderLocationsLabAndScan(int categoryId);
        bool ProviderLocationExists(int? locationId);
    }
}
