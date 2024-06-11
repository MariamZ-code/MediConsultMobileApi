namespace MediConsultMobileApi.DTO
{
    public class FilterMedicalNetworkDTO
    {
        public string? cityName { get; set; }
        public string? govName { get; set; }
        public string? providerName { get; set; }
        public string? specialtyName { get; set; }
        public string? subSpecialtyName { get; set; }
        public string[]? categories { get; set; }
    }
}
