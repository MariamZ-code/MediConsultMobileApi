using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>()
               .HasOne(s => s.ParentService)
               .WithMany(s => s.ChildServices)
               .HasForeignKey(s => s.Service_Class_Parent_Id);
            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Member> Members { get; set; }

        public DbSet<Member_services_with_copayments> member_Services_With_Copayments { get; set; }

        public DbSet<Serviceview> Serviceviews { get; set; }
        public DbSet<MedicalNetwork> medicalNetworks { get; set; }
        public DbSet<MemberECardInfo> MemberECardInfos { get; set; }
        public DbSet<ProviderData> Providers { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<ClientBranchMember> clientBranchMembers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<RefundType> RefundTypes { get; set; }
        public DbSet<ClientPriceList> ClientPriceLists { get; set; }
        public DbSet<MemberNationalId> MemberNationalIds { get; set; }
        public DbSet<ProgramTypeInfo> ProgramTypeInfos { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<NotificationHead> NotificationHeads { get; set; }
        public DbSet<NotificationData> NotificationDatas { get; set; }
        public DbSet<ServiceCopaymentproviderView> ServiceCopaymentproviders { get; set; } 
        public DbSet<PharmaApprovalAct> PharmaApprovalActs { get; set; }
        public DbSet<YodawyMedicins> YodawyMedicins { get; set; }
        public DbSet<ApprovalLog> ApprovalLogs { get; set; }
        public DbSet<AppActReasons> AppActReasons { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ProviderSpecialty> ProviderSpecialties { get; set; }
        public DbSet<ProviderLocation> providerLocations { get; set; }
        public DbSet<GeneralSpecialty> GeneralSpecialties { get; set; }
        public DbSet<AppSelectorGovernmentCity> SelectorGovernmentCities  { get; set; }
        public DbSet<AppSelectorGovernment> AppSelectorGovernments { get; set; }
        public DbSet<AppDiagosis> AppDiagosis { get; set; }
        public DbSet<ApprovalAct> ApprovalAct { get; set; }
        public DbSet<ApprovalDiagnosis> ApprovalDiagnosis { get; set; }
       
        public DbSet<MemberHistory> MemberHistories { get; set; }

    }
}
