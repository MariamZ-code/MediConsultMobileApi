using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class RefundRepository : IRefundRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RefundRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Refund AddRefund(RefundDTO refunddto)
        {
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            var refund = new Refund
            {

                refund_type_id = refunddto.refund_type_id,
                notes = refunddto.notes,
                member_id = refunddto.member_id,
                refund_date = refunddto.refund_date,
                total_amount = refunddto.amount,
                refund_reason = refunddto.refund_reason,
            };
            dbContext.Add(refund);

            dbContext.SaveChanges();

            var folder = Path.Combine(serverPath, "MemberPortalApp", refunddto.member_id.ToString(), "Refund", refund.id.ToString());


            refund.folder_path = folder;

            return refund;


        }

        #region RefundByMemberId
        public IQueryable<Refund> GetRefundByMemberId(int memberId)
        {

            var refund = dbContext.Refunds.Include(c => c.refundTypes).Where(r => r.member_id == memberId).AsNoTracking().AsQueryable();

            return refund;


        }

        #endregion


        #region RefundByRefundId
        public Refund GetById(int RefundId)
        {
            return dbContext.Refunds.Include(c => c.refundTypes).FirstOrDefault(r => r.id == RefundId);
        }
        #endregion

        #region RefundExists
        public bool RefundExists(int? refundId)
        {
            return dbContext.Refunds.Any(p => p.id == refundId);
        }

        #endregion

        #region RefundTypeExists
        public bool RefundTypeExists(int? refundTypeId)
        {
            return dbContext.RefundTypes.Any(p => p.id == refundTypeId);
        }

        #endregion

        #region EditRefund
        public void EditRefund(UpdateRefundDTO refundDto, int refundId)
        {

            var refund = GetById(refundId);
            refund.notes = refundDto.notes;
            refund.refund_type_id = refundDto.refund_type_id;
            //refund.Status = "Reviewing";
            refund.total_amount = refundDto.amount;
            refund.refund_date = refundDto.refund_date;
            refund.refund_reason = refundDto.refund_reason;

            if (refund.Status == "OnHold")
            {
                refund.Status = "Reviewing";

            }
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            var folder = Path.Combine(serverPath, "MemberPortalApp", refund.member_id.ToString(), "Refund", refundId.ToString());

            refund.folder_path = folder;

          
        }

        #endregion

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
