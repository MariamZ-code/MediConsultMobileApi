using iText.Kernel.Pdf;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
//using iText.Kernel.Pdf;
//using iText.Kernel.Pdf.Writer;
namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository requestRepo;
        private readonly IProviderDataRepository providerRepo;
        private readonly IMemberRepository memberRepo;


        public RequestController(IRequestRepository requestRepo, IProviderDataRepository providerRepo, IMemberRepository memberRepo)
        {
            this.requestRepo = requestRepo;
            this.providerRepo = providerRepo;
            this.memberRepo = memberRepo;

        }

        #region AddNewRequest
        [HttpPost]

        public async Task<IActionResult> PostRequest([FromForm] RequestDTO requestDto, [FromForm] List<IFormFile> files, string lang)
        {

            if (ModelState.IsValid)
            {

                const long maxSizeBytes = 5 * 1024 * 1024;
                var providerExists = await providerRepo.ProviderExistsAsync(requestDto.Provider_id);
                var memberExists = memberRepo.MemberExists(requestDto.Member_id);
                if (requestDto.Notes is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterNotes(lang) });
                }
                if (requestDto.Provider_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterProvider(lang) });
                }
               
                if (!providerExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.ProviderNotFound(lang) });
                }
                if (requestDto.Member_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterMember(lang) });
                }

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (requestDto.Is_chronic is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterIsChronic(lang) });
                }
                var provider = providerRepo.GetProvider(requestDto.Provider_id);

                if (provider.provider_status != "Activated")
                {
                    return BadRequest(new MessageDto { Message = Messages.ProviderDeactivated(lang) });

                }
                if (requestDto.Is_chronic is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterIsChronic(lang) });
                }
                if (requestDto.Is_chronic != 0 || requestDto.Is_chronic != 1)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterIsChronic(lang) });

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
                var request = requestRepo.AddRequest(requestDto);
                var folder = $"{serverPath}\\MemberPortalApp\\{requestDto.Member_id}\\Approvals\\{request.ID}";

                //var folder = Path.Combine(serverPath, "MemberPortalApp", requestDto.Member_id.ToString(), "Approvals", request.ID.ToString());

                for (int i = 0; i < files.Count; i++)
                {
                    if (!Directory.Exists(folder))
                    {
                        //Directory.Delete(folder, true); 
                        Directory.CreateDirectory(folder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + files[i].FileName;

                    string filePath = Path.Combine(folder, uniqueFileName);


                    if (Path.GetExtension(uniqueFileName) != ".pdf")
                    {

                        using (var stream = new MemoryStream())
                        {
                            // Read the uploaded image into a MemoryStream
                            files[i].CopyTo(stream);
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
                            await files[i].CopyToAsync(stream);
                        }
                    }

                }



                return Ok(request);

            }
            return BadRequest(ModelState);


        }

        #endregion

        #region EditRequest
        [HttpPost("UpdateRequest")]
        public async Task<IActionResult> EditRequest(int requestId, string lang, [FromForm] UpdateRequestDTO requestDto)
        {
            if (ModelState.IsValid)
            {
                const long maxSizeBytes = 5 * 1024 * 1024;
                var reqExists = requestRepo.RequestExists(requestId);
                if (!reqExists)
                {
                    return NotFound(new MessageDto { Message = Messages.RequestNotFound(lang) });
                }
                var request = requestRepo.GetById(requestId);
                if (request is null)
                {
                    return NotFound(new MessageDto { Message = Messages.RequestNotFound(lang) });
                }

                if (request.Status != "Received" && request.Status != "OnHold")
                {
                    return BadRequest(new MessageDto { Message = Messages.RequestEdit(lang) });
                }

                var providerExists = await providerRepo.ProviderExistsAsync(requestDto.Provider_id);
                var memberExists = memberRepo.MemberExists(requestDto.Member_id);
                if (requestDto.Notes is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterNotes(lang) });
                }
                if (requestDto.Provider_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterProvider(lang) });
                }
                if (!providerExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.ProviderNotFound(lang) });
                }
                if (requestDto.Member_id is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterMember(lang) });
                }

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (requestDto.Is_chronic is null  )
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterIsChronic(lang) });
                }
                if (requestDto.Is_chronic != 0 || requestDto.Is_chronic != 1)
                {
                    return BadRequest(new MessageDto { Message = Messages.EnterIsChronic(lang) });

                }
                var serverPath = AppDomain.CurrentDomain.BaseDirectory;

                var folder = $"{serverPath}\\MemberPortalApp\\{requestDto.Member_id}\\Approvals\\{request.ID}";

                DirectoryInfo dir = new DirectoryInfo(folder);

                if (requestDto.DeletePhotos is not null)
                {
                    foreach (var file in requestDto.DeletePhotos)
                    {
                        foreach (var existingfile in dir.GetFiles())
                        {
                            string convertedPath = file.Replace("\\\\", "\\");
                            if (convertedPath == existingfile.ToString())
                            {
                                existingfile.Delete();
                            }

                        }
                    }
                }

                if (requestDto.Photos is not null)
                {
                    for (int i = 0; i < requestDto.Photos.Count; i++)
                    {

                        if (requestDto.Photos[i].Length == 0)
                        {
                            return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });
                        }
                        if (requestDto.Photos[i].Length >= maxSizeBytes)
                        {
                            return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });
                        }
                        switch (Path.GetExtension(requestDto.Photos[i].FileName))
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



                    for (int i = 0; i < requestDto.Photos.Count; i++)
                    {
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + requestDto.Photos[i].FileName;

                        string filePath = Path.Combine(folder, uniqueFileName);


                        if (Path.GetExtension(uniqueFileName) != ".pdf")
                        {

                            using (var stream = new MemoryStream())
                            {
                                // Read the uploaded image into a MemoryStream
                                requestDto.Photos[i].CopyTo(stream);
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
                                await requestDto.Photos[i].CopyToAsync(stream);
                            }
                        }



                    }

                }
                        requestRepo.EditRequest(requestDto, requestId);

                        return Ok(new MessageDto { Message = Messages.Updated(lang) });
            }
            return BadRequest(ModelState);
        }
        #endregion


        #region RequestByMemberId
        [HttpGet("MemberId")]

        public async Task<IActionResult> GetbyMemberId(string lang, [Required] int memberId, [FromQuery] string? startDate, [FromQuery] string? endDate, [FromQuery] string[]? status, [FromQuery] string[]? providers, [FromQuery] int startpage = 1, [FromQuery] int pageSize = 10)
        {
            if (ModelState.IsValid)
            {

                var requests = requestRepo.GetRequestsByMemberId(memberId);
                var reqDto = new List<RequestDetailsForMemberDTO>();
                var memberExist = memberRepo.MemberExists(memberId);

                if (!memberExist)
                {
                    return NotFound(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                if (requests is null)
                {
                    return NotFound(new MessageDto { Message = Messages.RequestNotFound(lang) });

                }


                if (status != null && status.Any())
                {
                    for (int i = 0; i < status.Length; i++)
                    {
                        var sta = status[i];
                    }
                    requests = requests.Where(c => status.Contains(c.Status));
                }
                if (providers != null && providers.Any())
                {
                    for (int i = 0; i < providers.Length; i++)
                    {
                        var provider = providers[i];
                        if (lang == "en")
                        {
                            requests = requests.Where(p => p.Provider.provider_name_en.Contains(provider));
                        }
                        else
                        {
                            requests = requests.Where(p => p.Provider.provider_name_ar.Contains(provider));
                        }
                    }


                }

                if (endDate is not null && startDate is null)
                {
                    return NotFound(new MessageDto { Message = "Enter start Date" });
                }

                if (endDate is null && startDate is not null)
                {
                    return NotFound(new MessageDto { Message = "Enter end Date" });
                }

                if (endDate is not null && startDate is not null)
                {

                    DateTime startDat = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime endDat = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    requests = requests
                         .AsEnumerable().Where(entity =>
                               DateTime.TryParseExact(entity.created_date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var entityDate) &&
                               entityDate >= startDat &&
                               entityDate <= endDat)
                        .AsQueryable();
                }
                var totalProviders = requests.Count();
                requests = requests.Skip((startpage - 1) * pageSize).Take(pageSize).OrderBy(e => e.Provider.provider_name_en);

                if (lang == "en")
                {
                    foreach (var request in requests)
                    {
                        string url = $"https://hcms.mediconsulteg.com/generate_approval_page.aspx?approval_id={request.Approval_id}";

                        string file = string.Empty;
                        if (request.Approval_id is not null)
                        {
                            if (Path.Exists($@"C:\inetpub\wwwroot\hcms_v1\tempFiles\{request.Approval_id}.pdf"))
                            {
                                file = $"https://hcms.mediconsulteg.com/tempFiles/{request.Approval_id}.pdf";
                            }
                            else
                            {
                                WebClient client = new WebClient();
                                file = client.DownloadString(new Uri(url));
                                file = file.Replace(@"/hcms/C:\inetpub\wwwroot\hcms_v1\", string.Empty);
                                file = ExtractUrl(file);

                            }
                        }


                        RequestDetailsForMemberDTO reqDetalisEnDto = new RequestDetailsForMemberDTO
                        {

                            Id = request.ID,
                            CreatedDate = request.created_date,
                            ApprovalId = request.Approval_id,
                            ProviderName = request.Provider?.provider_name_en,
                            Status = request.Status,
                            ApprovalPDF = file,
                            rejectReason = request.reject_reason,
                        };

                        reqDto.Add(reqDetalisEnDto);

                    }
                    var medicalDto = new
                    {

                        TotalCount = totalProviders,
                        PageNumber = startpage,
                        PageSize = pageSize,
                        Requests = reqDto,


                    };
                    return Ok(medicalDto);
                }

                foreach (var request in requests)
                {
                    string url = $"https://hcms.mediconsulteg.com/generate_approval_page.aspx?approval_id={request.Approval_id}";

                    string file = string.Empty;
                    if (request.Approval_id is not null)
                    {
                        if (Path.Exists($@"C:\inetpub\wwwroot\hcms_v1\tempFiles\{request.Approval_id}.pdf"))
                        {
                            file = $"https://hcms.mediconsulteg.com/tempFiles/{request.Approval_id}.pdf";
                        }
                        else
                        {
                            WebClient client = new WebClient();
                            file = client.DownloadString(new Uri(url));
                            file = file.Replace(@"C:\inetpub\wwwroot\hcms_v1\", string.Empty);
                            file = ExtractUrl(file);

                        }
                    }

                    RequestDetailsForMemberDTO reqDetalisArDto = new RequestDetailsForMemberDTO
                    {
                        Id = request.ID,
                        CreatedDate = request.created_date,
                        Status = request.Status,
                        ApprovalPDF = file,
                        rejectReason = request.reject_reason,
                        ProviderName = request.Provider.provider_name_ar,

                    };

                    reqDto.Add(reqDetalisArDto);


                }

                var medicalArDto = new
                {

                    TotalCount = totalProviders,
                    PageNumber = startpage,
                    PageSize = pageSize,
                    Requests = reqDto,


                };
                return Ok(medicalArDto);
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

        #region RequestByRequestId
        [HttpGet("RequestId")]

        public async Task<IActionResult> GetResultByID([Required] int id, string lang)
        {
            if (ModelState.IsValid)
            {
                var request = requestRepo.GetById(id);
                if (request is null)
                {
                    return NotFound(new MessageDto { Message = Messages.RequestNotFound(lang) });
                }
                if (request.Folder_path == "0" || string.IsNullOrEmpty(request.Folder_path))
                {
                    return BadRequest(new MessageDto { Message = "Invalid" });
                }
                if (!Directory.Exists(request.Folder_path))
                {
                    return BadRequest(new MessageDto { Message = "Invalid folder" });
                }
                string[] fileNames = Directory.GetFiles(request.Folder_path);
                List<string> fileNameList = fileNames.ToList();
                string url = $"https://hcms.mediconsulteg.com/generate_approval_page.aspx?approval_id={request.Approval_id}";

                string file = string.Empty;
                if (request.Approval_id is not null)
                {
                    if (Path.Exists($@"C:\inetpub\wwwroot\hcms_v1\tempFiles\{request.Approval_id}.pdf"))
                    {
                        file = $"https://hcms.mediconsulteg.com/tempFiles/{request.Approval_id}.pdf";
                    }
                    else
                    {
                        WebClient client = new WebClient();
                        file = client.DownloadString(new Uri(url));
                        file = file.Replace(@"C:\inetpub\wwwroot\hcms_v1\", string.Empty);
                        file = ExtractUrl(file);

                    }
                }

                if (lang == "en")
                {

                    var reqEnDto = new RequestDetailsDTO
                    {

                        Id = request.ID,
                        ApprovalPDF = file,
                        ProviderName = request.Provider?.provider_name_en,
                        ProviderId = request.Provider_id,
                        rejectReason = request.reject_reason,
                        Notes = request.Notes,
                        FolderPath = fileNameList,
                        Status = request.Status,
                        Is_Chronic= request.is_chronic


                    };
                    if (request.provider_portal_request_id>0)
                    {
                        reqEnDto.Allow_Edit = false;
                    }
                    else
                    {
                        reqEnDto.Allow_Edit = true;
                    }
                    switch (request.Status)
                    {
                        case "Received":
                        case "OnHold":
                      
                            break;
                        default:
                           reqEnDto.Allow_Edit= false;
                            break;
                    }
                    return Ok(reqEnDto);
                }

                var reqArDto = new RequestDetailsDTO
                {

                    Id = request.ID,
                    ApprovalPDF = file,
                    ProviderName = request.Provider?.provider_name_ar,
                    rejectReason = request.Approval?.reject_reason,
                    ProviderId = request.Provider_id,
                    Notes = request.Notes,
                    FolderPath = fileNameList,
                    Is_Chronic = request.is_chronic

                };
                if (request.provider_portal_request_id > 0)
                {
                    reqArDto.Allow_Edit = false;
                }
                else
                {
                    reqArDto.Allow_Edit = true;
                }
                switch (request.Status)
                {
                    case "Received":
                    case "OnHold":
                        reqArDto.Allow_Edit = true;

                        break;
                    default:
                        reqArDto.Allow_Edit = false;
                        break;
                }
                return Ok(reqArDto);
            }
            return BadRequest(ModelState);
        }
        #endregion



        #region CompressPdf
        private void CompressPdf(string inputPath, string outputPath)
        {
            // Open the input PDF file
            using (PdfReader reader = new PdfReader(inputPath))
            {
                // Create a PdfWriter with compression settings
                using (PdfWriter writer = new PdfWriter(outputPath, new WriterProperties().SetCompressionLevel(CompressionConstants.BEST_COMPRESSION)))
                {
                    // Open the PdfDocument
                    using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                    {
                        // Save the compressed PDF
                        pdfDoc.Close();
                    }
                }
            }
        }
        #endregion

    }
}
