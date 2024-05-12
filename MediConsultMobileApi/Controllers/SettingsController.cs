using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsRepository settingsRepo;
        private readonly IMemberRepository memberRepo;

        public SettingsController(ISettingsRepository settingsRepo , IMemberRepository memberRepo)
        {
            this.settingsRepo = settingsRepo;
            this.memberRepo = memberRepo;
        }

        [HttpPost("ChangeNationalId")]
        public async Task<IActionResult> ChangeNationalId(string lang, [FromQuery] ChangeNationalIdDTO nationalIdDTO)
        {
            if (ModelState.IsValid)
            {
                var memberExists = memberRepo.MemberExists(nationalIdDTO.member_id);

                const long maxSizeBytes = 5 * 1024 * 1024;
                if (nationalIdDTO.national_id is null)
                {
                    return BadRequest(new MessageDto { Message = "Enter National id" });
                }

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }


                if (nationalIdDTO.national_id is not null)
                {

                    if (!long.TryParse(nationalIdDTO.national_id, out _))
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdNumber(lang) });


                    }
                    if (nationalIdDTO.national_id.Length != 14)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdFormat(lang) });


                    }
                    var (date, gender) = memberRepo.CreateDateAndGender(nationalIdDTO.national_id);     
                    if (!memberRepo.IsValidDate(date))
                    {
                        return BadRequest(new MessageDto { Message = Messages.NationalIdInvalid(lang) });


                    }

                }
                if (nationalIdDTO.nid_image is null)
                {
                    return NotFound(new MessageDto { Message = Messages.NoFileUploaded(lang) });
                }

                var serverPath = AppDomain.CurrentDomain.BaseDirectory;

                switch (Path.GetExtension(nationalIdDTO.nid_image.FileName))
                {
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                        break;
                    default:
                        return BadRequest(new MessageDto { Message = Messages.FileExtension(lang) });
                }
                if (nationalIdDTO.nid_image.Length >= maxSizeBytes)
                {
                    return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });

                }

                var folder = Path.Combine(serverPath, "MemberNationalId", nationalIdDTO.member_id.ToString());


                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }


                string uniqueFileName = Guid.NewGuid().ToString() + "_" + nationalIdDTO.nid_image.FileName;

                string filePath = Path.Combine(folder, uniqueFileName);


                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await nationalIdDTO.nid_image.CopyToAsync(stream);
                }
                nationalIdDTO.nid_image = ConvertFilePathToIFormFile(filePath);


               settingsRepo.EditNationalID(nationalIdDTO);
                settingsRepo.Save();
                var member = settingsRepo.GetByMemberId(nationalIdDTO.member_id);
                return Ok(member);
            }
            return BadRequest(ModelState);
        }
        private IFormFile ConvertFilePathToIFormFile(string filePath)
        {
            if (Path.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                var fileStream = fileInfo.OpenRead();

                var file = new FormFile(fileStream, 0, fileStream.Length, null, fileInfo.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/octet-stream" // Adjust the content type if needed
                };

                return file;
            }
            else
            {
                // Handle the case where the file does not exist
                throw new FileNotFoundException($"File not found at: {filePath}");
            }
        }
    }
}
