using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingLabAndScanController : ControllerBase
    {
        private readonly IProviderLocationRepository locationRepo;
        private readonly IServiceDataRepository serviceRepo;
        private readonly ICategoryRepository categoryRepo;
        private readonly ILabAndScanCenterRepository labRepo;
        private readonly IProviderDataRepository providerRepo;
        private readonly IMemberRepository memberRepo;

        public BookingLabAndScanController(IProviderLocationRepository locationRepo, IServiceDataRepository serviceRepo, ICategoryRepository categoryRepo, ILabAndScanCenterRepository labRepo, IProviderDataRepository providerRepo, IMemberRepository memberRepo)
        {
            this.locationRepo = locationRepo;
            this.serviceRepo = serviceRepo;
            this.categoryRepo = categoryRepo;
            this.labRepo = labRepo;
            this.providerRepo = providerRepo;
            this.memberRepo = memberRepo;
        }
        #region GetProvidersForCategory

        [HttpGet("GetProvidersForCategory")]
        public IActionResult GetProvidersForCategory(string lang, [Required] int categoryId, int startPage = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });
            var categoryExsists = categoryRepo.CategoryExsists(categoryId);
            if (!categoryExsists)
                return NotFound(new MessageDto { Message = Messages.CategoryNotFound(lang) });

            var providers = locationRepo.GetProviderLocationsLabAndScan(categoryId);
            var bookingListDto = new List<BookingLabAndScanDTO>();

            foreach (var location in providers)
            {

                var bookingDto = new BookingLabAndScanDTO();


                bookingDto.Location_id = location.location_id;
                bookingDto.Provider_id = location.provider_id;
                bookingDto.Telephone_1 = location.location_telephone_1;
                bookingDto.Telephone_2 = location.location_telephone_2;
                bookingDto.Mobile_1 = location.location_mobile_1;
                bookingDto.Mobile_2 = location.location_mobile_2;
                bookingDto.HotLine = location.hotline;
                bookingDto.Status = location.Provider.provider_status;
                if (lang == "en")
                {
                    bookingDto.Area = location.location_area_en;
                    bookingDto.Address = location.location_address_en;
                    bookingDto.City_Name = location.AppSelectorGovernmentCity.city_name_en;
                    bookingDto.Government_Name = location.AppSelectorGovernmentCity.appSelectorGovernment.government_name_en;
                    bookingDto.Provider_Name = location.Provider.provider_name_en;
                    bookingDto.Category_Name = location.Provider.Category.Category_Name_En;


                }
                else
                {

                    bookingDto.Area = location.location_area_ar;
                    bookingDto.Address = location.location_address_ar;
                    bookingDto.City_Name = location.AppSelectorGovernmentCity.city_name_ar;
                    bookingDto.Government_Name = location.AppSelectorGovernmentCity.appSelectorGovernment.government_name_ar;
                    bookingDto.Provider_Name = location.Provider.provider_name_ar;
                    bookingDto.Category_Name = location.Provider.Category.Category_Name_Ar;


                }


                bookingListDto.Add(bookingDto);

            }
            var totalBooking = bookingListDto.Count();

            bookingListDto = bookingListDto
                     .Skip((startPage - 1) * pageSize)
                     .Take(pageSize).ToList();
            var result = new
            {
                TotalBooking = totalBooking,
                PageNum = startPage,
                PageSize = pageSize,
                ProviderLocations = bookingListDto
            };
            return Ok(result);
        }

        #endregion


        #region ServiceName

        [HttpGet("ServiceName")]
        public IActionResult ServiceName(string? lang, [Required] int categoryId, string? serviceNameFilter, int startPage = 1, int pageSize = 10)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });

            var uniqueServices = labRepo.GetLabAndScanUnique(categoryId);

            if (serviceNameFilter is not null)
            {
                if (lang == "en")
                    uniqueServices = uniqueServices.Where(p => p.Service_name_En.Contains(serviceNameFilter));
                else
                    uniqueServices = uniqueServices.Where(p => p.Service_name_Ar.Contains(serviceNameFilter));

            }
            var serviceNames = new List<UniqueServiceDto>();

            foreach (var service in uniqueServices)
            {

                var serviceName = new UniqueServiceDto
                {
                    Service_id = service.service_id,

                };
                if (lang == "en")
                    serviceName.Service_name = service.Service_name_En;
                else
                    serviceName.Service_name = service.Service_name_Ar;

                serviceNames.Add(serviceName);

            }
            var totalServicename = serviceNames.Count();
            uniqueServices = uniqueServices.Skip((startPage - 1) * pageSize).Take(pageSize);

            var result = new
            {
                TotalServicename = totalServicename,
                PageNum = startPage,
                PageSize = pageSize,
                Data = serviceNames
            };
            return Ok(result);


        }
        #endregion

        #region GetLabAndScanByServiceId
        [HttpGet("GetLabAndScanByServiceId")]
        public IActionResult GetLabAndScanByServiceId(string? lang, [FromQuery] List<int>? serviceIds, int startPage = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });

            if (serviceIds.Count == 0)
                return NotFound(new MessageDto { Message = Messages.EnterServices(lang) });

            foreach (var serviceId in serviceIds)
            {
                var serviceExist = serviceRepo.ServiceExists(serviceId);
                if (!serviceExist)
                {
                    return BadRequest(new MessageDto { Message = Messages.ServicesNotFound(lang) });

                }
            }
            var labAndScans = labRepo.GetLabAndScanCenters(serviceIds);


            var labAndScanDtoDictionary = new Dictionary<int, LabAndScanCentersDTO>();


            foreach (var labAndScan in labAndScans)
            {
                if (!labAndScanDtoDictionary.TryGetValue(labAndScan.provider_id, out var labAndScanDto))
                {
                    labAndScanDto = new LabAndScanCentersDTO()
                    {
                        ProviderId = labAndScan.provider_id,
                        ProviderName = lang == "en" ? labAndScan.provider_name_en : labAndScan.provider_name_ar,
                        ServiceData = new List<ServicesDetailsDTO>(),
                        TotalServicePrice = 0,
                        ServiceIdNotInList = new List<string>()
                    };
                    labAndScanDtoDictionary[labAndScan.provider_id] = labAndScanDto;
                }

                var serviceDetail = new ServicesDetailsDTO
                {
                    Service_id = labAndScan.Service_id,
                    Service_price = labAndScan.Service_price,
                    Service_name = lang == "en" ? labAndScan.Service_name_En : labAndScan.Service_Name_Ar,
                };

                labAndScanDto.ServiceData.Add(serviceDetail);

                labAndScanDto.TotalServicePrice += Convert.ToDecimal(labAndScan.Service_price);
            }

            foreach (var labAndScanDto in labAndScanDtoDictionary.Values)
            {
                foreach (var serviceId in serviceIds)
                {
                    if (labAndScanDto.ServiceData.All(sd => sd.Service_id != serviceId))
                    {
                        var service = labRepo.GetLabAndScanServiceName(serviceId);
                        var servicename = lang == "en" ? service.Service_name_En : service.Service_Name_Ar;
                        var serviceNotExists = lang == "en" ? $"{servicename} not found" : $"{servicename} غير موجودة";
                        labAndScanDto.ServiceIdNotInList.Add(serviceNotExists);

                    }
                }
            }
            var labAndScanListDto = labAndScanDtoDictionary.Values.ToList();


            var totalLabAndScanResult = labAndScanListDto.Count();
            var labAndScanResult = new
            {
                totalLabAndScan = totalLabAndScanResult,
                PageNum = startPage,
                PageSize = pageSize,
                Data = labAndScanListDto
            };
            return Ok(labAndScanResult);

        }


        #endregion

        #region AddNewBooking
        [HttpPost("AddNewBooking")]
        public async Task<IActionResult> AddNewBooking(string? lang, BookingLabAndScanCenterDTO bookingLab)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });
            var providerExists = await providerRepo.ProviderExistsAsync(bookingLab.provider_id);
            var memberExists = memberRepo.MemberExists(bookingLab.member_id);

            if (bookingLab.provider_id is null)
            {
                return BadRequest(new MessageDto { Message = Messages.EnterProvider(lang) });
            }

            if (!providerExists)
            {
                return BadRequest(new MessageDto { Message = Messages.ProviderNotFound(lang) });
            }
            if (bookingLab.member_id is null)
            {
                return BadRequest(new MessageDto { Message = Messages.EnterMember(lang) });
            }

            if (!memberExists)
            {
                return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

            }
            if (bookingLab.serviceIds.Count == 0)
            {
                return BadRequest(new MessageDto { Message = Messages.EnterServices(lang) });
            }
            foreach (var serviceId in bookingLab.serviceIds)
            {
                var serviceExist = serviceRepo.ServiceExists(serviceId);
                if (!serviceExist)
                {
                    return BadRequest(new MessageDto { Message = Messages.ServicesNotFound(lang) });

                }
            }
            var provider = providerRepo.GetProvider(bookingLab.provider_id);

            if (provider.provider_status != "Activated")
            {
                return BadRequest(new MessageDto { Message = Messages.ProviderDeactivated(lang) });

            }

            var serviceIds = new List<BookingService>();
            foreach (var serviceId in bookingLab.serviceIds)
            {
                var service = new BookingService
                {
                    ServiceDataId = serviceId
                };

                serviceIds.Add(service);
            }
            var booking = new BookingLabAndScan
            {
                member_id = bookingLab.member_id,
                provider_id = bookingLab.provider_id,
                date = DateTime.Now.ToString("dd-MM-yyyy"),
                status = "Received",
                service = serviceIds
            };

            labRepo.AddBooking(booking);
            labRepo.Save();

            return Ok(bookingLab);




        }

        #endregion

        #region EditBooking
        [HttpPost("EditBooking")]
        public async Task<IActionResult> EditBooking(string? lang,[Required]int bookingId ,BookingLabAndScanCenterDTO bookingLab)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });
            var bookingExist = labRepo.BookingExist(bookingId);
            var providerExists = await providerRepo.ProviderExistsAsync(bookingLab.provider_id);
            var memberExists = memberRepo.MemberExists(bookingLab.member_id);
            if(!bookingExist)
                return NotFound(new MessageDto { Message = Messages.BookingNotFound(lang) });

            if (bookingLab.provider_id is not null)
            {
                if (!providerExists)
                {
                    return NotFound(new MessageDto { Message = Messages.ProviderNotFound(lang) });
                }
            }

            if (bookingLab.member_id is not null)
            {
                if (!memberExists)
                {
                    return NotFound(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
            }

            if (bookingLab.serviceIds.Count != 0)
            {
                foreach (var serviceId in bookingLab.serviceIds)
                {
                    var serviceExist = serviceRepo.ServiceExists(serviceId);
                    if (!serviceExist)
                    {
                        return NotFound(new MessageDto { Message = Messages.ServicesNotFound(lang) });

                    }
                }
            }


            labRepo.EditBooking(bookingLab, bookingId);
            labRepo.Save();

            return Ok("Updated");

        }

        #endregion
    }
}
