using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

namespace MediConsultMobileApi.Repository
{
    public class GovernmentRepository : IGovernmentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public GovernmentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<AppSelectorGovernment> GetGovernments() => dbContext.AppSelectorGovernments.ToList();
    }
}
