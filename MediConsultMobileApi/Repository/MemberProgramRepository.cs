using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;

namespace MediConsultMobileApi.Repository
{
    public class MemberProgramRepository : IMemberProgramRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MemberProgramRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public MemberProgram GetMemberbyMemberId(int memberId) => dbContext.memberPrograms.FirstOrDefault(c=>c.Member_Id == memberId);
    }
}
