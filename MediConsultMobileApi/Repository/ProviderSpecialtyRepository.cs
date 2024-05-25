using iText.Commons.Actions.Contexts;
using MediConsultMobileApi.DTO;
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
                .Include(g => g.GeneralSpecialty)
                .Include(s => s.subGeneralSpecialty)
                .AsNoTracking().AsQueryable();
        }
        public ProviderSpecialty GetProviderSpecialtiesByProviderId(int? providerId)
        {
            return dbContext.ProviderSpecialties
                .Include(g => g.GeneralSpecialty)
                .Include(s => s.subGeneralSpecialty)
                .AsNoTracking().FirstOrDefault(p => p.provider_id == providerId);
        }

        public List<ProviderSpecialtyCountDTO> GetCountOfProviders() =>
     dbContext.ProviderSpecialties.Include(p => p.Provider)
            .Include(g => g.GeneralSpecialty)
            .Where(p => p.Provider.provider_status == "Activated")
         .GroupBy(ps=> new {ps.General_specialty_Id, ps.GeneralSpecialty.General_Specialty_Name_En})
         .Select(g => new ProviderSpecialtyCountDTO
         {
             GeneralSpecialtyId = g.Key.General_specialty_Id,
             ProviderCount = g.Count(),
             SpecialtyName = g.Key.General_Specialty_Name_En

         })
            
         .OrderByDescending(x => x.ProviderCount)
         .ToList();
    }
}




