using MediConsultMobileApi.Models;

namespace MediConsultMobileApi.DTO
{
    public class ApprovalChronicDTO
    {
        public int approval_id { get; set; }
        public string approval_status { get; set; }
        public string approval_date { get; set; }
        public  string provider_name { get; set; }
        public string? folderPdf { get; set; }
        public List<Medicines>? Medicines { get; set; }
    }

    public class Medicines
    {
        public string med_name { get; set; }
        public double qty { get; set; }
        public string? dose { get; set; }
        public string? unit_name { get; set; }
        public string status { get; set; }
        public string? reason { get; set; }
    }
}
