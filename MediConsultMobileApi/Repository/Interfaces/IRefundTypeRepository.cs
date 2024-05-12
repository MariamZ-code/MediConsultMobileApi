using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IRefundTypeRepository
    {
        Task<List<RefundType>> GetAllRefundType();
    
    }
}
 