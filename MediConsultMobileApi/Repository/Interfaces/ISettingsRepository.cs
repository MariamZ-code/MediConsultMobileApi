using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ISettingsRepository
    {
        MemberNationalId GetByMemberId(int memberId);
        void EditNationalID(ChangeNationalIdDTO NationalIdDto);
        void Save();
    }
}
