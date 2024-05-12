using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.Repository.Interfaces
{
    public interface IBookingRepository
    {
        Booking GetById(int? bookingId);
        Booking AddBooking(AddBookingDto bookingDto);
        Booking UpdateBooking(UpdateBookingDto bookingDto , int? bookingId);
        bool BookingExists(int? bookingId);
        IQueryable<Booking> GetBookingByMemberId(int? memberId);
        void DeleteBooking(int? booingId);
        void Save();
    }
}
