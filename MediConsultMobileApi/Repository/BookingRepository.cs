

using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext dbContext;
        public BookingRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #region GetById
        public Booking GetById(int? bookingId) => dbContext.Bookings.Include(p=>p.Provider).ThenInclude(c=>c.Category).Include(l => l.ProviderLocation).FirstOrDefault(b => b.id == bookingId);
        #endregion

        #region AddBooking
        public Booking AddBooking(AddBookingDto bookingDto)
        {
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            var booking = new Booking
            {
                notes = bookingDto.notes,

                provider_id = bookingDto.provider_id,
                member_id = bookingDto.member_id,
                provider_Location_id = bookingDto.provider_Location_id
            };
            dbContext.Bookings.Add(booking);

            dbContext.SaveChanges();
            var folder = Path.Combine(serverPath, "MemberPortalApp", booking.member_id.ToString(), "Booking", booking.id.ToString());

            booking.attachment = folder;
            dbContext.SaveChanges();

            return booking;
        }
        #endregion

        #region EditBooking
        public Booking UpdateBooking(UpdateBookingDto bookingDto , int? bookingId)
        {
            var booking = GetById(bookingId);
            booking.notes = bookingDto.notes;
            booking.provider_id = bookingDto.provider_id;   
            booking.provider_Location_id = bookingDto.provider_Location_id;
        

            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            var folder = Path.Combine(serverPath, "MemberPortalApp", booking.member_id.ToString(), "Booking", bookingId.ToString());

            booking.attachment = folder;
            dbContext.Bookings.Update(booking);
            return booking;

        }


        #endregion

        #region BookingExists
        public bool BookingExists(int? bookingId)
        {
            return dbContext.Bookings.Any(r => r.id == bookingId);
        }

        #endregion

        #region HistoryOfBooking

        public IQueryable<Booking> GetBookingByMemberId(int? memberId)
        {

          return  dbContext.Bookings.Include(p=>p.Provider)
                                    .ThenInclude(c=>c.Category)
                                    .Include(l => l.ProviderLocation)  
                                    .Where(r => r.member_id == memberId).AsNoTracking().AsQueryable();

        }

        #endregion

        #region DeleteBooking
        public void DeleteBooking(int? booingId)
        {
            var booking = GetById(booingId);
            booking.status = "is_canceled";

            dbContext.Update(booking);

        }
        #endregion
        public void Save() => dbContext.SaveChanges();
    }
}
