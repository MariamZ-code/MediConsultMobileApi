using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IProviderRatingRepository
    {
        List<ProviderRating> GetRatingByMemberId(int memberId);
        ProviderRating GetRatingById(int rateId);
        void AddRate(ProviderRating rate);
        void UpdateRate(ProviderRating rate);
        void DeleteRate(int rateId);
        void Save();

        bool RateExists(int rateId);
    }
}
