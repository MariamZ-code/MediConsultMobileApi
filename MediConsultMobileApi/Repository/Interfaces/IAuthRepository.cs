using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IAuthRepository
    {
        void Registeration(RegisterUserDto userDto , int memberId);
        Task<MessageDto> Login(LoginUserDto userDto, string lang);

        bool MemberLoginExists(int? memberId);

         ClientBranchMember GetById(int memberId);

      

        void SendOtp(string otp, int memberId);

        void ChangePass(string otp, int id, ChangePasswordDTO changeDto);

        void Save();

        void UpdateOtp(ClientBranchMember member);
    }
}
