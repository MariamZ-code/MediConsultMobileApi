using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class ProviderRatingRepository : IProviderRatingRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProviderRatingRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<ProviderRating> GetRatingByMemberId(int memberId)
        {
            return dbContext.ProviderRatings.Include(p=>p.ProviderData).Where(m=>m.member_id == memberId).ToList();
        }
        public ProviderRating GetRatingById(int rateId)
        {
            return dbContext.ProviderRatings.FirstOrDefault(r => r.rate_id == rateId);
        }
        public void AddRate(ProviderRating rate)
        {
            dbContext.ProviderRatings.Add(rate);
        }
        public void UpdateRate(ProviderRating rate)
        {
            dbContext.ProviderRatings.Update(rate);
        }

        public void DeleteRate(int rateId)
        {
            var rate= GetRatingById(rateId);
            dbContext.ProviderRatings.Remove(rate);
        }
        public void Save()=> dbContext.SaveChanges();

        public bool RateExists(int rateId)
        {
            return dbContext.ProviderRatings.Any(r => r.rate_id == rateId);
        }
    }
}
