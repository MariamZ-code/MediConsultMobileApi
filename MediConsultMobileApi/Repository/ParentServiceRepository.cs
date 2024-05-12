using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class ParentServiceRepository : IParentServiceRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ParentServiceRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    

        public List<Serviceview> Get(int serviceId)
        {
            return dbContext.Serviceviews.Where(s=> s.service_id== serviceId && s.hidden ==1).AsNoTracking().ToList();
        }
    }
}
