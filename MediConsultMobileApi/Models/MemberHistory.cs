using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Models
{
    public class MemberHistory
    {
        [Key]
      
        //public int MemberId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDate { get; set; }
        public string Diagosis { get; set; }
        public string ServiceStatus { get; set; }
        public string Request_Type { get; set; }
        public double Qty { get; set; }
    }
}
