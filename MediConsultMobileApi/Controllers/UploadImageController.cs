using iText.Layout.Element;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public UploadImageController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

    

        [HttpGet("getPhoto/{filePath}")]
        public IActionResult GetPhoto(string filePath)
        {
            try
            {
                // Combine the web root path with the requested file path
                string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, filePath).Replace("\\\\" , "\\");

                
                  

                // Check if the file exists
                if (System.IO.File.Exists(absolutePath))
                {
                    // Read the file content
                    var fileContent = System.IO.File.ReadAllBytes(absolutePath);

                    // Determine the file content type
                    string contentType = GetContentType(filePath);

                    // Return the file with appropriate content type
                    return File(fileContent, contentType);

                }

                // Return not found if the file does not exist
                return NotFound("File not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        private string GetContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream"; // Default content type
            }
            return contentType;
        }

        [HttpGet("DownloadFileApproval")]
        public IActionResult DownloadFile([Required] int approval_id)
        {
            var downloadFile = new DownloadFileDTO();
            
            string url = $"https://hcms.mediconsulteg.com/generate_approval_page.aspx?approval_id={approval_id}";

            string file = string.Empty;
          
                if (Path.Exists($@"C:\inetpub\wwwroot\hcms_v1\tempFiles\{approval_id}.pdf"))
                {
                    file = $"https://hcms.mediconsulteg.com/tempFiles/{approval_id}.pdf";
                }
                else
                {
                    WebClient client = new WebClient();
                    file = client.DownloadString(new Uri(url));
                    file = file.Replace(@"C:\inetpub\wwwroot\hcms_v1\", string.Empty);
                    file = ExtractUrl(file);

                }
                downloadFile.FileName = file;
                return Ok(downloadFile);
            
        }

        [HttpGet("DownloadFileRefund")]
        public IActionResult DownloadFileRefund([Required] int refundId)
        {
            var downloadFile = new DownloadFileDTO();

            string url = $"https://hcms.mediconsulteg.com/generate_refund_page.aspx?refund_id={refundId}";
            string file = string.Empty;

            if (Path.Exists($@"C:\inetpub\wwwroot\hcms_v1\tempFiles\Refund{refundId}.pdf"))
            {
                file = $"https://hcms.mediconsulteg.com/tempFiles/Refund{refundId}.pdf";
            }
            else
            {
                WebClient client = new WebClient();
                file = client.DownloadString(new Uri(url));
                file = file.Replace(@"C:\inetpub\wwwroot\hcms_v1\", string.Empty);
                file = ExtractUrl(file);

            }
            downloadFile.FileName = file;
            return Ok(downloadFile);

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

    }
}
