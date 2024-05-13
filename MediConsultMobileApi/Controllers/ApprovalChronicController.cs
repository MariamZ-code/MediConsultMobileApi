using FirebaseAdmin.Messaging;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalChronicController : ControllerBase
    {
        private readonly IApprovalRepository approvalRepo;
        private readonly IPharmaApprovalRepository pharmaRepo;
        private readonly IMemberRepository memberRepo;
        private readonly IApprovalLogRepository approvalLogRepo;
        private readonly IAuthRepository authRepo;
        private readonly IMemberProgramRepository programRepo;

        public ApprovalChronicController(IApprovalRepository approvalRepo, IPharmaApprovalRepository pharmaRepo, IMemberRepository memberRepo, IApprovalLogRepository approvalLogRepo , IAuthRepository authRepo , IMemberProgramRepository programRepo)
        {
            this.approvalRepo = approvalRepo;
            this.pharmaRepo = pharmaRepo;
            this.memberRepo = memberRepo;
            this.approvalLogRepo = approvalLogRepo;
            this.authRepo = authRepo;
            this.programRepo = programRepo;
        }

        #region GetChronic 

        [HttpGet]
        public async Task<IActionResult> GetAllChronic([Required] int memberId, string lang, int startPage = 1, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var memberExist = memberRepo.MemberExists(memberId);
                if (!memberExist)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });
                }
                var chonics = await approvalRepo.GetAll(memberId);
                if (chonics is null)
                {
                    return NotFound("sothing Wrong");
                }
                var chronicList = new List<ApprovalChronicDTO>();
                var medicionList = new List<Medicines>();

                foreach (var chronic in chonics)
                {
                    string url = $"https://hcms.mediconsulteg.com/generate_approval_page.aspx?approval_id={chronic.approval_id}";

                    string file = string.Empty;

                    if (Path.Exists($@"C:\inetpub\wwwroot\hcms_v1\tempFiles\{chronic.approval_id}.pdf"))
                    {
                        file = $"https://hcms.mediconsulteg.com/tempFiles/{chronic.approval_id}.pdf";
                    }
                    else
                    {
                        WebClient client = new WebClient();
                        file = client.DownloadString(new Uri(url));
                        file = file.Replace(@"hcms/C:\inetpub\wwwroot\hcms_v1\", string.Empty);
                        file = ExtractUrl(file);

                    }
                    var medicinesList = pharmaRepo.GetAllByApprovalId(chronic.approval_id);
                    foreach (var medicine in medicinesList)
                    {
                        var med = new Medicines();
                        if (medicine.Act_Status_Reason != -1)
                        {
                            med.unit_name = medicine.unit_name;
                            med.dose = medicine.dose;
                            med.qty = medicine.Act_Qty;
                            med.status = medicine.Act_Status;
                            med.med_name = medicine.YodawyMedicins.name_en;

                            med.reason = medicine.AppActReasons.reason;

                        }
                        else
                        {
                            med.unit_name = medicine.unit_name;
                            med.dose = medicine.dose;
                            med.qty = medicine.Act_Qty;
                            med.status = medicine.Act_Status;
                            med.med_name = medicine.YodawyMedicins.name_en;
                            med.reason = null;

                        }
                        medicionList.Add(med);
                    }
                    var chronicDto = new ApprovalChronicDTO
                    {
                        approval_id = chronic.approval_id,
                        approval_date = chronic.approval_date,
                        approval_status = chronic.approval_status,
                        folderPdf = file,
                        Medicines = medicionList,

                    };
                    if (lang == "en")
                    {
                        chronicDto.provider_name = chronic.Provider.provider_name_en;
                    }
                    else
                    {
                        chronicDto.provider_name = chronic.Provider.provider_name_ar;
                    }

                    chronicList.Add(chronicDto);
                }
                var totalChronic = chronicList.Count();
                chronicList = chronicList.Skip((startPage - 1) * pageSize).Take(pageSize).OrderBy(m => m.approval_id).ToList();

                var chronicResult = new
                {

                    TotalCount = totalChronic,
                    PageNumber = startPage,
                    PageSize = pageSize,
                    Requests = chronicList,


                };
                return Ok(chronicResult);
            }
            return BadRequest(ModelState);
        }

        private string ExtractUrl(string input)
        {
            // Using regular expression to find the URL
            string pattern = @"https://[^\s]+";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                return "URL not found.";
            }
        }
        #endregion

        #region InsertApprovalChronic
        [HttpPost("InsertChronicApproval")]
        public async Task<IActionResult> InsertChronic([Required] int memberId, string lang)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = authRepo.GetById(memberId);
            var policy = programRepo.GetMemberbyMemberId(memberId);
            var chronicApp = new Approval
            {
                approval_date= DateTime.Now.ToString("dd-MM-yyyy"),
                approval_status="Received",
                approval_validation_period="7",
                Approval_User_Id = -1 ,
                is_claimed =0,
                price_list_id=-1,
                internal_notes= string.Empty,
                provider_location_id=-1,
                provider_id=-1,
                exceed_pool_id=-1,
                money_for_exceed_note= -1,
                is_pharma=1,
                is_chronic=1,
                claim_form_no="0" ,
                debit_spent=0,
                is_repeated=0,
                inpatient_duration_days=0,
                doctor_id=-1,
                is_canceld=0,
                is_re_auth=0,
                pool_spent=0,
                pool_child_id=0,
                general_specality_id=-1,
                Approval_Force_Debit=0,
                member_id=memberId,
                client_id=member.client_id,
                Client_Branch_id = member.branch_id,
                policy_id=policy.Policy_Id,
                program_id=policy.Program_id,
                dental_comment=string.Empty,
            };

            approvalRepo.AddApproval(memberId, chronicApp);
            

            return Ok(chronicApp);
        }
        #endregion

        #region Cancel 

        [HttpPost("Canceled")]
        public async Task<IActionResult> DeleteChronicApproval([Required] int approvalId, [Required] string reason, string lang)
        {
            if (ModelState.IsValid)
            {
                var approvalExists = approvalRepo.ApprovalExists(approvalId);
                if (!approvalExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.RefundNotFound(lang) });
                }

                if (reason is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterReason(lang) });
                }

                var approvalLog = new ApprovalLogDto
                {
                    claim_id = approvalId,
                    _event = $"Canceled by member with reason {reason}",
                    user_id = null
                };
                approvalLogRepo.Add(approvalLog);
                approvalRepo.Save();

                approvalRepo.Canceled(approvalId);
                approvalRepo.Save();

                return Ok(new MessageDto { Message = Messages.CanceledApprovalChronic(lang) });
            }
            return BadRequest(ModelState);
        }
        #endregion
    }
}
