using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernmentController : ControllerBase
    {
        private readonly IGovernmentRepository govRepo;

        public GovernmentController(IGovernmentRepository govRepo)
        {
            this.govRepo = govRepo;
        }

        #region GetGovernment
        [HttpGet("GetGovernment")]
        public IActionResult GetGovernment(string? lang)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });


            var govs = govRepo.GetGovernments();
            if (lang == "en")
                govs = govs.OrderBy(g => g.government_name_en);
            else
                govs= govs.OrderBy(g=>g.government_name_ar);


            var resultGov = new List<GovernmentDTO>();
            foreach (var gov in govs)
            {

                var govDto = new GovernmentDTO
                {
                    government_id = gov.government_id,
                };
                if (lang=="en")
                    govDto.government_name = gov.government_name_en;
                
                else
                    govDto.government_name = gov.government_name_ar;
                
                resultGov.Add(govDto);
            }

            return Ok(resultGov);


        }
        #endregion
    }
}
