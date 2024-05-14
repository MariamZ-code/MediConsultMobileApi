using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IApprovalTimelineRepository
    {
        void InsertApprovalTimeLine(ApprovalTimeline timeline);
    }
}
