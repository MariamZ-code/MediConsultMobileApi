using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IGovernmentRepository
    {
        List<AppSelectorGovernment> GetGovernments();
    }
}
