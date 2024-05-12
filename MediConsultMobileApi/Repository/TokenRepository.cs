using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TokenRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void SaveToken(FirebaseTokenDTO tokenDto)
        {
            var member = dbContext.clientBranchMembers.FirstOrDefault(x=> x.member_id== tokenDto.MemberId);

            member.member_id = tokenDto.MemberId;
            member.firebase_token = tokenDto.Firebase_token;
              

                dbContext.Update(member);
              
            
            
        }

        public void SaveChanges()
        {
                dbContext.SaveChanges();

        }
    }
}
