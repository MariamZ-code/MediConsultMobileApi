using MediConsultMobileApi.DTO;
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
        public async Task<List<Approval>> GetAll(int memberId) => await dbContext.Approvals.Include(p => p.Provider)
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
        public Approval GetByApprovalId(int approvalId) => dbContext.Approvals
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

        public void AddApproval(int memberId, Approval approval)
        {
            dbContext.Approvals.Add(approval);
            dbContext.SaveChanges();
        }

        #region SaveChanges
        public void Save()
        {
            dbContext.SaveChanges();
        }

        #endregion
        public Request GetById(int requestId)
        {
            return dbContext.Requests.Include(p => p.Provider).Include(a => a.Approval).FirstOrDefault(r => r.Approval_id == requestId && r.is_chronic==1);
        }

        #region EditRequest
        public void EditRequest(UpdateChronicApprovalDto requestDto, int requestId)
        {
            var request = GetById(requestId);
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            if (request is null)
            {
             

                var insertRequest = new Request
                {

                    Provider_id = 1,
                    Notes = requestDto.Notes,
                    Member_id = requestDto.Member_id,
                    is_chronic = 1,
                    Approval_id= requestId

                };
             

                dbContext.Add(insertRequest);

                dbContext.SaveChanges();
              
            }

            else
            {
                var folder2 = Path.Combine(serverPath, "MemberPortalApp", request.Member_id.ToString(), "Approvals", request.ID.ToString());

                request.Notes = requestDto.Notes;
                request.Member_id = requestDto.Member_id;
                request.Folder_path = folder2;

                dbContext.SaveChanges();
            }
        }

        #endregion

    }
}
