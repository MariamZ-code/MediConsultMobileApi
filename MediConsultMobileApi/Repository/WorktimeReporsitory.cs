using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class WorktimeReporsitory : IWorktimeReporsitory
    {
        private readonly ApplicationDbContext dbContext;

        public WorktimeReporsitory(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<ProviderWorktime> GetWorktimes(int providerId)=> dbContext.ProviderWorktimes.Include(p=>p.providerData)
            .Include(p=>p.workTime)
            .Where(p=>p.provider_id==providerId)
            .ToList();
    }
}
