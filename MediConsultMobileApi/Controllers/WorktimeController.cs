using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorktimeController : ControllerBase
    {
        private readonly IWorktimeReporsitory worktimeRepo;
        private readonly IProviderDataRepository providerRepo;

        public WorktimeController(IWorktimeReporsitory worktimeRepo , IProviderDataRepository providerRepo)
        {
            this.worktimeRepo = worktimeRepo;
            this.providerRepo = providerRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get([Required] int providerId, string? lang)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });

            var providerExists = await providerRepo.ProviderExistsAsync(providerId);

            if (!providerExists)
            {
                return NotFound(new MessageDto { Message = Messages.EnterProvider(lang) });
            }


            var workTimes = worktimeRepo.GetWorktimes(providerId);

            var worktimeDTOs = new List<GetWorktimeDTO>();

            foreach (var workTime in workTimes)
            {
                var worktimeDTO = new GetWorktimeDTO
                {
                    Provider_id = providerId,
                    Day = workTime.workTime.day,
                    Worktime_id = workTime.workTime.id,
                    Slot = workTime.workTime.time_slot,
                    Time_From = workTime.workTime.time_from,
                    Time_To = workTime.workTime.time_to,
                };
                worktimeDTOs.Add(worktimeDTO);
            }


            return Ok(worktimeDTOs);


        }
    }
}
