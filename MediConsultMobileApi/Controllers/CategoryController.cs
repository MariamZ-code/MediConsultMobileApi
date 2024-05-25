using Hangfire;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.HangFire;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepo;
        private readonly HangFireController hangFire;

        public CategoryController(ICategoryRepository categoryRepo , HangFireController hangFire)
        {
            this.categoryRepo = categoryRepo;
            this.hangFire = hangFire;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(string lang)
        {
            if (ModelState.IsValid)
            {
                RecurringJob.AddOrUpdate(() => hangFire.SendNotificationHangFire(lang), Cron.Minutely);
                var categories =  categoryRepo.GetCountOfCategories();
               
                    var categoriesDto = new List<CategoryDTO>();
                    foreach (var category in categories)
                    {
                        CategoryDTO categoryDto = new CategoryDTO
                        {

                            Category_Id = category.Category_Id,
                            Count= category.ProviderCount
                            
                        };
                        if (lang== "en")
                        {
                            categoryDto.Category_Name = category.Category_Name_En.Replace("\t", "");
                            
                        }
                        else
                        {
                            categoryDto.Category_Name = category.Category_Name_Ar.Replace("\t", "");

                        }
                        categoriesDto.Add(categoryDto);
                    }

                    return Ok(categoriesDto);

               
            }
            return BadRequest(ModelState);
        }

    }
}
