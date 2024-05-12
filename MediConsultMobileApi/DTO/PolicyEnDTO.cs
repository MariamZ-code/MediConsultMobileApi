namespace MediConsultMobileApi.DTO
{
    public class PolicyEnDTO
    {
        public int Policy_id { get; set; }

        public int Program_id { get; set; }

        public int Service_Class_Id { get; set; }

        public string ServiceNameEn { get; set; }

        public double SL_Limit { get; set; }

        public int SL_Copayment { get; set; }
    }
}
