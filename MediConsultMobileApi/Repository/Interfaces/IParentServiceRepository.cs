using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IParentServiceRepository
    {

        List<Serviceview> Get(int serviceId);

    }
}
