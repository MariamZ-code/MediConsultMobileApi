using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class PolicyInfoRepository : IPolicyInfoRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PolicyInfoRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public ProgramTypeInfo? GetInfo(int serviceClassId)
        {
            return dbContext.ProgramTypeInfos.Include(s=> s.Service).FirstOrDefault(i => i.Service_Class_id == serviceClassId);
        }
    }
}
