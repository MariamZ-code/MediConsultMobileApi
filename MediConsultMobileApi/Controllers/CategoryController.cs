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
                var categories = await categoryRepo.GetAll();
                if (lang == "en")
                {
                    var categoryEn = new List<CategoryEnDTO>();
                    foreach (var category in categories)
                    {
                        CategoryEnDTO categoryEnDto = new CategoryEnDTO
                        {

                            Category_Id = category.Category_Id,
                            Category_Name_En = category.Category_Name_En,
                        };

                        categoryEn.Add(categoryEnDto);
                    }

                    return Ok(categoryEn);

                }
                var categoryAr = new List<CategoryArDTO>();
                foreach (var category in categories)
                {
                    CategoryArDTO categoryArDto = new CategoryArDTO
                    {

                        Category_Id = category.Category_Id,
                        Category_Name_Ar = category.Category_Name_Ar,
                    };

                    categoryAr.Add(categoryArDto);
                }

                return Ok(categoryAr);
            }
            return BadRequest(ModelState);
        }

    }
}
