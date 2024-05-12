using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IMemberHistoryRepsitory
    {
        IEnumerable<MemberHistory> GetMemberHistory(int memberId);
    }
}
