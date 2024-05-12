using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediConsultMobileApi.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SettingsRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public MemberNationalId GetByMemberId(int memberId)
        {
            return dbContext.MemberNationalIds.FirstOrDefault(m=> m.member_id == memberId);
        }
        public void EditNationalID(ChangeNationalIdDTO NationalIdDto)
        {
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;
            var folder = Path.Combine(serverPath, "MemberNationalId", NationalIdDto.member_id.ToString(), NationalIdDto.nid_image.FileName);
            var member = new MemberNationalId
            {
                national_id = NationalIdDto.national_id,
                member_id = NationalIdDto.member_id,
                nid_image = folder

            };
            dbContext.MemberNationalIds.Add(member);
            
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
