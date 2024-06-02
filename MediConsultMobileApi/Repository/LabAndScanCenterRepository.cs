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

        public IQueryable<LabAndScanCenter> GetLabAndScanCenters(List<int> serviceIds) => dbContext.LabAndScanCenters
                .Where(l => serviceIds.Contains(l.Service_id))
                .GroupBy(l => new
                {
                    l.provider_id,
                    l.Service_id,
                    l.Service_name_En,
                    l.Service_Name_Ar,
                    l.provider_name_en,
                    l.provider_name_ar,
                    l.Service_price
                })
                .Select(g => new LabAndScanCenter
                {
                    Service_id = g.Key.Service_id,
                    Service_name_En = g.Key.Service_name_En,
                    Service_Name_Ar = g.Key.Service_Name_Ar,
                    provider_name_en = g.Key.provider_name_en,
                    provider_name_ar = g.Key.provider_name_ar,
                    Service_price = g.Key.Service_price,
                    provider_id = g.Key.provider_id,
                })
                .OrderBy(g => g.Service_price)

                .AsQueryable();


        public LabAndScanCenter GetLabAndScanServiceName(int serviceId) => dbContext.LabAndScanCenters.FirstOrDefault(s => s.Service_id == serviceId);
    }
}
