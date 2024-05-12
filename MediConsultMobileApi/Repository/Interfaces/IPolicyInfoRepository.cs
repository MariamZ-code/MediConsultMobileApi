using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IPolicyInfoRepository
    {
        ProgramTypeInfo GetInfo(int serviceId);
    }
}
