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
    public class RatingController : ControllerBase
    {
        private readonly IProviderDataRepository providerRepo;
        private readonly IMemberRepository memberRepo;
        private readonly IProviderRatingRepository ratingRepo;
        public RatingController(IProviderDataRepository providerRepo, IMemberRepository memberRepo, IProviderRatingRepository ratingRepo)
        {

            this.providerRepo = providerRepo;
            this.memberRepo = memberRepo;
            this.ratingRepo = ratingRepo;
        }

        #region GetRatingsByMemberId

        [HttpPost("GetRatingByMemberId")]
        public async Task<IActionResult> GetRatingByMemberId(string lang, [Required] int memberId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
         

            var memberExists = memberRepo.MemberExists(memberId);

            if (!memberExists)
                return NotFound(new MessageDto { Message = Messages.MemberNotFound(lang) });

            var rate = ratingRepo.GetRatingByMemberId(memberId);
            return Ok(rate);

        }

        #region GetRateById

        [HttpPost("GetRateById")]
        public async Task<IActionResult> GetRateById(string lang, [Required] int rateId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rateExists = ratingRepo.RateExists(rateId);

            if (!rateExists)
                return NotFound(new MessageDto { Message = Messages.RateNotExists(lang) });
           var rate = ratingRepo.GetRatingById(rateId);
            return Ok(rate);

        }
        #endregion
        #region AddRating
        [HttpPost("AddRating")]
        public async Task<IActionResult> AddRating(string lang, [FromForm] AddRatingDto rateDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (rateDto.provider_id is null)
                return NotFound(new MessageDto { Message = Messages.EnterProvider(lang) });

            var providerExists = await providerRepo.ProviderExistsAsync(rateDto.provider_id);

            if (!providerExists)
                return NotFound(new MessageDto { Message = Messages.ProviderNotFound(lang) });


            if (rateDto.member_id is null)
                return NotFound(new MessageDto { Message = Messages.EnterMember(lang) });

            var memberExists = memberRepo.MemberExists(rateDto.member_id);

            if (!memberExists)
                return NotFound(new MessageDto { Message = Messages.MemberNotFound(lang) });


            if (rateDto.rate is null)
                return NotFound(new MessageDto { Message = Messages.EnterRate(lang) });

            var rate = new ProviderRating
            {
                rate = rateDto.rate,
                member_id = rateDto.member_id,
                provider_id = rateDto.provider_id
            };
            ratingRepo.AddRate(rate);
            ratingRepo.Save();

            return Ok(new MessageDto { Message = Messages.AddrRate(lang) });


        }
        #endregion

        #region EditRating
        [HttpPost("EditRating")]
        public async Task<IActionResult> EditRating(string lang, [Required] int rateId, [FromForm] AddRatingDto rateDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rateExists = ratingRepo.RateExists(rateId);

            if (!rateExists)
                return NotFound(new MessageDto { Message = Messages.RateNotExists(lang) });



            if (rateDto.provider_id is null)
                return NotFound(new MessageDto { Message = Messages.EnterProvider(lang) });

            var providerExists = await providerRepo.ProviderExistsAsync(rateDto.provider_id);

            if (!providerExists)
                return NotFound(new MessageDto { Message = Messages.ProviderNotFound(lang) });


            if (rateDto.member_id is null)
                return NotFound(new MessageDto { Message = Messages.EnterMember(lang) });

            var memberExists = memberRepo.MemberExists(rateDto.member_id);

            if (!memberExists)
                return NotFound(new MessageDto { Message = Messages.MemberNotFound(lang) });


            if (rateDto.rate is null)
                return NotFound(new MessageDto { Message = Messages.EnterRate(lang) });
            var rate = ratingRepo.GetRatingById(rateId);


            rate.member_id = rateDto.member_id;
            rate.rate = rateDto.rate;
            rate.provider_id = rateDto.provider_id;



            ratingRepo.UpdateRate(rate);
            ratingRepo.Save();

            return Ok(new MessageDto { Message = Messages.Updated(lang) });


        }
        #endregion

        #region DeleteRating
        [HttpPost("DeleteRating")]
        public async Task<IActionResult> DeleteRating(string lang, [Required] int rateId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rateExists = ratingRepo.RateExists(rateId);

            if (!rateExists)
                return NotFound(new MessageDto { Message = Messages.RateNotExists(lang) });
            ratingRepo.DeleteRate(rateId);
            ratingRepo.Save();
            return Ok(new MessageDto { Message = Messages.Deleted(lang) });

        }
        #endregion
    }
}

