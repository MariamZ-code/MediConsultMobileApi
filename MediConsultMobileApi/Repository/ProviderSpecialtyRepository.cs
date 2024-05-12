using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class ProviderSpecialtyRepository : IProviderSpecialtyRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProviderSpecialtyRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<ProviderSpecialty> GetProviderSpecialties()
        {
            return dbContext.ProviderSpecialties
                .Include(g=>g.GeneralSpecialty)
                .Include(s => s.subGeneralSpecialty)
                .AsNoTracking().AsQueryable();
        }
        public ProviderSpecialty GetProviderSpecialtiesByProviderId(int? providerId)
        {
            return dbContext.ProviderSpecialties
                .Include(g => g.GeneralSpecialty)
                .Include(s => s.subGeneralSpecialty)
                .AsNoTracking().FirstOrDefault(p=>p.provider_id==providerId);
        }
    }
}
