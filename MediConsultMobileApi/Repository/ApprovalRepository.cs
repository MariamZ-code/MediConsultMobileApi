using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ApprovalRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        #region GetAll
        public async Task<List<Approval>> GetAll(int memberId) => await dbContext.Approvals.Include(p=> p.Provider)
                                                                        .Where(a => a.member_id == memberId && a.is_chronic == 1 && a.is_repeated == 1)
                                                                        .AsNoTracking()
                                                                        .ToListAsync();

        #endregion

        #region ApprovalExists
        public bool ApprovalExists(int approvalId)
        {
            return dbContext.Approvals.Any(m => m.approval_id == approvalId);

        }
        #endregion

        #region GetByApprovalId
        public  Approval GetByApprovalId(int approvalId) =>  dbContext.Approvals
                                                                       .FirstOrDefault(a => a.approval_id == approvalId);
                                                                       
        #endregion

        #region Deleted
        public void Canceled(int approvalId)
        {
          var approval = GetByApprovalId(approvalId);
            approval.approval_status = "Canceled";
            approval.is_canceld = 1;

        }
        #endregion

        #region SaveChanges
        public void Save()
        {
            dbContext.SaveChanges();
        }
        #endregion
    }
}
