using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

namespace MediConsultMobileApi.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CityRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public List<AppSelectorGovernmentCity> GetCity(int? govId)
        {
           return dbContext.SelectorGovernmentCities.Where(g=>g.government_id == govId).ToList();
        }
    }
}
