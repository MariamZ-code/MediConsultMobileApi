using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface ITokenRepository
    {
        void SaveToken(FirebaseTokenDTO tokenDto);
        void SaveChanges();


    }
}
