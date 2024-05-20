using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalNetworkController : ControllerBase
    {
        private readonly IMedicalNetworkRepository medicalRepo;
        private readonly IProviderLocationRepository locationRepo;

        public MedicalNetworkController(IMedicalNetworkRepository medicalRepo, IProviderLocationRepository locationRepo)
        {
            this.medicalRepo = medicalRepo;
            this.locationRepo = locationRepo;
        }
        [HttpGet]

        public IActionResult MedicalNetwork([FromQuery] string? providerName, string lang, [FromQuery] string[]? categories, [FromQuery] string? gov, [FromQuery] string? city, int StartPage = 1, int pageSize = 10)
        {

            if (ModelState.IsValid)
            {
                var medicalNets = medicalRepo.GetAll();
                var medicalNetEn = new List<MedicalNetworkEnDTO>();
                var medicalNetAr = new List<MedicalNetworkArDTO>();

                if (lang == "en")
                {
                    if (providerName is not null)
                    {
                        medicalNets = medicalNets.Where(x => x.provider_name_en.Contains(providerName));
                    }
                    if (gov is not null)
                    {
                        medicalNets = medicalNets.Where(x => x.government_name_en.Contains(gov));
                    }
                    if (city is not null)
                    {
                        medicalNets = medicalNets.Where(x => x.city_name_en.Contains(city));
                    }
                    if (categories != null && categories.Any())
                    {
                        for (int i = 0; i < categories.Length; i++)
                        {
                            var status = categories[i];
                        }
                        medicalNets = medicalNets.Where(c => categories.Contains(c.Category_Name_En));
                    }

                    var totalCount = medicalNets.Count();
                    medicalNets = medicalNets.Skip((StartPage - 1) * pageSize).Take(pageSize);
                    if (medicalNets == null)
                    {
                        return BadRequest(new MessageDto { Message = Messages.MedicalNetwork(lang) }); 
                    }

                    var medicalNetList = medicalNets.ToList();
                    foreach (var medicalNet in medicalNetList)
                    {
                        if (!string.IsNullOrEmpty(medicalNet.Latitude) && !string.IsNullOrEmpty(medicalNet.Longitude) &&
                            !medicalNet.Latitude.Contains(",") && !medicalNet.Longitude.Contains(","))
                        {
                            var address = locationRepo.GetProviderLocationsByProviderId(medicalNet.provider_id, medicalNet.location_id);

                            var providerOnline = medicalRepo.GetByProviderId(medicalNet.provider_id);
                            

                            MedicalNetworkEnDTO medicalNetEnDto = new MedicalNetworkEnDTO
                            {

                                Category = medicalNet.Category_Name_En,
                                providerName = medicalNet.provider_name_en,
                                Latitude = medicalNet.Latitude,
                                Longitude = medicalNet.Longitude,
                                Email = medicalNet.Email,
                                Hotline = medicalNet.Hotline,
                                Mobile = medicalNet.Mobile,
                                Telephone = medicalNet.Telephone,
                                Government = medicalNet.government_name_en,
                                City = medicalNet.city_name_en,
                                SpecialtyName = medicalNet.General_Specialty_Name_En,
                                ProviderAddress = address.location_address_en,
                                
                            };
                            if (providerOnline is null)
                            {
                                medicalNetEnDto.Is_online = 0;
                            }
                            else
                            {
                                medicalNetEnDto.Is_online = providerOnline.is_enabled;
                            }
                            medicalNetEn.Add(medicalNetEnDto);
                        }


                    }
                    var medicalEnDto = new
                    {

                        TotalCount = totalCount,
                        PageNumber = StartPage,
                        PageSize = pageSize,
                        MedicalNetwork = medicalNetEn,


                    };


                    return Ok(medicalEnDto);

                }


                if (providerName is not null)
                {
                    medicalNets = medicalNets.Where(x => x.provider_name.Contains(providerName));
                }
                if (gov is not null)
                {
                    medicalNets = medicalNets.Where(x => x.government_name_ar.Contains(gov));
                }
                if (city is not null)
                {
                    medicalNets = medicalNets.Where(x => x.city_name_ar.Contains(city));
                }
                if (categories != null && categories.Any())
                {
                    for (int i = 0; i < categories.Length; i++)
                    {
                        var cat = categories[i];
                    }
                    medicalNets = medicalNets.Where(c => categories.Contains(c.Category));
                }


                var totalCountt = medicalNets.Count();
                medicalNets = medicalNets.Skip((StartPage - 1) * pageSize).Take(pageSize);
                if (medicalNets == null) { return BadRequest(new MessageDto { Message = Messages.MedicalNetwork(lang) }); }
                var medicalNetListAR = medicalNets.ToList();

                foreach (var medicalNet in medicalNetListAR)
                {
                    if (!string.IsNullOrEmpty(medicalNet.Latitude) && !string.IsNullOrEmpty(medicalNet.Longitude) &&
                            !medicalNet.Latitude.Contains(",") && !medicalNet.Longitude.Contains(","))
                    {
                        var address = locationRepo.GetProviderLocationsByProviderId(medicalNet.provider_id, medicalNet.location_id);
                        MedicalNetworkArDTO medicalNetArDto = new MedicalNetworkArDTO
                        {

                            Category = medicalNet.Category,
                            providerName = medicalNet.provider_name,
                            Latitude = medicalNet.Latitude,
                            Longitude = medicalNet.Longitude,
                            Email = medicalNet.Email,
                            Hotline = medicalNet.Hotline,
                            Mobile = medicalNet.Mobile,
                            Telephone = medicalNet.Telephone,
                            Government = medicalNet.government_name_ar,
                            City = medicalNet.city_name_ar,
                            SpecialtyName = medicalNet.Speciality,
                            ProviderAddress = address.location_address_ar
                        };
                        medicalNetAr.Add(medicalNetArDto);
                    }


                }
                var medicalArDto = new
                {

                    TotalCount = totalCountt,
                    PageNumber = StartPage,
                    PageSize = pageSize,
                    MedicalNetwork = medicalNetAr,


                };


                return Ok(medicalArDto);
            }
            return BadRequest(ModelState);
        }
    }
}
