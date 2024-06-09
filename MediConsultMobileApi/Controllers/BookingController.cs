using MediConsultMobileApi.DTO;
using MediConsultMobileApi.Language;
using MediConsultMobileApi.Models;
using MediConsultMobileApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IProviderLocationRepository locationReppo;
        private readonly IProviderSpecialtyRepository specialRepo;
        private readonly IBookingRepository bookingRepo;
        private readonly IProviderDataRepository providerRepo;
        private readonly IMemberRepository memberRepo;
        private readonly IProviderRatingRepository ratingRepo;

        public BookingController(IProviderLocationRepository locationReppo, IProviderSpecialtyRepository specialRepo, IBookingRepository bookingRepo, IProviderDataRepository providerRepo, IMemberRepository memberRepo, IProviderRatingRepository ratingRepo)
        {
            this.locationReppo = locationReppo;
            this.specialRepo = specialRepo;
            this.bookingRepo = bookingRepo;
            this.providerRepo = providerRepo;
            this.memberRepo = memberRepo;
            this.ratingRepo = ratingRepo;
        }

        #region GetBooking
        [HttpGet("filtration")]
        public IActionResult GetBooking(string lang, [FromQuery] BookingFilter bookingFilter, int startPage = 1, int pageSize = 10)
        {

            if (ModelState.IsValid)
            {
                var bookingListDto = new List<BookingDto>();

                var locations = locationReppo.GetProviderLocations();
                var specialists = specialRepo.GetProviderSpecialties();
                var totalProviderCount = specialRepo.GetCountOfProviders().Count();

                if (lang == "en")
                {
                    if (bookingFilter.cityName is not null)
                    {
                        locations = locations.Where(p => p.AppSelectorGovernmentCity.city_name_en.Contains(bookingFilter.cityName));
                    }
                    if (bookingFilter.govName is not null)
                    {
                        locations = locations.Where(p => p.AppSelectorGovernmentCity.appSelectorGovernment.government_name_en.Contains(bookingFilter.govName));
                    }
                    if (bookingFilter.providerName is not null)
                    {
                        locations = locations.Where(p => p.Provider.provider_name_en.Contains(bookingFilter.providerName));
                    }
                    if (bookingFilter.categoryName is not null)
                    {
                        locations = locations.Where(p => p.Provider.Category.Category_Name_En.Contains(bookingFilter.categoryName));
                    }
                    if (bookingFilter.specialtyName is not null)
                    {
                        specialists = specialists.Where(p => p.GeneralSpecialty.General_Specialty_Name_En.Contains(bookingFilter.specialtyName));
                    }
                    if (bookingFilter.subSpecialtyName is not null)
                    {
                        specialists = specialists.Where(p => p.subGeneralSpecialty.Specialty_Name_En.Contains(bookingFilter.subSpecialtyName));
                    }
                }
                else
                {
                    if (bookingFilter.cityName is not null)
                    {
                        locations = locations.Where(p => p.AppSelectorGovernmentCity.city_name_ar.Contains(bookingFilter.cityName));
                    }
                    if (bookingFilter.govName is not null)
                    {
                        locations = locations.Where(p => p.AppSelectorGovernmentCity.appSelectorGovernment.government_name_ar.Contains(bookingFilter.govName));
                    }
                    if (bookingFilter.providerName is not null)
                    {
                        locations = locations.Where(p => p.Provider.provider_name_ar.Contains(bookingFilter.providerName));
                    }
                    if (bookingFilter.categoryName is not null)
                    {
                        locations = locations.Where(p => p.Provider.Category.Category_Name_Ar.Contains(bookingFilter.categoryName));
                    }
                    if (bookingFilter.specialtyName is not null)
                    {
                        specialists = specialists.Where(p => p.GeneralSpecialty.General_Specialty_Name_Ar.Contains(bookingFilter.specialtyName));
                    }
                    if (bookingFilter.subSpecialtyName is not null)
                    {
                        specialists = specialists.Where(p => p.subGeneralSpecialty.Specialty_Name_Ar.Contains(bookingFilter.subSpecialtyName));
                    }
                }


                var convertSpecialists = specialists.ToList();
                var convertLocations = locations.ToList();
                foreach (var location in convertLocations)
                {

                    var bookingDto = new BookingDto();
                    var totalProvidersSpecialty = 0;
                    foreach (var specialist in convertSpecialists)
                    {
                        //totalProvidersSpecialty = specialRepo.GetProvidersSpecialtiesByProviderId(specialist.Specialty_Id).Count();
                        if (location.provider_id == specialist.provider_id)
                        {
                            if (lang == "en")
                            {

                                bookingDto.Specialist_Name = specialist.GeneralSpecialty.General_Specialty_Name_En;
                                bookingDto.Sub_Specialist_Name = specialist.subGeneralSpecialty.Specialty_Name_En;
                            }
                            else
                            {
                                bookingDto.Specialist_Name = specialist.GeneralSpecialty.General_Specialty_Name_Ar;
                                bookingDto.Sub_Specialist_Name = specialist.subGeneralSpecialty.Specialty_Name_Ar;

                            }
                        }
                    }

                    bookingDto.Location_id = location.location_id;
                    bookingDto.Provider_id = location.provider_id;
                    bookingDto.Telephone_1 = location.location_telephone_1;
                    bookingDto.Telephone_2 = location.location_telephone_2;
                    bookingDto.Mobile_1 = location.location_mobile_1;
                    bookingDto.Mobile_2 = location.location_mobile_2;
                    bookingDto.HotLine = location.hotline;
                    bookingDto.Status = location.Provider.provider_status;
                    //bookingDto.TotalProviders = totalProvidersSpecialty;

                    if (lang == "en")
                    {
                        bookingDto.Area = location.location_area_en;
                        bookingDto.Address = location.location_address_en;
                        bookingDto.City_Name = location.AppSelectorGovernmentCity.city_name_en;
                        bookingDto.Government_Name = location.AppSelectorGovernmentCity.appSelectorGovernment.government_name_en;
                        bookingDto.Provider_Name = location.Provider.provider_name_en;
                        bookingDto.Category_Name = location.Provider.Category.Category_Name_En;


                    }
                    else
                    {

                        bookingDto.Area = location.location_area_ar;
                        bookingDto.Address = location.location_address_ar;
                        bookingDto.City_Name = location.AppSelectorGovernmentCity.city_name_ar;
                        bookingDto.Government_Name = location.AppSelectorGovernmentCity.appSelectorGovernment.government_name_ar;
                        bookingDto.Provider_Name = location.Provider.provider_name_ar;
                        bookingDto.Category_Name = location.Provider.Category.Category_Name_Ar;


                    }


                    bookingListDto.Add(bookingDto);
                    //}
                }
                var totalBooking = bookingListDto.Count();

                bookingListDto = bookingListDto
                         .Skip((startPage - 1) * pageSize)
                         .Take(pageSize).ToList();
                var result = new
                {
                    TotalBooking = totalBooking,
                    PageNum = startPage,
                    PageSize = pageSize,
                    ProviderLocations = bookingListDto
                };
                return Ok(result);
            }
            return BadRequest(ModelState);

        }

        #endregion

        #region AddBooking

        [HttpPost("AddBooking")]
        public async Task<IActionResult> AddBooking(string lang, [FromForm] AddBookingDto bookingDto)
        {
            if (ModelState.IsValid)
            {
                var providerExists = await providerRepo.ProviderExistsAsync(bookingDto.provider_id);
                var memberExists = memberRepo.MemberExists(bookingDto.member_id);
                var locationExists = locationReppo.ProviderLocationExists(bookingDto.provider_Location_id);

                if (string.IsNullOrEmpty(bookingDto.notes))
                {
                    return NotFound(new MessageDto { Message = Messages.EnterNotes(lang) });
                }

                if (string.IsNullOrEmpty(bookingDto.date))
                {
                    return NotFound(new MessageDto { Message = Messages.EnterDate(lang) });
                }
                if (string.IsNullOrEmpty(bookingDto.time))
                {
                    return NotFound(new MessageDto { Message = Messages.EnterTime(lang) });
                }
                if (bookingDto.member_id is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterMember(lang) });

                }
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });
                }
                if (bookingDto.provider_id is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterProvider(lang) });

                }
                if (!providerExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.ProviderNotFound(lang) });
                }
                if (bookingDto.provider_Location_id is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterLocation(lang) });

                }
                if (!locationExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.LocationNotFound(lang) });
                }
                if (bookingDto.attachment is null)
                {
                    return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                }
                var serverPath = AppDomain.CurrentDomain.BaseDirectory;
                if (bookingDto.attachment.Count == 0)
                {
                    return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                }
                for (int j = 0; j < bookingDto.attachment.Count; j++)
                {

                    if (bookingDto.attachment[j].Length == 0)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });
                    }
                    const long maxSizeBytes = 5 * 1024 * 1024;

                    if (bookingDto.attachment[j].Length >= maxSizeBytes)
                    {
                        return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });
                    }
                    // image.png --0
                    switch (Path.GetExtension(bookingDto.attachment[j].FileName))
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


                if (bookingDto.attachment.Count == 0)
                {
                    return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });

                }
                for (int j = 0; j < bookingDto.attachment.Count; j++)
                {

                    if (bookingDto.attachment[j].Length == 0)
                    {
                        return BadRequest(new MessageDto { Message = Messages.NoFileUploaded(lang) });
                    }
                    const long maxSizeBytes = 5 * 1024 * 1024;

                    if (bookingDto.attachment[j].Length >= maxSizeBytes)
                    {
                        return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });
                    }
                    // image.png --0
                    switch (Path.GetExtension(bookingDto.attachment[j].FileName))
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
                var newBooking = bookingRepo.AddBooking(bookingDto);


                var folder = $"{serverPath}\\MemberPortalApp\\{bookingDto.member_id}\\Booking\\{newBooking.id}";



                for (int i = 0; i < bookingDto.attachment.Count; i++)
                {
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + bookingDto.attachment[i].FileName;

                    string filePath = Path.Combine(folder, uniqueFileName);


                    if (Path.GetExtension(uniqueFileName) != ".pdf")
                    {

                        using (var stream = new MemoryStream())
                        {
                            bookingDto.attachment[i].CopyTo(stream);
                            stream.Seek(0, SeekOrigin.Begin);

                            using (var image = SixLabors.ImageSharp.Image.Load(stream))
                            {
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
                            bookingDto.attachment[i].CopyTo(stream);
                        }
                    }

                }


                return Ok(new MessageDto { Message = Messages.BookingReceived(lang) });
            }
            return BadRequest(ModelState);
        }

        #endregion

        #region EditBooking
        [HttpPost("EditBooking")]
        public async Task<IActionResult> EditBooking(string lang, [FromForm] UpdateBookingDto bookingDto, int? bookingId)
        {
            if (ModelState.IsValid)
            {
                var providerExists = await providerRepo.ProviderExistsAsync(bookingDto.provider_id);
                var memberExists = memberRepo.MemberExists(bookingDto.member_id);
                var locationExists = locationReppo.ProviderLocationExists(bookingDto.provider_Location_id);
                var bookingExists = bookingRepo.BookingExists(bookingId);

                if (bookingId is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterBooking(lang) });

                }
                if (!bookingExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.BookingNotFound(lang) });
                }
                if (string.IsNullOrEmpty(bookingDto.notes))
                {
                    return NotFound(new MessageDto { Message = Messages.EnterNotes(lang) });
                }
                if (string.IsNullOrEmpty(bookingDto.date))
                {
                    return NotFound(new MessageDto { Message = Messages.EnterDate(lang) });
                }
                if (string.IsNullOrEmpty(bookingDto.time))
                {
                    return NotFound(new MessageDto { Message = Messages.EnterTime(lang) });
                }
                if (bookingDto.member_id is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterMember(lang) });

                }
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });
                }
                if (bookingDto.provider_id is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterProvider(lang) });

                }
                if (!providerExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.ProviderNotFound(lang) });
                }
                if (bookingDto.provider_Location_id is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterLocation(lang) });

                }
                if (!locationExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.LocationNotFound(lang) });
                }

                var serverPath = AppDomain.CurrentDomain.BaseDirectory;
                var folder = $"{serverPath}\\MemberPortalApp\\{bookingDto.member_id}\\Booking\\{bookingId}";

                DirectoryInfo dir = new DirectoryInfo(folder);

                if (bookingDto.DeletePhotos is not null)
                {
                    foreach (var file in bookingDto.DeletePhotos)
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
                const long maxSizeBytes = 5 * 1024 * 1024;
                if (bookingDto.attachment is not null)
                {
                    for (int j = 0; j < bookingDto.attachment.Count; j++)
                    {

                        if (bookingDto.attachment[j].Length >= maxSizeBytes)
                        {
                            return BadRequest(new MessageDto { Message = Messages.SizeOfFile(lang) });
                        }
                        // image.png --0
                        switch (Path.GetExtension(bookingDto.attachment[j].FileName))
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

                    for (int i = 0; i < bookingDto.attachment.Count; i++)
                    {
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + bookingDto.attachment[i].FileName;

                        string filePath = Path.Combine(folder, uniqueFileName);


                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            await bookingDto.attachment[i].CopyToAsync(stream);
                        }
                    }

                }
                bookingRepo.UpdateBooking(bookingDto, bookingId);


                bookingRepo.Save();

                return Ok(new MessageDto { Message = Messages.Updated(lang) });

            }
            return BadRequest(ModelState);

        }

        #endregion

        #region HistoryOfBooking
        [HttpGet("HistoryOfBooking")]
        public async Task<IActionResult> GetHistoryBooking(string lang, [Required] int? memberId, [FromQuery] BookingFilter bookingFilter, int startPage = 1, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var memberExists = memberRepo.MemberExists(memberId);

                if (memberId is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterMember(lang) });

                }
                if (!memberExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.MemberNotFound(lang) });
                }

                var bookings = bookingRepo.GetBookingByMemberId(memberId);

                if (lang == "en")
                {
                    if (bookingFilter.cityName is not null || bookingFilter.govName is not null)
                    {
                        bookings = bookings.Where(p => p.ProviderLocation.location_area_en.Contains(bookingFilter.cityName));
                    }
                    if (bookingFilter.providerName is not null)
                    {
                        bookings = bookings.Where(p => p.Provider.provider_name_en.Contains(bookingFilter.providerName));
                    }
                    if (bookingFilter.categoryName is not null)
                    {
                        bookings = bookings.Where(p => p.Provider.Category.Category_Name_En.Contains(bookingFilter.categoryName));
                    }

                }
                else
                {
                    if (bookingFilter.cityName is not null || bookingFilter.govName is not null)
                    {
                        bookings = bookings.Where(p => p.ProviderLocation.location_area_ar.Contains(bookingFilter.cityName));
                    }

                    if (bookingFilter.providerName is not null)
                    {
                        bookings = bookings.Where(p => p.Provider.provider_name_ar.Contains(bookingFilter.providerName));
                    }
                    if (bookingFilter.categoryName is not null)
                    {
                        bookings = bookings.Where(p => p.Provider.Category.Category_Name_Ar.Contains(bookingFilter.categoryName));
                    }

                }

                var historyBookings = new List<HistoryBookingDto>();
                foreach (var booking in bookings.ToList())
                {
                    var location = locationReppo.GetProviderLocationsByLocationId(booking.provider_Location_id);
                    var specialty = specialRepo.GetProviderSpecialtiesByProviderId(booking.provider_id);
                    var historyBooking = new HistoryBookingDto
                    {
                        bookingId = booking.id,
                        date = booking.date,
                        notes = booking.notes,
                        status = booking.status,


                    };

                    if (lang == "en")
                    {
                        historyBooking.areaName = booking.ProviderLocation.location_area_en;
                        historyBooking.address = booking.ProviderLocation.location_address_en;
                        historyBooking.providerName = booking.Provider.provider_name_en;
                        historyBooking.govName = location.AppSelectorGovernment.government_name_en;
                        historyBooking.cityName = location.AppSelectorGovernmentCity.city_name_en;
                        historyBooking.categoryName = booking.Provider.Category.Category_Name_En;
                        if (specialty is null)
                        {
                            historyBooking.specialtyName = null;
                            historyBooking.subSpecialtyName = null;
                        }
                        else
                        {
                            historyBooking.specialtyName = specialty.GeneralSpecialty.General_Specialty_Name_En;
                            historyBooking.subSpecialtyName = specialty.subGeneralSpecialty.Specialty_Name_En;

                        }
                    }
                    else
                    {
                        historyBooking.areaName = booking.ProviderLocation.location_area_ar;
                        historyBooking.address = booking.ProviderLocation.location_address_ar;
                        historyBooking.providerName = booking.Provider.provider_name_ar;
                        historyBooking.govName = location.AppSelectorGovernment.government_name_ar;
                        historyBooking.cityName = location.AppSelectorGovernmentCity.city_name_ar;
                        historyBooking.categoryName = booking.Provider.Category.Category_Name_Ar;
                        if (specialty is null)
                        {
                            historyBooking.specialtyName = null;
                            historyBooking.subSpecialtyName = null;
                        }
                        else
                        {

                            historyBooking.specialtyName = specialty.GeneralSpecialty.General_Specialty_Name_Ar;
                            historyBooking.subSpecialtyName = specialty.subGeneralSpecialty.Specialty_Name_Ar;
                        }
                    }
                    historyBookings.Add(historyBooking);
                }

                return Ok(historyBookings);

            }
            return BadRequest(ModelState);
        }

        #endregion

        #region BookingById
        [HttpGet("BookingById")]

        public async Task<IActionResult> GetBookingById([Required] int bookingId, string lang)
        {
            if (ModelState.IsValid)
            {
                var booking = bookingRepo.GetById(bookingId);
                var bookingExists = bookingRepo.BookingExists(bookingId);


                if (!bookingExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.BookingNotFound(lang) });
                }
                if (booking is null)
                {
                    return NotFound(new MessageDto { Message = Messages.BookingNotFound(lang) });
                }

                if (!Directory.Exists(booking.attachment))
                {
                    return BadRequest(new MessageDto { Message = "Invalid folder path" });
                }
                var location = locationReppo.GetProviderLocationsByLocationId(booking.provider_Location_id);
                var specialty = specialRepo.GetProviderSpecialtiesByProviderId(booking.provider_id);
                string[] fileNames = Directory.GetFiles(booking.attachment);
                List<string> fileNameList = fileNames.ToList();

                var bookingDto = new BookingDetailsDTO
                {
                    bookingId = bookingId,
                    notes = booking.notes,
                    folderPath = fileNameList,
                    status = booking.status,
                    date = booking.date,
                    time = booking.time,


                };
                if (lang == "en")
                {
                    bookingDto.providerName = booking.Provider.provider_name_en;
                    bookingDto.govName = location.AppSelectorGovernment.government_name_en;
                    bookingDto.cityName = location.AppSelectorGovernmentCity.city_name_en;
                    bookingDto.categoryName = booking.Provider.Category.Category_Name_En;
                    bookingDto.address = booking.ProviderLocation.location_address_en;
                    bookingDto.areaName = booking.ProviderLocation.location_area_en;
                    bookingDto.specialtyName = specialty.GeneralSpecialty.General_Specialty_Name_En;
                    bookingDto.subSpecialtyName = specialty.subGeneralSpecialty.Specialty_Name_En;

                }
                else
                {
                    bookingDto.providerName = booking.Provider.provider_name_ar;
                    bookingDto.govName = location.AppSelectorGovernment.government_name_ar;
                    bookingDto.cityName = location.AppSelectorGovernmentCity.city_name_ar;
                    bookingDto.categoryName = booking.Provider.Category.Category_Name_Ar;
                    bookingDto.address = booking.ProviderLocation.location_address_ar;
                    bookingDto.areaName = booking.ProviderLocation.location_area_ar;
                    bookingDto.specialtyName = specialty.GeneralSpecialty.General_Specialty_Name_Ar;
                    bookingDto.subSpecialtyName = specialty.subGeneralSpecialty.Specialty_Name_Ar;

                }
                return Ok(bookingDto);


            }
            return BadRequest(ModelState);
        }
        #endregion



        #region DeleteBooking
        [HttpPost("DeleteBooking")]
        public async Task<IActionResult> DeleteBooking(string lang, int? bookingId)
        {
            if (ModelState.IsValid)
            {
                var bookingExists = bookingRepo.BookingExists(bookingId);
                var booking = bookingRepo.GetById(bookingId);

                if (bookingId is null)
                {
                    return NotFound(new MessageDto { Message = Messages.EnterBooking(lang) });

                }

                if (!bookingExists)
                {
                    return BadRequest(new MessageDto { Message = Messages.BookingNotFound(lang) });
                }

                if (booking.status != "Received")
                {
                    return NotFound(new MessageDto { Message = Messages.CantDeleted(lang) });

                }
                bookingRepo.DeleteBooking(bookingId);
                bookingRepo.Save();
                return Ok(new MessageDto { Message = Messages.Deleted(lang) });

            }
            return BadRequest(ModelState);



        }
        #endregion




    }
}
