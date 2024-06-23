
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using MediConsultMobileApi.HangFire;
using MediConsultMobileApi.Hubs;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository;
using MediConsultMobileApi.Repository.Interfaces;
using MediConsultMobileApi.ServiceRegistrations;
using MediConsultMobileApi.Validations;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCaching();

// Register services 
builder.Services.RegisterApplicationSservices();

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();


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
