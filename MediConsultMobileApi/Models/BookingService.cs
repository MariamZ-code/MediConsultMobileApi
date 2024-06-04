using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    //TODO : Add to db
    [Table("BookingService")]
    [Keyless]
    public class BookingService
    {
        [ForeignKey("BookingLabAndScan")]
        public int BookingLabAndScanId { get; set; }
        public BookingLabAndScan BookingLabAndScan { get; set; }


        [ForeignKey("ServiceData")]

        public int ServiceDataId { get; set; }
        public ServiceData ServiceData { get; set; }
    }
}
