using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IGovernmentRepository
    {
        IQueryable<AppSelectorGovernment> GetGovernments();
        bool GovernmentExsists (int governmentId);
    }
}
