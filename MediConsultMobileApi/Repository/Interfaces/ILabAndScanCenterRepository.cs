using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ILabAndScanCenterRepository
    {
        IQueryable<GetServicesLabAndScan> GetLabAndScanUnique();

        IQueryable<LabAndScanCenter> GetLabAndScanCenters(List<int> serviceIds);
    }
}
