namespace MediConsultMobileApi.Language
{
    public class Messages
    {
        // Member  
        public static string MemberNotFound(string language)
        {
            return language == "ar" ? "المستخدم غير موجود" : "Member not found";
        }

        public static string MemberArchive(string language)
        {
            return language == "ar" ? "الحساب معطل" : "User in Archive";
        }
        public static string MemberDeactivated(string language)
        {
            return language == "ar" ? "الحساب معطل" : "User in Deactivated";
        }
        public static string MemberHold(string language)
        {
            return language == "ar" ? "الحساب معطل" : "User in Hold";
        }
        public static string MemberChange(string language)
        {
            return language == "ar" ? "تم التعديل بنجاح" : "Modified successfully";
        }

        public static string MemberLoginExists(string language)
        {
            return language == "ar" ? " المستخدم ليس لديه حساب" : "Member not have Account";
        }

        public static string EnterMember(string language)
        {
            return language == "ar" ? "يرجي إدخال رقم المستخدم " : "Please Enter member Id";
        }

        // Email Validation

        public static string Emailexist(string language)
        {
            return language == "ar" ? "البريد الإلكتروني موجود بالفعل لعضو آخر" : "Email already exists for another member.";
        }
        public static string EmailNotValid(string language)
        {
            return language == "ar" ? "البريد الإلكتروني غير صالح" : "Email is not valid.";
        }

        //Mobile Number Validation
        public static string MobileNumberNotFound(string language)
        {
            return language == "ar" ? "لا يوجد رقم موبيل" : "Mobile number not found";
        }
        public static string MobileNumber(string language)
        {
            return language == "ar" ? "يرجي إدخال الرقم الجوال صالحًا" : "Please Mobile number must be number";
        }
        public static string MobileStartWith(string language)
        {
            return language == "ar" ? "يرجي إدخال رقم الجوال يبدأ بـ 01" : "Please Mobile Number must start with 01";
        }
        public static string MobileNumberFormat(string language)
        {
            return language == "ar" ? "يرجي إدخال رقم الجوال مكون من 11 رقما" : "Please Mobile Number must be 11 number";
        }

        public static string MobileNumbeExist(string language)
        {
            return language == "ar" ? "رقم الجوال موجود بالفعل لعضو آخر" : "Mobile number already exists for another member.";
        }

        // National Id Validation 
        public static string NationalIdNumber(string language)
        {
            return language == "ar" ? "يرجي إدخال الرقم القومي صالحًا " : "Please National Id must be number";
        }
        public static string NationalIdFormat(string language)
        {
            return language == "ar" ? " يرجي إدخال الرقم القومي صالحًا والمكون من 14 رقمًا" : "Please National Id must be 14 number";
        }
        public static string NationalIdExist(string language)
        {
            return language == "ar" ? "الرقم القومي موجودة بالفعل لعضو آخر" : "National Id already exists for another member.";
        }
        public static string NationalIdInvalid(string language)
        {
            return language == "ar" ? "الرقم القومي غير صحيح" : "Invalid National Id.";
        }

        // Photo Validation 
        public static string NoFileUploaded(string language)
        {
            return language == "ar" ? "يرجي إدخال الملفات" : "Please Enter attachments";
        }
        public static string SizeOfFile(string language)
        {
            return language == "ar" ? "يرجي أن يكون حجم الملف أقل من 5 ميجابايت" : "Please File size must be less than 5 MB.";
        }
        public static string FileExtension(string language)
        {
            return language == "ar" ? "يرجي أن ينتهي مسار المجلد بالامتداد .jpg أو .png أو .jpeg" : "Please Folder Path must end with extension .jpg, .png, or .jpeg";
        }


        // notification 
        public static string NotificationValid(string language)
        {
            return language == "ar" ? "رسالة الاشعار غير صالحة" : "Invalid notification message";
        }
        public static string NotificationToken(string language)
        {
            return language == "ar" ? "لم يتم العثور على Token للأعضاء المحددين" : "No valid tokens found for the specified members";
        }
        public static string NotificationSend(string language)
        {
            return language == "ar" ? "تم إرسال الإشعار بنجاح" : "Notification sent successfully";
        }
        public static string NotificationImage(string language)
        {
            return language == "ar" ? "رابط الصورة غير صحيح" : "Imges not Url";
        }


        // Login 
        public static string PasswordFormat(string language)
        {
            return language == "ar" ? "كلمة المرور يجب ان تكون اكثر من 8 حروف او ارقام " : "Please Password  must be more than 8 charactar ";
        }

        public static string PasswordAndIdRequired(string language)
        {
            return language == "ar" ? "يرجي إدخال (رقم التعريف/الرقم القومي) و كلمة المرور" : "Please Id/NationalId and Password is required";
        }
        public static string InvalidId(string language)
        {
            return language == "ar" ? "رقم التعريف غير صالحة" : "Invalid Id";
        }
        public static string PasswordAndIdIncorrect(string language)
        {
            return language == "ar" ? "رقم التعريف/الرقم القومي او كلمة المرور غير صالحة" : "Id/NationalId  or Password is incorrect";
        }
        public static string AccountDisabled(string language)
        {
            return language == "ar" ? "الحساب معطل" : "Account Disabled";
        }
        public static string LoginSuccessfully(string language)
        {
            return language == "ar" ? "تم تسجيل الدخول بنجاح" : "Login Successfully";
        }

        public static string IncorrectId(string language)
        {
            return language == "ar" ? "رقم المستخدم غير صحيح" : "Member id is incorrect";
        }
        public static string IncorrectOtp(string language)
        {
            return language == "ar" ? "غير صحيح OTP" : "Otp is incorrect";
        }
        public static string DeliveredOtp(string language)
        {
            return language == "ar" ? "تم ارسال رسالة OTP " : "OTP Message delivered";
        }
        public static string PasswordAndConfirmPassword(string language)
        {
            return language == "ar" ? "كلمة المرور غير متساوية" : "Password not Equal ConfirmPasswod";
        }
        public static string EnterMobile(string language)
        {
            return language == "ar" ? "يرجي إدخال رقم الهاتف" : "Please Enter Mobile number";
        }
        public static string EnterNationalId(string language)
        {
            return language == "ar" ? "يرجي إدخال الرقم القومي" : "Please Enter National Id";
        }
        public static string EnterPassword(string language)
        {
            return language == "ar" ? "يرجي إدخال  كلمة المرور" : "Please Enter Password ";
        }
        public static string EnterConfirmPassword(string language)
        {
            return language == "ar" ? "يرجي إدخال التاكد علي كلمة المرور " : "Please Enter confirm Password ";
        }
        public static string EnterOtp(string language)
        {
            return language == "ar" ? "يرجي إدخال OTP " : "Please Enter OTP ";
        }
        public static string ChangePassword(string language)
        {
            return language == "ar" ? "تم" : "Done";
        }


        // MedicalNetwork
        public static string MedicalNetwork(string language)
        {
            return language == "ar" ? "غير موجود" : "Not found";
        }

        // Policy 

        public static string Policy(string language)
        {
            return language == "ar" ? "العميل غير مفعل" : "program id not found";
        }

        // Request 
        public static string ProviderNotFound(string language)
        {
            return language == "ar" ? "مقدم الخدمة غير موجود" : "Provider Id not found";
        }
        public static string ProviderDeactivated(string language)
        {
            return language == "ar" ? "مقدم الخدمة معطل" : "Provider is Deactivated";
        }
        public static string EnterProvider(string language)
        {
            return language == "ar" ? "يرجي إدخال مقدم الخدمة" : "Please Enter Provider Id";
        }
        public static string EnterIsChronic(string language)
        {
            return language == "ar" ? "يرجي إدخال هل ادوية مزمنة ام لا 0 او 1" : "Please Enter 0 if Approval is not Chronic & 1 if Chronic";
        }
        public static string RequestNotFound(string language)
        {
            return language == "ar" ? "لم يتم العثور على الطلب" : "Request not found";
        }
        public static string RequestEdit(string language)
        {
            return language == "ar" ? ".نعنذر الطلب في مرحلة المراجعة" : "Sorry Request in the review stage";
        }
        public static string Updated(string language)
        {
            return language == "ar" ? " تم التعديل  " : "Uploaded";
        }

        public static string Deleted(string language)
        {
            return language == "ar" ? " تم المسح  " : "Deleted";
        }

        // Updated
        // Refund

        public static string RefundTypeNotFound(string language)
        {
            return language == "ar" ? "لم يتم العثور على نوع طلب استرداد الأموال" : "Refund Type not found";
        }
        public static string RefundNotFound(string language)
        {
            return language == "ar" ? "لم يتم العثور على طلب استرداد الأموال" : "Refund not found";
        }
        public static string AmountNotFound(string language)
        {
            return language == "ar" ? "يرجي إدخال إجمالي المبلغ " : "Please Enter Total Amount ";
        }
        public static string EnterRefund(string language)
        {
            return language == "ar" ? "يرجي إدخال نوع الاسترداد" : "Please Enter Refund Type";
        }
        public static string EnterRefundDate(string language)
        {
            return language == "ar" ? "يرجي إدخال تاريخ الاسترداد" : "Please Enter Refund Date";
        }
        public static string RefundDateIncorrect(string language, string name)
        {
            return language == "ar" ? $"عزيزي ( {name} ) طبقاً لبنود التعاقد مع شركتكم الموقرة فإنه لا يتم الاسترداد النقدي بعد مرور 60 يوم من تاريخ تأدية الخدمة الطبية" : $"Dear {name}, according to the terms of the contract with your esteemed company, cash refunds will not be made after 60 days from the date of performing the medical service.";
        }
        public static string SuccessRegestration(string language)
        {
            return language == "ar" ? "تم التسجيل بنجاح" : "Successfully registered";
        }
        public static string EnterNotes(string language)
        {
            return language == "ar" ? "يرجي إدخال الملاحظات" : "Please Enter Notes";
        }
        public static string EnterDate(string language)
        {
            return language == "ar" ? "يرجي إدخال التاريخ" : "Please Enter Date";
        }
        public static string EnterTime(string language)
        {
            return language == "ar" ? "يرجي إدخال الوقت" : "Please Enter Time";
        }
        public static string EnterReason(string language)
        {
            return language == "ar" ? "يرجي إدخال سبب الاسترداد" : "Please Enter Refund Reason";
        }

        // Registeration

        public static string AccountExists(string language)
        {
            return language == "ar" ? "الحساب موجود بالفعل" : "Account already exists";
        }
        public static string SuccessValidationRegestration(string language)
        {
            return language == "ar" ? "تم بنجاح" : "SuccessValidationRegestration";
        }

        public static string CanceledApprovalChronic(string language)
        {
            return language == "ar" ? "تم المسح" : "Canceled";
        }
        public static string EnterLocation(string language)
        {
            return language == "ar" ? "برجاء ادخال المكان " : "Please Enter Location ";
        }
        public static string LocationNotFound(string language)
        {
            return language == "ar" ? "المكان غير موجود" : "Location not found";
        }
        public static string EnterBooking(string language)
        {
            return language == "ar" ? "برجاء ادخال رقم الحجز" : "Please Enter Booking Id";
        }
        public static string BookingNotFound(string language)
        {
            return language == "ar" ? "رقم الحجز غير موجود" : "Booking Id not found";
        }
        public static string CantDeleted(string language)
        {
            return language == "ar" ? "نعتذر لا يمكنك مسح الحجز" : "Sorry you can not delete";
        }
        public static string BookingReceived(string language)
        {
            return language == "ar" ? "تم الحجز بنجاح" : "Booking Received";
        }
        public static string CategoryNotFound(string language)
        {
            return language == "ar" ? "هذا فئة غير موجودة" : "Category is not Found";
        }
        public static string GovernmentExists(string language)
        {
            return language == "ar" ? "المحافظة غير موجودة" : "Government not found ";
        }
        public static string EnterServices(string language)
        {
            return language == "ar" ? "الرجاء ادخال الخدمة" : "Please enter Service id ";
        }
        public static string ServicesNotFound(string language)
        {
            return language == "ar" ? "احد الخدمات غير موجودة" : "Service not found ";
        }

        public static string EnterRate(string language)
        {
            return language == "ar" ? "الرجاء ادخال التقييم" : "Please enter your rating ";
        }
        public static string RateNotExists(string language)
        {
            return language == "ar" ? "التقييم غير موجود" : "rating not found ";
        }

        public static string AddRating(string language)
        {
            return language == "ar" ? "تم إضافة تقييمك" : "Your rating has been added ";
        }
    }
}
