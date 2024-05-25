using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("Provider_worktime")]
    public class ProviderWorktime
    {
        public int id { get; set; }

        [ForeignKey("providerData")]
        public int provider_id { get; set; }
        public ProviderData providerData { get; set; }

        [ForeignKey("workTime")]
        public int worktime_id { get; set; }
        public WorkTime workTime { get; set; }
    }
}
