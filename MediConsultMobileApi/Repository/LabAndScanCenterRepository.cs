using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class LabAndScanCenterRepository : ILabAndScanCenterRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LabAndScanCenterRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<GetServicesLabAndScan> GetLabAndScanUnique() => dbContext.GetServicesLabAndScans.AsNoTracking()
                                .AsQueryable();
    }
}
