using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IMemberECardInfoRepository
    {
        List<MemberECardInfo> GetInfos(int? programId);
    }
}
