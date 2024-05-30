using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ILabAndScanCenterRepository
    {
        IQueryable<UniqueLabAndScanServiceDto> GetLabAndScanUnique();
    }
}
