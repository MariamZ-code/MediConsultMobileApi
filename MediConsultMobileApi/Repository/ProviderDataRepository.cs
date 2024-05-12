using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class ProviderDataRepository : IProviderDataRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProviderDataRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public  IQueryable<ProviderData> GetProviders()
        {
            return  dbContext.Providers.Where(p=>p.provider_status== "Activated").AsNoTracking().AsQueryable();
        }

        public ProviderData GetProvider(int? id) => dbContext.Providers.SingleOrDefault(p=> p.provider_id== id);
        public async Task<bool> ProviderExistsAsync(int? providerId)
        {
            return await dbContext.Providers.AnyAsync(p => p.provider_id == providerId);
        }
    }
}
