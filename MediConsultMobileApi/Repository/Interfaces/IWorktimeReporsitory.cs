using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IWorktimeReporsitory
    {
        List<ProviderWorktime> GetWorktimes(int providerId);
    }
}
