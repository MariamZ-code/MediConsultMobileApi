using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CityRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IQueryable<AppSelectorGovernmentCity> GetCity(int? govId)
        {
           return dbContext.SelectorGovernmentCities.Where(g=>g.government_id == govId).OrderBy(c=>c.city_name_en).AsNoTracking().AsQueryable();
        }
    }
}
