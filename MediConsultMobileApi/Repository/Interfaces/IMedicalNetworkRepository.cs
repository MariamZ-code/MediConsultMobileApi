using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IMedicalNetworkRepository
    {
        IQueryable<MedicalNetwork> GetAll();
    }
}
