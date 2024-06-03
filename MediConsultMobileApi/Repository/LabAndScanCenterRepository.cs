using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediConsultMobileApi.Repository
{
    public class LabAndScanCenterRepository : ILabAndScanCenterRepository
    {
        private readonly ApplicationDbContext dbContext;

        public LabAndScanCenterRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<GetServicesLabAndScan> GetLabAndScanUnique(int categoryId) => dbContext.GetServicesLabAndScans
                                .Where(c => c.Category_ID == categoryId)
                                .AsNoTracking()
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

        public void AddBooking(BookingLabAndScan booking)
        {

            dbContext.BookingLabAndScans.Add(booking);
        }


        public void AddBookingService(BookingService service) => dbContext.BookingServices.Add(service);
        public void Save() => dbContext.SaveChanges();

        public void EditBooking(BookingLabAndScanCenterDTO bookingLab, int bookingId)
        {
            var oldBooking = dbContext.BookingLabAndScans
                       .Include(b => b.service)
                       .ThenInclude(bs => bs.ServiceData)
                       .FirstOrDefault(b => b.id == bookingId);
            var oldBookingService = dbContext.BookingServices.Where(b=>b.BookingLabAndScanId== bookingId).ToList();

            

            //foreach (var serviceId in oldBookingService)
            //{
               
            //       serviceId.ServiceDataId= bookingLab.serviceIds
                

            //    serviceIds.Add(service);
            //}

            oldBooking.member_id = bookingLab.member_id;
            oldBooking.provider_id = bookingLab.provider_id;
            oldBooking.service = bookingLab.serviceIds;
            
        }
    }
}
