using Azure.Core;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;

namespace MediConsultMobileApi.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MemberRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region MemberExist
        public bool MemberExists(int? memberId)
        {
            return dbContext.Members.Any(m => m.member_id == memberId);

        }

        #endregion

        #region GetMemberbyId
        public async Task<Member> GetByID(int? id)
        {

            return await dbContext.Members.FirstOrDefaultAsync(m => m.member_id == id);

        }

        #endregion          
         
        #region ValidationMember
        public async Task<MessageDto> validation(Member member)
        {
            var msg = new MessageDto();

            if (member is null)
            {
                msg.Message = "User in Archive";
                return msg;

            }
            if (member.program_name is null)
            {
                msg.Message = "User in Archive";
                return msg;

            }
            if (member.member_status == "Deactivated")
            {
                msg.Message = "User is Deactivated";

                return msg;


            }
            if (member.member_status == "Hold")
            {
                msg.Message = "User is Hold";
                return msg;
            }



            return msg;
        }

        #endregion

        #region MemberFamily

        public async Task<List<ClientBranchMember>> MemberFamily(int id)
        {
            return await dbContext.clientBranchMembers.Where(f=>f.member_HOF_id== id).AsNoTracking().ToListAsync();
        }
        #endregion

        #region MemberDetails
        public  ClientBranchMember MemberDetails(int memberId)
        {
            return  dbContext.clientBranchMembers.FirstOrDefault(m => m.member_id == memberId); 
        }
        #endregion

        #region UpdateMember
        public  void UpdateMember(UpdateMemberDTO memberDTO , int id)
        {
            var serverPath = AppDomain.CurrentDomain.BaseDirectory;

            var member =  MemberDetails(id);
            member.email = memberDTO.Email;
            member.mobile = memberDTO.Mobile;
            //member.member_nid = memberDTO.SSN;

            //if (memberDTO.SSN is not null)
            //{
            //    var (date, gender) = CreateDateAndGender(memberDTO.SSN);
            //    member.member_birthday = date;
           
            //    member.member_gender = gender;
            //}
            if (memberDTO.Photo is not null)
            {
                var folder = Path.Combine(serverPath, "Members", id.ToString(), memberDTO.Photo.FileName);
                member.member_photo = folder;
            }


            dbContext.clientBranchMembers.Update(member);
            

        }
        #endregion

        #region CreateDateAndGender 

       public (string date, string gender) CreateDateAndGender(string ssn)
        {
            if (ssn is null)
            {
                return (null , null ); 
            }
            char[] charSSN = ssn.ToCharArray();
       

                var gender = (charSSN[12]) % 2 == 0 ? "Female" : "Male";

                var centuray = charSSN[0] == '2' ? "19" : "20";

                var year = centuray + charSSN[1] + charSSN[2];

                var month = $"{charSSN[3]}{charSSN[4]}";

                var day = $"{charSSN[5]}{charSSN[6]}";

                var date = day  + "-" + month + "-" + year;
                return (date , gender);
        


        }
        #endregion


        #region ValidationDateGender

        public bool IsValidDate(string date)
        {
            // Parse the date to get the day part.
            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
               
                return parsedDate.Day < 32 && parsedDate.Month < 13;
            }

            return false;
        }
        #endregion

        public void SaveDatabase()
        {
            dbContext.SaveChanges();
        }
        public bool SSNExists(string nId)
        {
            return dbContext.clientBranchMembers.Any(m => m.member_nid == nId);

        }

        public bool PhoneExists(string mobile)
        {

            return dbContext.clientBranchMembers.Any(m => m.mobile == mobile);

        }

        public ClientBranchMember GetMemberByMobile(string mobile)
        {
            return dbContext.clientBranchMembers.FirstOrDefault(m=> m.mobile == mobile);
        }

        public ClientBranchMember GetMemberByEmail(string email)
        {
            return dbContext.clientBranchMembers.FirstOrDefault(m => m.email == email);

        }
        public ClientBranchMember GetMemberByNationalId(string nId)
        {
            return dbContext.clientBranchMembers.FirstOrDefault(m => m.member_nid == nId && m.member_status=="Activated");

        }
       
    }
}
