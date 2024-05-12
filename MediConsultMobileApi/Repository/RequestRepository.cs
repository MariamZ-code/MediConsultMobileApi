using Google.Api.Gax.ResourceNames;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MediConsultMobileApi.Repository
{

    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RequestRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #region AddNewRequest
        public Request AddRequest(RequestDTO requestDto)
        {
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            var request = new Request
            {

                Provider_id = requestDto.Provider_id,
                Notes = requestDto.Notes,
                Member_id = requestDto.Member_id,

            };
            var provider = dbContext.Providers.FirstOrDefault(p => p.provider_id == request.Provider_id);

            if (provider != null)
            {
               
                if (provider.Category_ID == 1)
                {
                    request.Is_pharma = 1;
                }
            }
          
            dbContext.Add(request);

            dbContext.SaveChanges();
            var folder = Path.Combine(serverPath, "MemberPortalApp", request.Member_id.ToString(), "Approvals", request.ID.ToString());

            request.Folder_path = folder;

            dbContext.SaveChanges();

            return request;

        }

        #endregion

        #region EditRequest
        public  void EditRequest(UpdateRequestDTO requestDto, int requestId)
        {

            var request =  GetById(requestId);
            request.Notes= requestDto.Notes;
            request.Provider_id = requestDto.Provider_id;
            request.Member_id = requestDto.Member_id;
            if(request.Status == "OnHold")
            {
                request.Status = "Received";
                
            }
            var provider = dbContext.Providers.FirstOrDefault(p => p.provider_id == request.Provider_id);

            if (provider != null)
            {

                if (provider.Category_ID == 1)
                {
                    request.Is_pharma = 1;
                }
            }
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            var folder = Path.Combine(serverPath, "MemberPortalApp", request.Member_id.ToString(), "Approvals", requestId.ToString());

             request.Folder_path = folder;

             //dbContext.Requests.Update(request);
             dbContext.SaveChanges();
        }

        #endregion


        #region RequestExists
        public bool RequestExists(int requestId)
        {
            return dbContext.Requests.Any(r => r.ID == requestId);

        }
        #endregion
        #region RequestByMemberId
        public IQueryable<Request> GetRequestsByMemberId(int memberId)
        {

            var requests = dbContext.Requests.Include(p => p.Provider).Where(r => r.Member_id == memberId).AsNoTracking().AsQueryable();

            return requests;


        }

        #endregion


        #region RequestByRequestId
        public  Request GetById(int RequestId)
        {
            return  dbContext.Requests.Include(p => p.Provider).Include(a=>a.Approval).FirstOrDefault(r => r.ID == RequestId);
        }
        #endregion


        public void Save()
        {
            dbContext.SaveChanges();
        }

    }

}
