using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IProviderRatingRepository
    {
        void AddRate(ProviderRating rate);
        void Save();
    }
}
