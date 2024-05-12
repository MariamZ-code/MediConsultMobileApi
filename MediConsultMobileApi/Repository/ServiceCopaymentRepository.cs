using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class ServiceCopaymentRepository : IServiceCopaymentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ServiceCopaymentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<ServiceCopaymentproviderView> GetServices(int memberId)
        {
            return dbContext.ServiceCopaymentproviders.Where(m=> m.member_id == memberId).AsNoTracking().ToList();
        }
    }
}
