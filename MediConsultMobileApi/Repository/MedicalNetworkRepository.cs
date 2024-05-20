using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediConsultMobileApi.Repository
{
    public class MedicalNetworkRepository : IMedicalNetworkRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MedicalNetworkRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<MedicalNetwork> GetAll()
        {
            var medicalResult =  dbContext.medicalNetworks.AsNoTracking().AsQueryable();
            return medicalResult;
        }

        public PortalUsersTable GetByProviderId(int providerId) => dbContext.portalUsersTables.FirstOrDefault(p=> p.provider_id == providerId);
    }
}
