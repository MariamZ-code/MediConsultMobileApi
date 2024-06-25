using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace MediConsultMobileApi.Repository
{
    public class IsMemberAllowedOnThisProviderRepository : IIsMemberAllowedOnThisProviderRepository
    {
        private readonly ApplicationDbContext dbContext;

        public IsMemberAllowedOnThisProviderRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> IsMemberAllowedOnThisProvider(int? memberId, int? providerId)
        {
            return await dbContext.IsMemberAllowedOnThisProvider(memberId, providerId);
        }
    }
}
