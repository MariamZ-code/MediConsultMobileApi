using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderSpecialtyController : ControllerBase
    {
        private readonly IProviderSpecialtyRepository specialRepo;

        public ProviderSpecialtyController(IProviderSpecialtyRepository specialRepo)
        {
            this.specialRepo = specialRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetProviderSpecialty(string lang)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = " Please Enter language" });

            var providerSpeCount = specialRepo.GetCountOfProviders();

            return Ok(providerSpeCount);
        }
    }
}
