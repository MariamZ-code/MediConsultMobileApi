using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IMemberRepository memberRepo;

        public UserAccountController(IMemberRepository memberRepo)
        {
            this.memberRepo = memberRepo;
        }
        [HttpGet("UserAccount")]
        public IActionResult UserAccount(string lang ,int memberId)
        {
            if (ModelState.IsValid)
            {
                var member = memberRepo.MemberDetails(memberId);
                var memberExsits= memberRepo.MemberExists(memberId);
                if (!memberExsits)
                {
                    return NotFound(new MessageDto { Message = Messages.MemberNotFound(lang) });
                }

                if (member.is_enabled == 0)
                {
                    return Ok(new MessageDto { Message = Messages.AccountDisabled(lang) });

                }
                return Ok(new MessageDto { Message= "can login" });
            }
            return BadRequest(ModelState);
        }
    }
}
