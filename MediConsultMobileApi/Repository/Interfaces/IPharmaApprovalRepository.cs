using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IPharmaApprovalRepository
    {
        List<PharmaApprovalAct> GetAllByApprovalId(int approvalId);
    }
}
