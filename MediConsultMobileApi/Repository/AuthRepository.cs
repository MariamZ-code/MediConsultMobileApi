using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Net;

using MediConsultMobileApi.Language;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Util;

namespace MediConsultMobileApi.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMemberRepository memberRepo;

        public AuthRepository(ApplicationDbContext dbContext, IMemberRepository memberRepo)
        {
            this.dbContext = dbContext;
            this.memberRepo = memberRepo;
        }


        #region GetClientBranchMemberById
        public ClientBranchMember GetById(int memberId)
        {
            return dbContext.clientBranchMembers.FirstOrDefault(m => m.member_id == memberId);
        }

        #endregion


        #region Registeration

        public async void Registeration(RegisterUserDto userDto, int memberId)
        {
            var member = GetById(memberId);
            var createDateAndGender = memberRepo.CreateDateAndGender(userDto.NationalId);

            member.mobile = userDto.Mobile;
            member.member_nid = userDto.NationalId;
            member.member_birthday = createDateAndGender.date;
            member.member_gender = createDateAndGender.gender;
            member.is_enabled = 1;

            var memberPassword = GetById(memberId);
            memberPassword.the_password = userDto.Password;

            dbContext.clientBranchMembers.Update(member);
         

        }
        #endregion

        #region Login
        public async Task<MessageDto> Login(LoginUserDto userDto, string lang)
        {
            var authDto = new MessageDto();
            var member = new ClientBranchMember();
            if (userDto.Id.Length > 13)
            {

                member = await dbContext.clientBranchMembers.FirstOrDefaultAsync(u => u.member_nid == userDto.Id.ToString()&& u.the_password == userDto.Password);

            }
            else
            {

                member = await dbContext.clientBranchMembers.FirstOrDefaultAsync(u => u.member_id == int.Parse(userDto.Id)&& u.the_password == userDto.Password);
            }

            if (member is null)
            {
                authDto.Message = Messages.PasswordAndIdIncorrect(lang);

                return authDto;
            }

            if (member.is_enabled == 0 || member.is_enabled is null )
            {
                authDto.Message = Messages.AccountDisabled(lang);

                return authDto;
            }

            authDto.Message = Messages.LoginSuccessfully(lang);

            return authDto;
        }
        #endregion

        #region MemberExists
        public bool MemberLoginExists(int? memberId)
        {
            return dbContext.clientBranchMembers.Any(m => m.member_id == memberId);

        }


        #endregion

      

        #region SendOtp

        public void SendOtp(string otp, int memberId)
        {
            ClientBranchMember member = GetById(memberId);
            member.Otp = otp;

            dbContext.clientBranchMembers.Update(member);
            dbContext.SaveChanges();

        }
        public void UpdateOtp(ClientBranchMember member)
        {
            
            dbContext.clientBranchMembers.Update(member);
            dbContext.SaveChanges();

        }
        #endregion

        #region ChangePassword

        public void ChangePass(string otp, int id, ChangePasswordDTO changeDto)
        {
            var member = GetById(id);


            member.the_password = changeDto.Password;

            dbContext.clientBranchMembers.Update(member);

            dbContext.SaveChanges();

        }

        #endregion

        #region SaveChange
        public void Save()
        {

            dbContext.SaveChanges();

        }
        #endregion

    }
}
