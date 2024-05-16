using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IRefundRepository refundRepo;
        private readonly IMemberRepository memberRepo;
        ApplicationDbContext context;

        public RefundController(IRefundRepository refundRepo, IMemberRepository memberRepo, ApplicationDbContext context)
        {
            this.refundRepo = refundRepo;
            this.memberRepo = memberRepo;
            this.context = context;
        }

        #region AddNewRefund
        [HttpPost("AddNewRefund")]
        public async Task<IActionResult> PostRefund([FromForm] RefundDTO refundDto, [FromForm] List<IFormFile> files, string lang)
        {

            const long maxSizeBytes = 5 * 1024 * 1024;
            if (ModelState.IsValid)
            {
                var memberExists = memberRepo.MemberExists(refundDto.member_id);
                //var refundExists = refundRepo.RefundExists(refundDto.refund_type_id);
                var refundTypeExists = refundRepo.RefundTypeExists(refundDto.refund_type_id);

                if (refundDto.notes is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterNotes(lang) });
                }
                if (refundDto.member_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterMember(lang) });
                }

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (!refundTypeExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.RefundTypeNotFound(lang) });
                }
                if (refundDto.amount is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.AmountNotFound(lang) });
                }
                if (refundDto.refund_type_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterRefund(lang) });
                }
                if (refundDto.refund_date is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterRefundDate(lang) });
                }

                if (refundDto.refund_reason is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterReason(lang) });
                }
                var serverPath = AppDomain.CurrentDomain.BaseDirectory;
                if (files.Count == 0)
                {
                    return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                }
                for (int j = 0; j < files.Count; j++)
                {

                    if (files[j].Length == 0)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });
                    }
                    if (files[j].Length >= maxSizeBytes)
                    {
                        return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });
                    }
                    // image.png --0
                    switch (Path.GetExtension(files[j].FileName))
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
                var refund = refundRepo.AddRefund(refundDto);

                var folder = Path.Combine(serverPath, "MemberPortalApp", refundDto.member_id.ToString(), "Refund", refund.id.ToString());

                for (int i = 0; i < files.Count; i++)
                {
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + files[i].FileName;

                    string filePath = Path.Combine(folder, uniqueFileName);


                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await files[i].CopyToAsync(stream);
                    }
                }

                DateTime refundDate = DateTime.ParseExact(refundDto.refund_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime nowDate = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);

                int daysDifference = (int)(nowDate - refundDate).TotalDays;
                var memberName = memberRepo.GetByID(refundDto.member_id).Result.member_name;
                if (daysDifference >= 60)
                {
                    refund.Status = "Rejected";
                    refund.reject_reason = $"{Messages.RefundDateIncorrect(lang, memberName)}";
                    refundRepo.Save();

                    return BadRequest(new MessageDto { Message = Messages.RefundDateIncorrect(lang, memberName) });
                }
                refundRepo.Save();
                return Ok(refund);

            }
            return BadRequest(ModelState);


        }
        #endregion


        #region Edit
        [HttpPost("UpdateRefund")]
        public async Task<IActionResult> EditRequest(int refundId, string lang, [FromForm] UpdateRefundDTO refundDto)
        {
            if (ModelState.IsValid)
            {
                const long maxSizeBytes = 5 * 1024 * 1024;
                var refExists = refundRepo.RefundExists(refundId);
                var refTypeExists = refundRepo.RefundTypeExists(refundDto.refund_type_id);
                if (!refExists)
                {
                    return NotFound(new MessageDto { Message = Messages.RefundNotFound(lang) });
                }
                var refund = refundRepo.GetById(refundId);
                if (refund is null)
                {
                    return NotFound(new MessageDto { Message = Messages.RefundNotFound(lang) });
                }

                if (refund.Status != "Received" && refund.Status != "OnHold")
                {
                    return BadRequest(new MessageDto { Message = Messages.RequestEdit(lang) });
                }

                var memberExists = memberRepo.MemberExists(refundDto.member_id);
             

                if (refundDto.notes is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterNotes(lang) });
                }
                if (refundDto.member_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterMember(lang) });
                }

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }

                if (refundDto.amount is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.AmountNotFound(lang) });
                }
                if (refundDto.refund_type_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterRefund(lang) });
                }
                if (!refTypeExists)
                {
                    return NotFound(new MessageDto { Message = Messages.RefundTypeNotFound(lang) });
                }
                if (refundDto.refund_date is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterRefundDate(lang) });
                }
                if (refundDto.refund_reason is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterReason(lang) });
                }
                var serverPath = AppDomain.CurrentDomain.BaseDirectory;
                //if (refundDto.Photos is null)
                //{
                //    return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                //}
                //if (refundDto.Photos.Count == 0)
                //{
                //    return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                //}
                //var folder = Path.Combine(serverPath, "MemberPortalApp", refundDto.member_id.ToString(), "Refund", refund.id.ToString());


                var folder = $"{serverPath}\\MemberPortalApp\\{refundDto.member_id}\\Refund\\{refund.id}";

                DirectoryInfo dir = new DirectoryInfo(folder);

                if (refundDto.DeletePhotos is not null)
                {
                    foreach (var file in refundDto.DeletePhotos)
                    {
                        string convertedPath = file.Replace("\\\\", "\\");
                        foreach (var existingfile in dir.GetFiles())
                        {
                            if (convertedPath == existingfile.ToString())
                            {
                                existingfile.Delete();
                            }

                        }
                    }
                }

               
                    if (refundDto.Photos is not null)
                {
                    for (int j = 0; j < refundDto.Photos.Count; j++)
                    {

                        if (refundDto.Photos[j].Length >= maxSizeBytes)
                        {
                            return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });
                        }
                        // image.png --0
                        switch (Path.GetExtension(refundDto.Photos[j].FileName))
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

                    for (int i = 0; i < refundDto.Photos.Count; i++)
                    {
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + refundDto.Photos[i].FileName;

                        string filePath = Path.Combine(folder, uniqueFileName);


                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            await refundDto.Photos[i].CopyToAsync(stream);
                        }
                    }

                }
                refundRepo.EditRefund(refundDto, refundId);

                DateTime refundDate = DateTime.ParseExact(refundDto.refund_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                DateTime nowDate = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);

                int daysDifference = (int)(nowDate - refundDate).TotalDays;
                var memberName = memberRepo.GetByID(refundDto.member_id).Result.member_name;
                if (daysDifference >= 60)
                {
                    refund.Status = "Rejected";
                    refundRepo.Save();
                    return BadRequest(new MessageDto { Message = Messages.RefundDateIncorrect(lang, memberName) });
                }
                refundRepo.Save();

                return Ok(new MessageDto { Message = Messages.Updated(lang) });

            }
            return BadRequest(ModelState);
        }
        #endregion


        #region RefundByMemberId
        [HttpGet("HistoryRefund")]

        public IActionResult GetbyMemberId(string lang, [Required] int memberId, [FromQuery] string? startDate, [FromQuery] string? endDate, [FromQuery] string[]? status, [FromQuery] string[]? refundTypes, [FromQuery] int startpage = 1, [FromQuery] int pageSize = 10)
        {
            if (ModelState.IsValid)
            {

                var refunds = refundRepo.GetRefundByMemberId(memberId);
                var refDto = new List<RefundDetailsForMemberDTO>();
                var memberExist = memberRepo.MemberExists(memberId);
                //var refundExsit = refundRepo.

                if (!memberExist)
                {
                    return NotFound(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (refunds is null)
                {
                    return NotFound(new MessageDto { Message = Messages.RefundNotFound(lang) });

                }


                if (status != null && status.Any())
                {
                    for (int i = 0; i < status.Length; i++)
                    {
                        var sta = status[i];
                    }
                    refunds = refunds.Where(c => status.Contains(c.Status));
                }

                if (endDate is not null && startDate is null)
                {
                    return NotFound(new MessageDto { Message = "Enter start Date" });
                }

                if (endDate is null && startDate is not null)
                {
                    return NotFound(new MessageDto { Message = "Enter end Date" });
                }

                if (endDate is not null || startDate is not null)
                {

                    DateTime startDat = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime endDat = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    refunds = refunds
                         .AsEnumerable().Where(entity =>
                               DateTime.TryParseExact(entity.created_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var entityDate) &&
                               entityDate >= startDat &&
                               entityDate <= endDat)
                        .AsQueryable();
                }
                if (refundTypes != null && refundTypes.Any())
                {
                    for (int i = 0; i < refundTypes.Length; i++)
                    {
                        var provider = refundTypes[i];
                        refunds = refunds.Where(r => r.refundTypes.en_name.Contains(provider));
                    }


                }
                var totalProviders = refunds.Count();
                refunds = refunds.Skip((startpage - 1) * pageSize).Take(pageSize);

                foreach (var refund in refunds)
                {
                    string url = $"https://hcms.mediconsulteg.com/generate_refund_page.aspx?refund_id={refund.refund_id}";

                    string file = string.Empty;
                    if (refund.refund_id is not null)
                    {
                        if (Path.Exists($@"C:\inetpub\wwwroot\hcms_v1\tempFiles\Refund{refund.refund_id}.pdf"))
                        {
                            file = $"https://hcms.mediconsulteg.com/tempFiles/Refund{refund.refund_id}.pdf";
                        }
                        else
                        {
                            WebClient client = new WebClient();
                            file = client.DownloadString(new Uri(url));
                                if (file.StartsWith("404"))
                                {
                                    file = string.Empty;
                            }
                            else
                            {

                            file = file.Replace(@"C:\inetpub\wwwroot\hcms_v1\", string.Empty);
                            file = ExtractUrl(file);
                            }

                        }
                    }
                    RefundDetailsForMemberDTO refDetalisDto = new RefundDetailsForMemberDTO
                    {

                        Id = refund.id,
                        CreatedDate = refund.created_date,
                        RefundDate = refund.refund_date,
                        Status = refund.Status,
                        Amount = refund.total_amount,
                        Note = refund.notes,
                        refund_reason = refund.refund_reason,
                        RefundPDF = file,
                        RefundId= refund.refund_id,
                        reject_reason = refund.reject_reason,


                    };
                    if (lang == "en")
                    {
                        refDetalisDto.RefundType = refund.refundTypes?.en_name;
                    }
                    else
                    {
                        refDetalisDto.RefundType = refund.refundTypes?.ar_name;
                    };

                    refDto.Add(refDetalisDto);
                }

                var medicalDto = new
                {

                    TotalCount = totalProviders,
                    PageNumber = startpage,
                    PageSize = pageSize,
                    Refund = refDto,


                };

                return Ok(medicalDto);


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

        #region RefundByRefundId
        [HttpGet("RefundDetails")]

        public async Task<IActionResult> GetResultByID([Required] int id, string lang)
        {
            if (ModelState.IsValid)
            {
                var refExists = refundRepo.RefundExists(id);
                if (!refExists)
                {
                    return NotFound(new MessageDto { Message = Messages.RefundNotFound(lang) });
                }
                var refund = refundRepo.GetById(id);
                string[] fileNames = Directory.GetFiles(refund.folder_path);
                List<string> fileNameList = fileNames.ToList();

                if (refund is null)
                {
                    return NotFound(new MessageDto { Message = Messages.RefundNotFound(lang) });
                }
                if (!Directory.Exists(refund.folder_path))
                {
                    return BadRequest(new MessageDto { Message = "Invalid folder path" });
                }
                var reqDto = new RefundDetailsDTO
                {
                    Id = refund.id,
                    RefundId = refund.refund_id,
                    Refund_date = refund.refund_date,
                    Notes = refund.notes,
                    refund_reason = refund.refund_reason,
                    reject_reason = refund.reject_reason,

                    FolderPath = fileNameList,
                    Amount = refund.total_amount

                };
           
                switch (refund.Status)
                {
                    case "Received":
                    case "OnHold":
                        reqDto.Allow_Edit = true;

                        break;
                    default:
                        reqDto.Allow_Edit = false;
                        break;
                }
                if (lang == "en")
                {
                    reqDto.RefundType = refund.refundTypes?.en_name;
                }
                else
                {
                    reqDto.RefundType = refund.refundTypes?.ar_name;
                };
                return Ok(reqDto);

            }
            return BadRequest(ModelState);
        }
        #endregion

    }
}
