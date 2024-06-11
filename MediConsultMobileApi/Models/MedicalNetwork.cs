using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediConsultMobileApi.Models
{
    [Table("app_en_get_medical_network_map_view")]
    [Keyless]
    public class MedicalNetwork
    {
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Email { get; set; }
        public string? Hotline { get; set; }
        public string? Mobile { get; set; }
        public string? Telephone { get; set; }

        public string provider_name { get; set; }
        public string provider_name_en { get; set; }
        public string government_name_en { get; set; }
        public string government_name_ar { get; set; }
        public string city_name_ar { get; set; }
        public string city_name_en { get; set; }
        public string Category_Name_En { get; set; }
        public string? Speciality { get; set; }
        public string? General_Specialty_Name_En { get; set; }
        public string Category { get; set; }
        public int provider_id { get; set; }
        public int city_id { get; set; }
        public int location_id { get; set; }
        public string Specialty_Name_En { get; set; }
        public string Specialty_Name_Ar { get; set; }
    }
}
