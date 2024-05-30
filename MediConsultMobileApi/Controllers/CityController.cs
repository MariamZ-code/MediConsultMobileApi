using FirebaseAdmin.Messaging;
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
    public class CityController : ControllerBase
    {
        private readonly ICityRepository cityRepo;
        private readonly IGovernmentRepository govRepo;

        public CityController(ICityRepository cityRepo , IGovernmentRepository govRepo)
        {
            this.cityRepo = cityRepo;
            this.govRepo = govRepo;
        }

        #region GetCity
        [HttpGet("GetCity")]
        public IActionResult GetCity(string? lang ,[Required] int govId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });

            
            var govExists = govRepo.GovernmentExsists(govId);
            if (!govExists)
            {
                return NotFound(new MessageDto { Message = Messages.GovernmentExists(lang) });
            }


            var cities = cityRepo.GetCity(govId);
            if (lang == "en")
                cities = cities.OrderBy(g => g.city_name_en);
            else
                cities = cities.OrderBy(g => g.city_name_ar);


            var resultCity = new List<CityDTO>();
            foreach (var city in cities)
            {
                var cityDto = new CityDTO
                {
                    city_id = city.city_id,
                };
                if (lang == "en")
                    cityDto.city_name = city.city_name_en;

                else
                    cityDto.city_name = city.city_name_ar;

                resultCity.Add(cityDto);
            }

            return Ok(resultCity);


        }
        #endregion
    }
}
