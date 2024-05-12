using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IRefundRepository
    {
        Refund AddRefund(RefundDTO refundDto);
        Refund GetById(int RefundId);
        void Save();
        IQueryable<Refund> GetRefundByMemberId(int memberId);
        void EditRefund(UpdateRefundDTO refundDto, int refundId);
        bool RefundExists(int? refundId);
        bool RefundTypeExists(int? refundTypeId);
    }
}
