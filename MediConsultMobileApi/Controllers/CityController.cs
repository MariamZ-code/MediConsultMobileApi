using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository cityRepo;

        public CityController(ICityRepository cityRepo)
        {
            this.cityRepo = cityRepo;
        }

        #region GetCity
        [HttpGet("GetCity")]
        public IActionResult GetCity(string? lang , int? govId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (lang is null)
                return NotFound(new MessageDto { Message = "Please enter Language" });

            if (govId is null)
            {
                return NotFound(new MessageDto { Message = "Please enter Government Id" });
            }
            var cities = cityRepo.GetCity(govId);

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
