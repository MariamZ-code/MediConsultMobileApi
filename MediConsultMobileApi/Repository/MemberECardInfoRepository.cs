using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Esf;

namespace MediConsultMobileApi.Repository
{
    public class MemberECardInfoRepository : IMemberECardInfoRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MemberECardInfoRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<MemberECardInfo> GetInfos(int? programId)
        {
            return dbContext.MemberECardInfos.Where(m => m.program_id == programId).AsNoTracking().ToList();    
        }
    }
}
