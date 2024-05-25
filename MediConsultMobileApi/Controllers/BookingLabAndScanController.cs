﻿using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingLabAndScanController : ControllerBase
    {
        private readonly IProviderLocationRepository locationRepo;
        private readonly ICategoryRepository categoryRepo;

        public BookingLabAndScanController(IProviderLocationRepository locationRepo, ICategoryRepository categoryRepo)
        {
            this.locationRepo = locationRepo;
            this.categoryRepo = categoryRepo;
        }

        [HttpGet("")]
        public IActionResult GetProvidersForCategory(string lang,[Required]int categoryId , int startPage = 1, int pageSize = 10)
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

    }
}