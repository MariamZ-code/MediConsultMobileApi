using FirebaseAdmin.Messaging;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
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
        private readonly IApprovalTimelineRepository appTimelineRepo;
        private readonly IYodawyMedicinsRepository medRepo;

        public ApprovalChronicController(IApprovalRepository approvalRepo, IPharmaApprovalRepository pharmaRepo, IMemberRepository memberRepo, IApprovalLogRepository approvalLogRepo, IAuthRepository authRepo, IMemberProgramRepository programRepo, IApprovalTimelineRepository appTimelineRepo , IYodawyMedicinsRepository medRepo)
        {
            this.approvalRepo = approvalRepo;
            this.pharmaRepo = pharmaRepo;
            this.memberRepo = memberRepo;
            this.approvalLogRepo = approvalLogRepo;
            this.authRepo = authRepo;
            this.programRepo = programRepo;
            this.appTimelineRepo = appTimelineRepo;
            this.medRepo = medRepo;
        }

        #region GetChronic 

        [HttpGet]
        public async Task<IActionResult> GetAllChronic([Required] int memberId, string lang, string? date, string? providerName, int startPage = 1, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var memberExist = memberRepo.MemberExists(memberId);
                if (!memberExist)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });
                }
                var chronics = await approvalRepo.GetAll(memberId);
                if (chronics is null)
                {
                    return NotFound("sothing Wrong");
                }

                var chronicList = new List<ApprovalChronicDTO>();


                if (date is not null && date.Any())
                {
                    for (int i = 0; i < date.Length; i++)
                    {
                        var sta = date[i];
                    }
                    chronics = chronics.Where(c => c.approval_date.Contains(date)).ToList();
                }
                if (providerName is not null && providerName.Any())
                {
                    for (int i = 0; i < providerName.Length; i++)
                    {
                        var sta = providerName[i];
                    }
                    chronics = chronics.Where(c => c.Provider.provider_name_en.Contains(providerName)).ToList();
                }
                foreach (var chronic in chronics)
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

                    var medicionList = new List<Medicines>();
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
                    medicinesList.Clear();
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
        public async Task<IActionResult> InsertChronic([Required] int memberId, string lang, AddChronicApprovalDto approvalDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = authRepo.GetById(memberId);
            var policy = programRepo.GetMemberbyMemberId(memberId);
            var chronicApp = new Approval
            {
                approval_date = DateTime.Now.ToString("dd-MM-yyyy"),
                approval_status = "Received",
                approval_validation_period = "7",
                Approval_User_Id = -1,
                is_claimed = 0,
                price_list_id = -1,
                internal_notes = string.Empty,
                provider_location_id = -1,
                provider_id = -1,
                exceed_pool_id = -1,
                money_for_exceed_note = -1,
                is_pharma = 1,
                is_chronic = 1,
                claim_form_no = "0",
                debit_spent = 0,
                is_repeated = 0,
                inpatient_duration_days = 0,
                doctor_id = -1,
                is_canceld = 0,
                is_re_auth = 0,
                pool_spent = 0,
                pool_child_id = 0,
                general_specality_id = -1,
                Approval_Force_Debit = 0,
                member_id = memberId,
                client_id = member.client_id,
                Client_Branch_id = member.branch_id,
                policy_id = policy.Policy_Id,
                program_id = policy.Program_id,
                dental_comment = string.Empty,
            };

            approvalRepo.AddApproval(memberId, chronicApp);

           var med=await medRepo.GetAllById(approvalDto.act_id);
            var pharmaAct = new PharmaApprovalAct
            {
                Act_Approval_id = chronicApp.approval_id,
                Act_Qty = approvalDto.qty,
                dose = approvalDto.dose,
                unit_name = approvalDto.unit_name,
                Act_id = approvalDto.act_id,
                Act_Discount = 0,
                Act_Discount_Value = 0,
                Act_Copayment_Percentage = 0,
                Act_Copayment_Value = 0,
                Act_Grand_Total = 0,
                Act_Total_Amount = 0,
                Act_Status = "Approved",
                Act_Status_Reason = null,
                

            };

            if (med.unit2_name == "TABLET")
            {
                pharmaAct.Act_Price = Convert.ToDecimal(med.sell_price) / Convert.ToDecimal(med.unit2_count);
            }

            pharmaRepo.InsertPharmaApproval(pharmaAct);

            var appTimeline = new ApprovalTimeline
            {
                action = "requested by member",
                portal_source = " mobile app",
                provider_request_id = -1,
                user_id = -1,
                status = "Received"
            };

            appTimelineRepo.InsertApprovalTimeLine(appTimeline);

            var serverPath = AppDomain.CurrentDomain.BaseDirectory;
            const long maxSizeBytes = 5 * 1024 * 1024;


            if (approvalDto.files.Count == 0)
            {
                return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

            }
            for (int j = 0; j < approvalDto.files.Count; j++)
            {

                if (approvalDto.files[j].Length == 0)
                {
                    return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });
                }
                if (approvalDto.files[j].Length >= maxSizeBytes)
                {
                    return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });
                }
                // image.png --0
                switch (Path.GetExtension(approvalDto.files[j].FileName))
                {
                    case ".pdf":
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                        break;
                    default:
                        return BadRequest(new MessageDto { Message = Messages.FileExtension(lang) });
                }
            }
            var request = requestRepo.AddRequest(requestDto);
            var folder = $"{serverPath}\\MemberPortalApp\\{requestDto.Member_id}\\Approvals\\{request.ID}";

          

            for (int i = 0; i < approvalDto.files.Count; i++)
            {
                if (!Directory.Exists(folder))
                {
                    //Directory.Delete(folder, true); 
                    Directory.CreateDirectory(folder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + approvalDto.files[i].FileName;

                string filePath = Path.Combine(folder, uniqueFileName);


                if (Path.GetExtension(uniqueFileName) != ".pdf")
                {

                    using (var stream = new MemoryStream())
                    {
                        // Read the uploaded image into a MemoryStream
                        approvalDto.files[i].CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);

                        // Load the image using ImageSharp
                        using (var image = SixLabors.ImageSharp.Image.Load(stream))
                        {
                            // Compress the image
                            image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

                            using (var outputStream = new FileStream(filePath, FileMode.Create))
                            {
                                image.Save(outputStream, new JpegEncoder());
                            }
                        }
                    }

                }
                else
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await approvalDto.files[i].CopyToAsync(stream);
                    }
                }

            }



            return Ok(request);

            approvalRepo.Save();




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
