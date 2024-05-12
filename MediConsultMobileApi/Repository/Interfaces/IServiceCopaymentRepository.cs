using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IServiceCopaymentRepository
    {
        List<ServiceCopaymentproviderView> GetServices(int memberId);
    }
}
