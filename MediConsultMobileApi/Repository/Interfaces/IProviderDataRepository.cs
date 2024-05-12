using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IProviderDataRepository
    {
        IQueryable<ProviderData> GetProviders();
        ProviderData GetProvider(int? id);
        Task<bool> ProviderExistsAsync(int? providerId);

    }
}
