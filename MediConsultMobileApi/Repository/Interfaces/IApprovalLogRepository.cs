using MediConsultMobileApi.DTO;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IApprovalLogRepository
    {
        void Add(ApprovalLogDto appLog);
    }
}
