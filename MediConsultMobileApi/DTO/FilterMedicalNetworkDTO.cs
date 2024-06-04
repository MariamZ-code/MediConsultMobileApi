namespace MediConsultMobileApi.DTO
{
    public class FilterMedicalNetworkDTO
    {
        public string? providerName { get; set; }
        public string[]? categories { get; set; }
        public string? gov { get; set; }
        public string? city { get; set; }
    }
}
