using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

namespace MediConsultMobileApi.Repository
{
    public class ApprovalTimelineRepository : IApprovalTimelineRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ApprovalTimelineRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void InsertApprovalTimeLine(ApprovalTimeline timeline) => dbContext.ApprovalTimelines.Add(timeline);
    }
}
