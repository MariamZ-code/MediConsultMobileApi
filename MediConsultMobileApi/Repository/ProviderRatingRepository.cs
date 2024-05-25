using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

namespace MediConsultMobileApi.Repository
{
    public class ProviderRatingRepository : IProviderRatingRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProviderRatingRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddRate(ProviderRating rate)
        {
            dbContext.ProviderRatings.Add(rate);
        }
        public void Save()=> dbContext.SaveChanges();
    }
}
