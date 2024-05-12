using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IRequestRepository
    {
        Request GetById(int RequestId);

        IQueryable<Request> GetRequestsByMemberId(int memberId) ;
        Request AddRequest(RequestDTO requestDto);

        bool RequestExists(int requestId);
        void EditRequest(UpdateRequestDTO requestDto, int requestId);
        void Save();
    }
}
