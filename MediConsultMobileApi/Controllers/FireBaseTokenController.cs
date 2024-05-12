using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FireBaseTokenController : ControllerBase
    {
        private readonly ITokenRepository tokenRepo;
        private readonly IMemberRepository memberRepo;
        private readonly IAuthRepository authRepo;

        public FireBaseTokenController(ITokenRepository tokenRepo , IMemberRepository memberRepo , IAuthRepository authRepo)
        {
            this.tokenRepo = tokenRepo;
            this.memberRepo = memberRepo;
            this.authRepo = authRepo;
        }
        [HttpPost("SaveToken")]
        public  IActionResult SaveToken(FirebaseTokenDTO tokenDto)
        {
            if (ModelState.IsValid)
            {
                var memberExists =  memberRepo.MemberExists(tokenDto.MemberId);
                var memberLoginExists =authRepo.MemberLoginExists(tokenDto.MemberId);
                if (!memberExists)
                {
                    return BadRequest( new MessageDto { Message = "Member Id not found" });
                }
                if (!memberLoginExists)
                {
                    return BadRequest(new MessageDto { Message = "Member Id not have Account" });
                }

                if (tokenDto.Firebase_token is null || tokenDto.Firebase_token.Trim().Length==0 )
                {
                    return BadRequest(new MessageDto { Message = "Enter Token " });


                }
                tokenRepo.SaveToken(tokenDto);
                tokenRepo.SaveChanges();

                return Ok(new MessageDto { Message = "Token Saved"});
            }

            return BadRequest(new MessageDto { Message = "Wrong" });
        }
    }
}
