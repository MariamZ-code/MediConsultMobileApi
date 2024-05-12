using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IParentServiceRepository parentServiceRepo;
        private readonly IServiceRepository serviceRepo;
        private readonly IMemberRepository memberRepo;
        private readonly IPolicyInfoRepository infoRepo;
        private readonly IServiceCopaymentRepository serCpProviderRepo;

        public ServiceController(IParentServiceRepository parentServiceRepo, IServiceRepository serviceRepo, IMemberRepository memberRepo, IPolicyInfoRepository infoRepo, IServiceCopaymentRepository serCpProviderRepo)
        {
            this.parentServiceRepo = parentServiceRepo;
            this.serviceRepo = serviceRepo;
            this.memberRepo = memberRepo;
            this.infoRepo = infoRepo;
            this.serCpProviderRepo = serCpProviderRepo;
        }


        [HttpGet("GetPolicy")]
        public async Task<ActionResult> Get(int memberId, string lang)
        {
            if (ModelState.IsValid)
            {

                var ServicesDto = new List<ServiceDTO>();
                var serviceDatasDto = new List<ServiceDataDto>();
                //var serviceNotesDto = new List<Notes>();
                var member = await memberRepo.GetByID(memberId);
                var services = await serviceRepo.GetById(member);

                foreach (var service in services)
                {
                    var servicess = parentServiceRepo.Get(service.Service_Class_Id);

                    foreach (var item in servicess)
                    {
                        var serviceDto = new ServiceDTO();

                        if (string.IsNullOrEmpty(item.Parent_ServiceName_En))
                        {
                            if (lang == "en")
                            {
                                serviceDto.parentServiceName_En = item.service_name_En;

                            }
                            else
                            {
                                serviceDto.parentServiceName_En = item.service_name_Ar;

                            }
                            serviceDto.ParentImage = item.parent_photo;
                            serviceDto.copyment = service.copayment.ToString() + "%" ;
                            serviceDto.Infos = new List<ServiceDataDto>();

                        }
                        else
                        {
                            if (lang == "en")
                            {
                                serviceDto.parentServiceName_En = item.Parent_ServiceName_En;

                            }
                            else
                            {
                                serviceDto.parentServiceName_En = item.Parent_ServiceName_Ar;

                            }
                            serviceDto.ParentImage = item.parent_photo;
                            serviceDto.copyment = service.copayment.ToString() + "%" ;
                            serviceDto.Infos = new List<ServiceDataDto>();
                        }
                        Notes serviceNote = new Notes();
                        var serviceCops = serCpProviderRepo.GetServices(memberId);

                        foreach (var serviceCop in serviceCops)
                        {
                            if (item.Parent_id == serviceCop.Service_Class_Parent_Id)
                            {
                                if (service.Service_Class_Id == serviceCop.Service_Class_id)
                                {
                                    if (lang == "en")
                                    {

                                        if (serviceCop.copayment_percent is not null && serviceCop.provider_name_en is not null)
                                        {
                                            var notes = $"{serviceCop.provider_name_en} : {serviceCop.copayment_percent}%";
                                            serviceNote.note.Add(notes);
                                        }
                                       

                                            if (serviceCop.SL_Limit_Type == "Limited")
                                            {
                                                var noteLimit = $"{service.service_nameEn} Limit : {service.SL_Limit}";
                                                serviceNote.note.Add(noteLimit);

                                            }
                                            if (serviceCop.SL_Service_Count != 0)
                                            {
                                                if (service.Service_Class_Id == 52)
                                                {
                                                    var noteCount = $"{service.service_nameEn} sessions : {service.SL_Service_Count}";
                                                    serviceNote.note.Add(noteCount);
                                                }
                                                else
                                                {
                                                    var noteCount = $"{service.service_nameEn} per year : {service.SL_Service_Count}";
                                                    serviceNote.note.Add(noteCount);
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(service.notes))
                                            {
                                                var noteService = $"{service.notes}";

                                                serviceNote.note.Add(noteService);
                                            }
                                        

                                        

                                    }
                                    else
                                    {
                                        if (serviceCop.copayment_percent is not null && serviceCop.provider_name_ar is not null)
                                        {
                                            var notes = $"%{serviceCop.provider_name_ar} : {serviceCop.copayment_percent}%";
                                            serviceNote.note.Add(notes);
                                        }


                                        if (service.SL_Limit_Type == "Limited")
                                        {
                                            var noteLimit = $"الحد الاقصي لل{service.service_nameAr}  : {service.SL_Limit}";
                                            serviceNote.note.Add(noteLimit);

                                        }
                                        if (service.SL_Service_Count != 0)
                                        {
                                            if (service.Service_Class_Id == 52)
                                            {
                                                var noteCount = $"جلسات {service.service_nameAr}  : {service.SL_Service_Count}";
                                                serviceNote.note.Add(noteCount);
                                            }
                                            else
                                            {
                                                var noteCount = $"{service.service_nameAr} خلال السنة : {service.SL_Service_Count}";
                                                serviceNote.note.Add(noteCount);
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(service.notes))
                                        {
                                            var noteService = $"{service.notes}";
                                            serviceNote.note.Add(noteService);
                                        }
                                    }

                                }


                            }
                        }



                        var serviceData = new ServiceDataDto
                        {

                            service_id = item.service_id,
                            Notes = serviceNote,
                            Photo = item.photo,
                            copyment = service.copayment.ToString() +"%",

                        };

                        if (lang == "en")
                        {
                            serviceData.Service_Name_En = item.service_name_En;
                        }
                        else
                        {
                            serviceData.Service_Name_En = item.service_name_Ar;
                        }
                        if (!serviceDto.Infos.Any(sd => sd.service_id == serviceData.service_id))
                        {
                            serviceDatasDto.Add(serviceData);
                            serviceDto.Infos.Add(serviceData);  // Add serviceData to Infos
                        }

                        ServicesDto.Add(serviceDto);
                    }
                }
                var mergedData = ServicesDto
                .GroupBy(x => x.parentServiceName_En)
                .Select(group => new ServiceDTO
                {
                    parentServiceName_En = group.Key,
                    ParentImage = group.First().ParentImage,
                    copyment = group.First().copyment,
                    Infos = group.SelectMany(x => x.Infos).ToList()
                })
                .ToList();
                return Ok(mergedData);
            }

            return BadRequest(ModelState);

        }



    }
}
