using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberHistoryController : ControllerBase
    {
        private readonly IMemberHistoryRepsitory memberHistoryRepo;
        private readonly IMemberRepository memberRepo;

        public MemberHistoryController(IMemberHistoryRepsitory memberHistoryRepo , IMemberRepository memberRepo)
        {
            this.memberHistoryRepo = memberHistoryRepo;
            this.memberRepo = memberRepo;
        }

        [HttpGet("MemberHistory")]
        public IActionResult GetMemberHistory(string lang,[Required] int memberId,[FromQuery]MemberHistoryDto? historyDto, int startPage = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var memberExists = memberRepo.MemberExists(memberId);
            if (!memberExists)
            {
                return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

            }
            var member = memberHistoryRepo.GetMemberHistory(memberId);

            if (historyDto.Status != null && historyDto.Status.Any())
            {
                for (int i = 0; i < historyDto.Status.Length; i++)
                {
                    var sta = historyDto.Status[i];
                }
                
                member = member.Where(c => historyDto.Status.Contains(c.ServiceStatus));
            }


            if (historyDto.ServiceName != null && historyDto.ServiceName.Any())
            {
                for (int i = 0; i < historyDto.ServiceName.Length; i++)
                {
                    var sta = historyDto.ServiceName[i];
                }
                member = member.Where(c => historyDto.ServiceName.StartsWith(c.ServiceName));
            }


            if (historyDto.Diagnosis != null && historyDto.Diagnosis.Any())
            {
                for (int i = 0; i < historyDto.Diagnosis.Length; i++)
                {
                    var sta = historyDto.Diagnosis[i];
                }
                member = member.Where(c => historyDto.Diagnosis.StartsWith(c.Diagnosis));
            }


            if (historyDto.Request_Type != null && historyDto.Request_Type.Any())
            {
                for (int i = 0; i < historyDto.Request_Type.Length; i++)
                {
                    var sta = historyDto.Request_Type[i];
                }
                member = member.Where(c => historyDto.Request_Type.Contains(c.Request_Type));
            }

        

            if (historyDto.Qty != null && historyDto.Qty.ToString().Any())
            {
                for (int i = 0; i < historyDto.Qty.ToString().Length; i++)
                {
                    var sta = historyDto.Qty.ToString()[i];
                }
                member = member.Where(c => historyDto.Qty.ToString().StartsWith(c.Qty.ToString()));
            }
            var totalHistories = member.Count();

            member = member.Skip((startPage - 1) * pageSize).Take(pageSize).OrderBy(m => m.ServiceName);

            var memberHistory = new
            {

                TotalCount = totalHistories,
                PageNumber = startPage,
                PageSize = pageSize,
                Requests = member,


            };
            return Ok(memberHistory);
        }
    }
}
