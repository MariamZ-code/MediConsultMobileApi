using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IMemberProgramRepository
    {
        MemberProgram GetMemberbyMemberId(int memberId);
    }
}
