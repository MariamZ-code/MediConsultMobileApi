using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace MediConsultMobileApi.Repository
{
    public class MemberHistoryRepsitory : IMemberHistoryRepsitory
    {
        private readonly ApplicationDbContext dbContext;

        public MemberHistoryRepsitory(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<MemberHistory> GetMemberHistory(int memberId)
        {
            return dbContext.MemberHistories.FromSqlRaw("EXEC MemeberHistory @memberId", 
            new SqlParameter("@memberId", memberId)).AsNoTracking().AsEnumerable();
        }
    }
}
