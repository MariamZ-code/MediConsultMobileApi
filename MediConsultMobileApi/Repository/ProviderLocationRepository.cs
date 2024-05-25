using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class ProviderLocationRepository : IProviderLocationRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProviderLocationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<ProviderLocation> GetProviderLocations()
        {
            return dbContext.providerLocations.Include(c => c.AppSelectorGovernmentCity)
                                              .ThenInclude(g => g.appSelectorGovernment)
                                              .Include(p => p.Provider).ThenInclude(c=>c.Category)
                                              .Where(c => c.Provider.provider_status == "Activated")
                                              .AsNoTracking().AsQueryable();
        }
        public ProviderLocation GetProviderLocationsByProviderId(int providerId, int locationId)
        {
            return dbContext.providerLocations.FirstOrDefault(p => p.provider_id == providerId && p.location_id == locationId);
        }
        public ProviderLocation GetProviderLocationsByLocationId(int? locationId)
        {
            return dbContext.providerLocations.Include(c => c.AppSelectorGovernmentCity)
                                              .ThenInclude(g => g.appSelectorGovernment)
                                              .Include(p => p.Provider).ThenInclude(c => c.Category)
                                              .FirstOrDefault(l=>l.location_id==locationId);
        }

        public bool ProviderLocationExists(int? locationId)
        {
            return dbContext.providerLocations.Any(r => r.location_id == locationId);
        }
    }
}
