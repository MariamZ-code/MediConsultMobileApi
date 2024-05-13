
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using MediConsultMobileApi.HangFire;
using MediConsultMobileApi.Hubs;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository;
using MediConsultMobileApi.Repository.Interfaces;

using MediConsultMobileApi.Validations;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCaching();
//var txt = "_myAllowSpecificOrigins";

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAuthRepository, AuthRepository>(); 
builder.Services.AddScoped<ChatHub, ChatHub>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IServiceRepository, SeviceRepository>();
builder.Services.AddScoped<IProviderDataRepository , ProviderDataRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IMedicalNetworkRepository , MedicalNetworkRepository>();
builder.Services.AddScoped<ITokenRepository , TokenRepository>();
builder.Services.AddScoped<ICategoryRepository , CategoryRepository>();
builder.Services.AddScoped<IRefundRepository, RefundRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IRefundTypeRepository, RefundTypeRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<IPolicyInfoRepository, PolicyInfoRepository>();
builder.Services.AddScoped<IMemberECardInfoRepository, MemberECardInfoRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IValidation, Validation>();
builder.Services.AddScoped<IParentServiceRepository, ParentServiceRepository>();
builder.Services.AddScoped<IServiceCopaymentRepository, ServiceCopaymentRepository>();
builder.Services.AddScoped<IApprovalRepository, ApprovalRepository>();
builder.Services.AddScoped<IPharmaApprovalRepository,PharmaApprovalRepository>();
builder.Services.AddScoped<IApprovalLogRepository, ApprovalLogRepository>();
builder.Services.AddScoped<HangFireController, HangFireController>();
builder.Services.AddScoped<IProviderLocationRepository, ProviderLocationRepository>();
builder.Services.AddScoped<IProviderSpecialtyRepository, ProviderSpecialtyRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IMemberHistoryRepsitory, MemberHistoryRepsitory>();
builder.Services.AddScoped<IYodawyMedicinsRepository, YodawyMedicinsRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("cs2")));

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("cs2")));
builder.Services.AddHangfireServer();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string jsonKeyFilePath = "Keys/mediconsult_app_firebase_key.json";
string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonKeyFilePath);
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(fullPath),
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});



var app = builder.Build();
app.UseResponseCaching();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // This maps all attribute-routed controller endpoints
                                // Add more endpoint mappings if necessary
});
app.UseHangfireDashboard("/Api");
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
