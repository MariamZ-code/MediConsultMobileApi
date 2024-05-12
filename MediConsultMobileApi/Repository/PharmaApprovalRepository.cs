using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class PharmaApprovalRepository : IPharmaApprovalRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PharmaApprovalRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<PharmaApprovalAct> GetAllByApprovalId(int approvalId)
        {
            var actStatusReasons = dbContext.PharmaApprovalActs
                   .Where(r=> r.Act_Status_Reason==-1)
                    .AsNoTracking()
                    .ToList();
            foreach (var item in actStatusReasons)
            {
                 item.Act_Status_Reason = null ;
            }
            return  dbContext.PharmaApprovalActs
                    .Include(y => y.YodawyMedicins)
                    .Include(r => r.AppActReasons)
                    .Where(a => a.Act_Approval_id == approvalId)
                    .AsNoTracking()
                    .ToList();
        }
    }
}
