using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IServiceRepository
    {

        Task<List<Member_services_with_copayments>> GetById(Member member);
        
    }
}
