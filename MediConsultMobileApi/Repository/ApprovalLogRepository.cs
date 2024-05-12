using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

namespace MediConsultMobileApi.Repository
{
    public class ApprovalLogRepository : IApprovalLogRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ApprovalLogRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(ApprovalLogDto appLog)
        {
            var newApproval = new ApprovalLog()
            {
                claim_id = appLog.claim_id,
                _event = appLog._event,
                user_id = appLog.user_id,
            };

            dbContext.ApprovalLogs.Add(newApproval);
        }


    }
}
