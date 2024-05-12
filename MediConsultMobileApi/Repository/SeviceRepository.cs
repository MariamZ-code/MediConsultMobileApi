using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class SeviceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMemberRepository memberRepo;

        public SeviceRepository(ApplicationDbContext dbContext , IMemberRepository memberRepo)
        {
            this.dbContext = dbContext;
            this.memberRepo = memberRepo;
        }
        public  async Task<List<Member_services_with_copayments>> GetById(Member member)
        {
              
                var service =await dbContext.member_Services_With_Copayments.Where(x => x.program_id == member.program_id).ToListAsync();
                return service;
             
        }
        public bool ServiceExists(int serviceId)
        {
            return dbContext.Services.Any(s=> s.Service_Class_id == serviceId);
        }
        public Service GetById(int serviceId) 
        {
            return dbContext.Services.FirstOrDefault(s => s.Service_Class_id == serviceId);
        }




    }
}
