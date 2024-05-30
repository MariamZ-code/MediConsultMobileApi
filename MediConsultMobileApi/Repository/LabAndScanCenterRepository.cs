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
        public IQueryable<UniqueLabAndScanServiceDto> GetLabAndScanUnique() => dbContext.labAndScanCenters
                                .GroupBy(s => new { s.Service_name_En, s.Service_Name_Ar })
                                .Select(g => g.First())
                                .Select(s => new UniqueLabAndScanServiceDto
                                {
                                    Service_id = s.Service_id,
                                    Service_name_En = s.Service_name_En,
                                    Service_Name_Ar = s.Service_Name_Ar,
                                    Service_price = s.Service_price,
                                    provider_name_en = s.provider_name_en
                                })
                                .AsQueryable();
    }
}
