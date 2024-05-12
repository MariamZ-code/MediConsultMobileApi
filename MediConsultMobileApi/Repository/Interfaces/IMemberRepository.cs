using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IMemberRepository
    {
        (string date, string gender) CreateDateAndGender(string ssn);
        bool MemberExists(int? memberId);
        bool SSNExists(string nId);
        bool PhoneExists(string mobile);
        ClientBranchMember GetMemberByMobile(string mobile);
        ClientBranchMember GetMemberByEmail(string email);
        ClientBranchMember GetMemberByNationalId(string nId);
        Task<MessageDto> validation(Member member);
        Task<Member> GetByID(int? id);

        Task<List<ClientBranchMember>> MemberFamily(int id);

        ClientBranchMember MemberDetails(int memberId);

        void UpdateMember(UpdateMemberDTO memberDTO, int id);
        bool IsValidDate(string date);
        void SaveDatabase();

    }
}
