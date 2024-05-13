using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YodawyMedicinsController : ControllerBase
    {
        private readonly IYodawyMedicinsRepository medicinsRepo;

        public YodawyMedicinsController(IYodawyMedicinsRepository medicinsRepo)
        {
            this.medicinsRepo = medicinsRepo;
        }

        [HttpGet("Medicins")]
        public async Task<IActionResult> GetAllMedicins(string lang, int startPage = 1, int endpage = 10)
        {

            if (ModelState.IsValid)
            {
                var medicins =await medicinsRepo.GetAll();

                var totalHistories = medicins.Count();

                medicins = medicins.Skip((startPage-1) * endpage).Take(endpage).ToList();

                var Medicins = new
                {

                    TotalCount = totalHistories,
                    PageNumber = startPage,
                    PageSize = endpage,
                    Requests = medicins,


                };

                return Ok(Medicins);
            }
            return BadRequest(ModelState);

        }
    }
}
