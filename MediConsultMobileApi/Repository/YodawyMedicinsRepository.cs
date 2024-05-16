using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class YodawyMedicinsRepository : IYodawyMedicinsRepository
    {
        private readonly ApplicationDbContext dbContext;

        public YodawyMedicinsRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<YodawyMedicins>> GetAll() => await dbContext.YodawyMedicins.ToListAsync();
      
        public async Task<YodawyMedicins> GetById(int medId) => await dbContext.YodawyMedicins.FirstOrDefaultAsync(m=>m.id==medId);

        public async Task<bool> MedicinsExists(int medId) =>await dbContext.YodawyMedicins.AnyAsync(y=>y.id==medId);
    }
}
