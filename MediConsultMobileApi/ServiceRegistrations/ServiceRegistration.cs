using MediConsultMobileApi.HangFire;
using MediConsultMobileApi.Hubs;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository;
using MediConsultMobileApi.Repository.Interfaces;
using MediConsultMobileApi.Validations;

namespace MediConsultMobileApi.ServiceRegistrations
{
    public static class ServiceRegistration
    { 
        public static void RegisterApplicationSservices(this IServiceCollection services)
        {

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ChatHub, ChatHub>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IServiceRepository, SeviceRepository>();
            services.AddScoped<IProviderDataRepository, ProviderDataRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IMedicalNetworkRepository, MedicalNetworkRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IRefundRepository, RefundRepository>();
            services.AddScoped<IPolicyRepository, PolicyRepository>();
            services.AddScoped<IRefundTypeRepository, RefundTypeRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            services.AddScoped<IPolicyInfoRepository, PolicyInfoRepository>();
            services.AddScoped<IMemberECardInfoRepository, MemberECardInfoRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IValidation, Validation>();
            services.AddScoped<IParentServiceRepository, ParentServiceRepository>();
            services.AddScoped<IServiceCopaymentRepository, ServiceCopaymentRepository>();
            services.AddScoped<IApprovalRepository, ApprovalRepository>();
            services.AddScoped<IPharmaApprovalRepository, PharmaApprovalRepository>();
            services.AddScoped<IApprovalLogRepository, ApprovalLogRepository>();
            services.AddScoped<HangFireController, HangFireController>();
            services.AddScoped<IProviderLocationRepository, ProviderLocationRepository>();
            services.AddScoped<IProviderSpecialtyRepository, ProviderSpecialtyRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IMemberHistoryRepsitory, MemberHistoryRepsitory>();
            services.AddScoped<IYodawyMedicinsRepository, YodawyMedicinsRepository>();
            services.AddScoped<IMemberProgramRepository, MemberProgramRepository>();
            services.AddScoped<IApprovalTimelineRepository, ApprovalTimelineRepository>();
            services.AddScoped<IProviderRatingRepository, ProviderRatingRepository>();
            services.AddScoped<IGovernmentRepository, GovernmentRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ILabAndScanCenterRepository, LabAndScanCenterRepository>();
            services.AddScoped<IServiceDataRepository, ServiceDataRepository>();
            services.AddScoped<IWorktimeReporsitory, WorktimeReporsitory>();
            services.AddScoped<IIsMemberAllowedOnThisProviderRepository, IsMemberAllowedOnThisProviderRepository > ();
        }
    }
}
