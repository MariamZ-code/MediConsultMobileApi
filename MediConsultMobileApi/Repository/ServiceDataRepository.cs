using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

namespace MediConsultMobileApi.Repository
{
    public class ServiceDataRepository : IServiceDataRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ServiceDataRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool ServiceExists(int serviceId)
        => dbContext.ServiceDatas.Any(s=>s.Service_id== serviceId);
    }
}
