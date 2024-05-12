using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediConsultMobileApi.Repository
{
    public class serviceRepository :IserviceRepository
    {
        private readonly ApplicationDbContext dbContext;

        public serviceRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<member_services_with_copayments>> GetAll()
        {
            var result = from member in dbContext.Members
                         join service in dbContext.member_Services_With_Copayments
                         on member.program_id equals service.program_id
                         select new
                         {
                             ServiceName = service.service_name,
                             ServiceId = service.program_id,
                             ServiceCopament = service.copayment,

                         };



            return (List<member_services_with_copayments>)result;
        }
    }
}
