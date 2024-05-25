using Azure.Core;
using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using MediConsultMobileApi.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.Extensions.FileProviders;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository memberRepo;
        private readonly IValidation validation;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMemberECardInfoRepository eCardRepo;

        public MemberController(IMemberRepository memberRepo, IValidation validation, IWebHostEnvironment webHostEnvironment, IMemberECardInfoRepository eCardRepo)
        {
            this.memberRepo = memberRepo;
            this.validation = validation;
            this.webHostEnvironment = webHostEnvironment;
            this.eCardRepo = eCardRepo;
        }


        #region MemberById
        [HttpGet("Id")]
        public async Task<IActionResult> GetById(int id, string lang)
        {

            if (ModelState.IsValid)
            {
                var member = await memberRepo.GetByID(id); // member 

                if (member is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }

                var memberEnabled = memberRepo.MemberDetails(id).is_enabled;
                if (memberEnabled == 0 || memberEnabled is null)
                {
                    return NotFound(new MessageDto { Message = Messages.AccountDisabled(lang) });

                }
                if (member.program_name is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberArchive(lang) });
                }
                if (member.member_status == "Deactivated")
                {


                    return BadRequest(new MessageDto { Message = Messages.MemberDeactivated(lang) });

                }
                if (member.member_status == "Hold")
                {

                    return BadRequest(new MessageDto { Message = Messages.MemberHold(lang) });

                }

                var infoListDto = new List<MemberECardInfoDTO>();
                var infoList = eCardRepo.GetInfos(member.program_id);
                foreach (var info in infoList)
                {
                    var infoDto = new MemberECardInfoDTO
                    {
                        notes = info.notes
                    };
                    infoListDto.Add(infoDto);

                }

                if (lang == "en")
                {
                    MemberDetailsProfileEnDTO memberEnDTo = new MemberDetailsProfileEnDTO();


                    memberEnDTo.member_id = member.member_id;
                    memberEnDTo.member_name = member.member_name;
                    memberEnDTo.email = member.email;
                    memberEnDTo.room_class = member.room_class;
                    memberEnDTo.mobile = member.mobile;
                    memberEnDTo.program_name = member.Type_Name_En;
                    memberEnDTo.member_status = member.member_status;
                    memberEnDTo.job_title = member.job_title;
                    memberEnDTo.policy_id = member.policy_id;
                    memberEnDTo.program_id = member.program_id;
                    memberEnDTo.notes = infoListDto;
                    memberEnDTo.renew_date = member.renew_date;
                    memberEnDTo.member_birthday = member.member_birthday;
          
                    if (member.member_photo is not null &&  member.member_photo.Contains("hcms_v1"))
                    {
                        memberEnDTo.member_photo = member.member_photo.Replace("C:\\inetpub\\wwwroot\\hcms_v1", "https://hcms.mediconsulteg.com");
                    }
                    else
                    {
                        memberEnDTo.member_photo = member.member_photo;
                    }


                    return Ok(memberEnDTo);
                }

                MemberDetailsProfileArDTO memberArDTo = new MemberDetailsProfileArDTO();

                memberArDTo.member_id = member.member_id;
                memberArDTo.member_name = member.member_name;
                memberArDTo.email = member.email;
                memberArDTo.room_class = member.room_class;
                memberArDTo.mobile = member.mobile;
                memberArDTo.program_name = member.Type_Name_Ar;
                memberArDTo.member_status = member.member_status;
                memberArDTo.job_title = member.job_title;
                memberArDTo.policy_id = member.policy_id;
                memberArDTo.program_id = member.program_id;
                memberArDTo.notes = infoListDto;
                memberArDTo.renew_date = member.renew_date;
                memberArDTo.member_birthday = member.member_birthday;
                if (member.member_photo is not null && member.member_photo.Contains("hcms_v1"))
                {
                    memberArDTo.member_photo = member.member_photo.Replace("C:\\inetpub\\wwwroot\\hcms_v1", "https://hcms.mediconsulteg.com");
                }
                else
                {
                    memberArDTo.member_photo = member.member_photo;
                }
                return Ok(memberArDTo);
            }
            return BadRequest(ModelState);
        }

        #endregion

        #region MemberFamily
        [HttpGet("Family")]
        public async Task<IActionResult> MemberFamily([Required] int memberId, string lang)
        {
            if (ModelState.IsValid)
            {
                var families = await memberRepo.MemberFamily(memberId);

                var famDto = new List<MemberFamilyDTO>();
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }
                for (int i = 0; i < families.Count; i++)
                {
                    MemberFamilyDTO member = new MemberFamilyDTO
                    {

                        MemberId = families[i].member_id,
                        //male
                        MemberGender = families[i].member_gender,
                        //ahmed
                        MemberName = families[i].member_name,
                        //2022-03-17
                        MemberBirthday = families[i].member_birthday,
                        //Member
                        MemberLevel = families[i].member_level,

                        MemberStatus = families[i].member_status,

                        PhoneNumber = families[i].mobile,

                        NationalId = families[i].member_nid,


                    };

                    if (families[i].member_photo is not null && families[i].member_photo.Contains("hcms_v1"))
                    {
                        member.Photo = families[i].member_photo.Replace("C:\\inetpub\\wwwroot\\hcms_v1", "https://hcms.mediconsulteg.com");
                    }
                    else
                    {
                        member.Photo = families[i].member_photo;
                    }
                    famDto.Add(member);
                }
                return Ok(famDto);




            }
            return BadRequest(ModelState);
        }
        #endregion


        #region MemberDetails
        [HttpGet("MemberDetails")]
        public async Task<IActionResult> MemberDetails([Required] int memberId, string lang)
        {
            if (ModelState.IsValid)
            {
                var member = memberRepo.MemberDetails(memberId);
                var memberExists = memberRepo.MemberExists(memberId);
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }


                var memberDTo = new MemberDetailsDTO
                {

                    member_id = member.member_id,
                    member_name = member.member_name,
                    member_gender = member.member_gender,
                    email = member.email,
                    member_nid = member.member_nid,
                    //member_photo = member.member_photo,
                    mobile = member.mobile,
                    birthDate = member.member_birthday,
                    jobTitle = member.job_title

                };
                if (member.member_photo is not null && member.member_photo.Contains("hcms_v1"))
                {
                    memberDTo.member_photo = member.member_photo.Replace("C:\\inetpub\\wwwroot\\hcms_v1", "https://hcms.mediconsulteg.com");
                }
                else
                {
                    memberDTo.member_photo = member.member_photo;
                }
                return Ok(memberDTo);
            }
            return BadRequest(ModelState);
        }
        #endregion


        #region UpdateMember
        [HttpPost]
        public async Task<IActionResult> UpdateMember([FromForm] UpdateMemberDTO memberDTO, [Required] int id, string lang)
        {
            if (ModelState.IsValid)
            {
                var result = memberRepo.MemberDetails(id);
                var memberExists = memberRepo.MemberExists(id);

                string[] validExtensions = { ".jpg", ".jpeg", ".png" };
                const long maxSizeBytes = 5 * 1024 * 1024;

                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });

                }

                var existingMemberWithSameMobile = memberRepo.GetMemberByMobile(memberDTO.Mobile);
                var existingMemberWithSameEmail = memberRepo.GetMemberByEmail(memberDTO.Email);
                //var existingMemberWithSameNationalId = memberRepo.GetMemberByNationalId(memberDTO.SSN);


                if (memberDTO.Email is not null)
                {

                    if (existingMemberWithSameEmail != null && existingMemberWithSameEmail.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = Messages.Emailexist(lang) });

                    }

                    if (!validation.IsValidEmail(memberDTO.Email))
                    {
                        return BadRequest(new MessageDto { Message = Messages.EmailNotValid(lang) });

                    }

                }

                if (memberDTO.Mobile is not null)
                {
                    if (!long.TryParse(memberDTO.Mobile, out _))
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileNumber(lang) });


                    }
                    if (!memberDTO.Mobile.StartsWith("01"))
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileStartWith(lang) });

                    }
                    if (memberDTO.Mobile.Length != 11)
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileNumberFormat(lang) });

                    }

                    if (existingMemberWithSameMobile != null && existingMemberWithSameMobile.member_id != id)
                    {
                        return BadRequest(new MessageDto { Message = Messages.MobileNumbeExist(lang) });

                    }


                }

                //if (memberDTO.SSN is not null)
                //{

                //    if (!long.TryParse(memberDTO.SSN, out _))
                //    {
                //        return BadRequest(new MessageDto { Message = Messages.NationalIdNumber(lang) });


                //    }
                //    if (memberDTO.SSN.Length != 14)
                //    {
                //        return BadRequest(new MessageDto { Message = Messages.NationalIdFormat(lang) });


                //    }
                //    if (existingMemberWithSameNationalId != null && existingMemberWithSameNationalId.member_id != id)
                //    {
                //        return BadRequest(new MessageDto { Message = Messages.NationalIdExist(lang) });

                //    }

                //var (date, gender) = memberRepo.CreateDateAndGender(memberDTO.SSN);
                //    if (!memberRepo.IsValidDate(date))
                //    {
                //        return BadRequest(new MessageDto { Message = Messages.NationalIdInvalid(lang) });
                //    }
                //}


                if (memberDTO.Photo is not null)
                {

                    var serverPath = AppDomain.CurrentDomain.BaseDirectory;

                    var folder2 = Path.Combine(serverPath, "Members", id.ToString(), memberDTO.Photo.FileName);




                    if (memberDTO.Photo.Length == 0)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                    }
                    if (memberDTO.Photo.Length >= maxSizeBytes)
                    {
                        return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });

                    }

                    switch (Path.GetExtension(memberDTO.Photo.FileName))
                    {
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                            break;
                        default:
                            return BadRequest(new MessageDto { Message = Messages.FileExtension(lang) });

                    }
                    if (!Directory.Exists(folder2))
                    {
                        Directory.CreateDirectory(folder2);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + memberDTO.Photo.FileName;

                    using (var stream = new MemoryStream())
                    {
                        // Read the uploaded image into a MemoryStream
                        memberDTO.Photo.CopyTo(stream);
                        stream.Seek(0, SeekOrigin.Begin);

                        // Load the image using ImageSharp
                        using (var image = SixLabors.ImageSharp.Image.Load(stream))
                        {
                            // Compress the image
                            image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
                            string filePath2 = Path.Combine(folder2, uniqueFileName);

                            using (var outputStream = new FileStream(filePath2, FileMode.Create))
                            {
                                image.Save(outputStream, new JpegEncoder());
                            }


                            memberDTO.Photo = ConvertFilePathToIFormFile(filePath2);
                        }
                    }

                }
                memberRepo.UpdateMember(memberDTO, id);
                memberRepo.SaveDatabase();

                return Ok(new MessageDto { Message = Messages.MemberChange(lang) });
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





        #endregion


    }
}
