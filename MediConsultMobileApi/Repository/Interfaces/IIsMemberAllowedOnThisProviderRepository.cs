namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IIsMemberAllowedOnThisProviderRepository
    {
        Task<int> IsMemberAllowedOnThisProvider(int? memberId, int? providerId);
    }
}
