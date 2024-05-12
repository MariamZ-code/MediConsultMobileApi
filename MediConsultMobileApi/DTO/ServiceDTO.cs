using MediConsultMobileApi.Models;
using System.Text.Json.Serialization;

namespace MediConsultMobileApi.DTO
{
    public class ServiceDTO
    {
        public string parentServiceName_En { get; set; }
        public string? ParentImage { get; set; }
        public string? copyment { get; set; }

        public List<ServiceDataDto> Infos { get; set; }
    }




}
