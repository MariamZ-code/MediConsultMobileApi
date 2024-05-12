using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundTypeController : ControllerBase
    {
        private readonly IRefundTypeRepository refundTypeRepo;
        private readonly IMemberRepository memberRepo;

        public RefundTypeController(IRefundTypeRepository refundTypeRepo, IMemberRepository memberRepo)
        {
            this.refundTypeRepo = refundTypeRepo;
            this.memberRepo = memberRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetRefundType(string lang)
        {
            if (ModelState.IsValid)
            {


                var refundTypes =await refundTypeRepo.GetAllRefundType();

                var refundTypesEn = new List<RefundTypeEnDTO>();
                var refundTypesAr = new List<RefundTypeArDTO>();
                if (lang == "en")
                {
                    foreach (var refundType in refundTypes)
                    {

                        RefundTypeEnDTO refundTypeEnDTO = new RefundTypeEnDTO
                        {
                            id = refundType.id,
                            en_name = refundType.en_name,
                            notes = refundType.notes,

                        };

                        refundTypesEn.Add(refundTypeEnDTO);

                    }
                    //foreach (var refundType in refundTypesByPolicy)
                    //{
                    //    RefundTypeEnDTO refundTypeEnDTO = new RefundTypeEnDTO
                    //    {
                    //        id = refundType.id,
                    //        en_name = refundType.reimbursementType.en_name,
                    //        notes = refundType.reimbursementType.notes,

                    //    };

                    //    refundTypesEn.Add(refundTypeEnDTO);
                    //}
                    return Ok(refundTypesEn);

                }
                foreach (var refundType in refundTypes)
                {

                    RefundTypeArDTO refundTypeArDTO = new RefundTypeArDTO
                    {
                        id = refundType.id,
                        notes = refundType.notes,

                        ar_name = refundType.ar_name,

                    };

                    refundTypesAr.Add(refundTypeArDTO);


                }
                //foreach (var refundType in refundTypesByPolicy)
                //{
                //    RefundTypeArDTO refundTypeArDTO = new RefundTypeArDTO
                //    {
                //        id = refundType.id,
                //        notes = refundType.notes,

                //        ar_name = refundType.reimbursementType.ar_name,

                //    };

                //    refundTypesAr.Add(refundTypeArDTO);
                //}
                return Ok(refundTypesAr);

            }

            return BadRequest(ModelState);


        }
    }
}
