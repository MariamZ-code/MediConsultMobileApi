using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class GovernmentRepository : IGovernmentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public GovernmentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<AppSelectorGovernment> GetGovernments() => dbContext.AppSelectorGovernments.AsNoTracking().AsQueryable();

        public bool GovernmentExsists(int governmentId) => dbContext.AppSelectorGovernments.Any(g=> g.government_id==governmentId);
    }
}
