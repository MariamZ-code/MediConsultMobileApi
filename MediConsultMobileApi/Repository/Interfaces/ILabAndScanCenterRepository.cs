using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ILabAndScanCenterRepository
    {
        IQueryable<GetServicesLabAndScan> GetLabAndScanUnique(int categoryId);

        IQueryable<LabAndScanCenter> GetLabAndScanCenters(List<int> serviceIds);
        LabAndScanCenter GetLabAndScanServiceName(int serviceId);
    }
}
