using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyRepository policyRepo;

        public PolicyController(IPolicyRepository policyRepo)
        {
            this.policyRepo = policyRepo;
        }



        [HttpGet("ProgramId")]

        public async Task<IActionResult> GetByProgramId(int programId, string lang)
        {
            if (ModelState.IsValid)
            {
                var serviceExists = policyRepo.ServiceExists(programId);
                var policies = await policyRepo.GetByProgramId(programId);


                if (serviceExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.Policy(lang) });
                }
                var policyListArDto = new List<PolicyArDTO>();
                var policyListEnDto = new List<PolicyEnDTO>();
                if (lang == "en")
                {
                    foreach (var policy in policies)
                    {
                        var policyEnDto = new PolicyEnDTO
                        {
                            Policy_id = policy.Policy_id,
                            Program_id = policy.Program_id,
                            Service_Class_Id = policy.Service_Class_Id,
                            SL_Copayment = policy.SL_Copayment,
                            SL_Limit = policy.SL_Limit,
                            ServiceNameEn = policy.Service?.Service_Class_Name_En

                        };
                        policyListEnDto.Add(policyEnDto);
                    }
                    return Ok(policyListEnDto);

                }
                foreach (var policy in policies)
                {
                    var policyArDto = new PolicyArDTO
                    {
                        Policy_id = policy.Policy_id,
                        Program_id = policy.Program_id,
                        Service_Class_Id = policy.Service_Class_Id,
                        SL_Copayment = policy.SL_Copayment,
                        SL_Limit = policy.SL_Limit,
                        ServiceNameAr = policy.Service?.Service_Class_Name_Ar,

                    };
                    policyListArDto.Add(policyArDto);
                }


                return Ok(policyListArDto);


            }
            return BadRequest(ModelState);
        }
    }
}
