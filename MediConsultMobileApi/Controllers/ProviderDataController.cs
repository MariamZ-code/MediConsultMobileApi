using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderDataController : ControllerBase
    {
        private readonly IProviderDataRepository providerRepo;

        public ProviderDataController(IProviderDataRepository providerRepo)
        {
            this.providerRepo = providerRepo;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll(string lang, string? providerName, int startpage = 1, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var providers = providerRepo.GetProviders();

                if (lang == "en")
                {

                if (providerName is not null)
                {
                    providers = providers.Where(p => p.provider_name_en.Contains(providerName));
                }
                    var totalProvidersEn = providers.Count();

                    providers = providers
                        .OrderBy(e => e.provider_name_en)
                        .Skip((startpage - 1) * pageSize)
                        .Take(pageSize);

                    var providerEn = providers.Select(provider => new ProviderDataEnDTO
                    {
                        Provider_id = provider.provider_id,
                        Provider_name_en = provider.provider_name_en,
                    }).ToList();

                    var resultEn = new
                    {
                        TotalProviders = totalProvidersEn,
                        PageNum = startpage,
                        PageSize = pageSize,
                        Providers = providerEn
                    };

                    return Ok(resultEn);
                }


                if (providerName is not null)
                {
                    providers = providers.Where(p => p.provider_name_ar.Contains(providerName));
                }
                var totalProvidersAr = providers.Count();

                providers = providers
                    .OrderBy(e => e.provider_name_ar)
                    .Skip((startpage - 1) * pageSize)
                    .Take(pageSize);

                var providerAr = providers.Select(provider => new ProviderDataArDTO
                {
                    Provider_id = provider.provider_id,
                    Provider_name_ar = provider.provider_name_ar,
                }).ToList();

                var resultAr = new
                {
                    TotalProviders = totalProvidersAr,
                    PageNum = startpage,
                    PageSize = pageSize,
                    Providers = providerAr
                };

                return Ok(resultAr);
            }

            return BadRequest(ModelState);
        }

    }
}
