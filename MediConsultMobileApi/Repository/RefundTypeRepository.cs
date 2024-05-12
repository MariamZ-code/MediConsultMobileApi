using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class RefundTypeRepository : IRefundTypeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RefundTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<RefundType>> GetAllRefundType()
        { 
            return await dbContext.RefundTypes.AsNoTracking().ToListAsync();

        }
    

    }
}
