using MemberPortalGICWebApi.Core;
using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.CompalintDAL;
using MemberPortalGICWebApi.DataObjects.Generics;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.MemberUserDAL;
using MemberPortalGICWebApi.DataObjects.RegisterDAL;
using MemberPortalGICWebApi.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace MemberPortalGICWebApi.Controllers
{
    [RoutePrefix("api/Register")]
    public class RegisterController : ApiController
    {
        private ErrorLogs_DB ErrorClass = new ErrorLogs_DB();
        private DBCommonError objeCommon = new DBCommonError();
        IRegister repo = new RegisterDAL();
        // [Authorize] 
        [Route("RegisterNewUser")]
        [HttpPost]
        public IHttpActionResult RegisterMember(RegisterMemberModel inputmodel)
        {
            string password = "";
            string EncryptedPassword = "";
            int addedUserID = 0;
            int addresult = 0;
            int smsAdded = 0;
            string policyNo = "";
            string networkID = "";
            string memberID = "";

            if (inputmodel.CivilId != "" & inputmodel.PhoneNumber != "")
            {
                //IRegister repo = new RegisterDAL();
                string IsValidUserResponce = repo.IsValidUser(inputmodel); //IF Is Valid User

                if (IsValidUserResponce == "success")
                {

                    bool IsExist = repo.IsMemberExist(inputmodel.CivilId); // Check if CivilId / Member Exists

                    #region Check is USer Exist

                    if (IsExist == true)
                    {
                        bool IsRegistered = repo.IsRegistrationComplete(inputmodel.CivilId); // Check Member is Registered but not login for the first time

                        if (IsRegistered == true)
                        {

                            var memberProfile = repo.GetMemberName(inputmodel.CivilId);
                            //Required NAME AND CIVILid
                            GenericModel obj = new GenericModel();
                            obj.name = memberProfile.Name;
                            obj.arabicName = memberProfile.ArabicName;
                            obj.civilId = inputmodel.CivilId;
                            obj.medicalNo = memberProfile.memNo;
                            obj.policyNo = memberProfile.policyNumber;
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-08",
                                Type = "Post/RegistrationNewMember",
                                Description = "Registration Request Already Recieved, Please Enter Password Sent in SMS to Complete Registration Process",
                                DescriptionArabic = "طلب التسجيل وصل. من فضلك أدخل كلمة المرور المرسلة في الرسائل القصيرة لإكمال عملية التسجيل",
                                Data = obj
                            };


                            string passwords = Common.GetRandomPassword();
                            #region Send SMS

                            SmsModel smsModel = new SmsModel()
                            {
                                OS_CLAIM_NO = "",
                                OS_LANG_FLAG = "AR",
                                OS_MOBILE_NO = "965" + inputmodel.PhoneNumber,
                                OS_TEXT = "عزيزي العميل, الرقم السري هو" + " " + passwords + " " + " الرجاء استخدامه لاكمال التسجيل"
                            };
                            try
                            {
                                string EncryptedPasswords = Common.Encrypt(passwords);
                                var updated = repo.UpdatePassword(inputmodel.CivilId, EncryptedPasswords);
                                int smsAddeds = repo.AddSMS(smsModel);
                                if (smsAddeds > 0)
                                {
                                    ErrorClass.ErrorCode = "SMS Send Success Mobile Registration";
                                    ErrorClass.ErorDesc = "SMS Send Success Mobile Registration" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + "Psw" + password + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                                    ErrorClass.TypeError = "Success";
                                    ErrorClass.ErrorExp = "SMS Send Success Mobile Registration Without Exception";
                                    //ErrorClass.ErrorExp = "Success sending sms from mobile " + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret; ;
                                    objeCommon.InsertErrorLogs(ErrorClass);
                                }
                                else
                                {
                                    ErrorClass.ErrorCode = "SMS Send Error Mobile Registration";
                                    ErrorClass.ErorDesc = "SMS Send Error Mobile Registration" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                                    ErrorClass.ErrorExp = "SMS Send Error Mobile Registration  Without Exception";
                                    ErrorClass.TypeError = "Error";

                                    objeCommon.InsertErrorLogs(ErrorClass);
                                }
                            }
                            catch (Exception ex)
                            {

                                ErrorClass.ErrorCode = "SMS Send Exception Mobile Registration";
                                ErrorClass.ErorDesc = "SMS Send Exception Mobile Registration" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                                ErrorClass.TypeError = "Exception";
                                ErrorClass.ErrorExp = ex.InnerException + " || " + ex.Message;

                                objeCommon.InsertErrorLogs(ErrorClass);
                            }

                            #endregion


                            return Ok(code);
                        }

                        else  // Member Already Exist Send Error Message
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-09",
                                Type = "Post/RegistrationNewMember",
                                Description = "Member User with this Civil ID is already Registered",
                                DescriptionArabic = "البطاقة المدنية مسجلة مسبقا"
                            };
                            return Ok(code);
                        }
                    }
                    #endregion

                    #region Add/RegisterNEw User
                    else // If Member Not exist , Register New Member
                    {

                        #region GetPolicyActiveDetails

                        try
                        {
                            var policyActiveModel = repo.GetActivePolicyCivilId(inputmodel.CivilId);
                            if (policyActiveModel == null)
                            {
                                APIResponceCodes code = new APIResponceCodes()
                                {
                                    Code = "CD-02",
                                    Type = "Get",
                                    Description = "Member Validity Not Found",
                                    DescriptionArabic = "Member Validity Not Found"

                                };
                                return Ok(code);
                            }

                            policyNo = policyActiveModel.POLICY_NUMBER;
                            networkID = policyActiveModel.Network_Id;
                            memberID = policyActiveModel.MEMBER_NUMBER;

                            if (DateTime.Now >= policyActiveModel.EXPIRY_DATE)
                            {
                                APIResponceCodes code = new APIResponceCodes()
                                {
                                    Code = "CD-02",
                                    Type = "Get",
                                    Description = "No Active Policy Found",
                                    DescriptionArabic = "No Active Policy Found"

                                };
                                return Ok(code);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorClass.ErrorCode = "GetPolicyActiveDetails Error";
                            ErrorClass.ErorDesc = "GetPolicyActiveDetails" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber;
                            ErrorClass.TypeError = "Error";
                            ErrorClass.ErrorExp = ex.InnerException + " || " + ex.Message;

                            objeCommon.InsertErrorLogs(ErrorClass);
                        }

                        #endregion

                        #region GetRandomPassword

                        password = Common.GetRandomPassword();

                        if (password == "" || password == null)
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-02",
                                Type = "Get",
                                Description = "Generated Random Password in Null",
                                DescriptionArabic = "Generated Random Password in Null"

                            };
                            return Ok(code);

                        }
                        EncryptedPassword = Common.Encrypt(password); // Encrypt passowrd

                        #endregion

                        #region Get Member User Details
                        AddMemberUserModel result = repo.GetMemberUserDetails(inputmodel.CivilId, inputmodel.MedicalInsuranceCardNumber); // Get Member Details for Insert

                        //Check for Childs only Principal mmebr and spouse can register now.
                        if (result.PrincipalMember == "N" && result.RelationshipDescription.ToUpper() == "CHILD")
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-06",
                                Type = "PostModel",
                                Description = "User is not a Principal Member",
                                DescriptionArabic = "User is not a Principal Member",

                            };
                            return Ok(code);
                        }

                        if (result.PolicyNo == "" || result.Network_Id == "" || result.PolicyNo == null || result.Network_Id == null)
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-06",
                                Type = "PostModel",
                                Description = "GetMemberUserDetails wrt CivilID is Null",
                                DescriptionArabic = "GetMemberUserDetails wrt CivilID is Null",

                            };
                            return Ok(code);
                        }

                        AddMemberUserModel addModel = new AddMemberUserModel() // If mmeber details != null fill add model
                        {
                            CivilId = inputmodel.CivilId,
                            PolicyNo = policyNo,
                            Member_Id = memberID,
                            MedicalInsuranceCardNumber = memberID,
                            Mobile = inputmodel.PhoneNumber,
                            CreationDate = DateTime.Now,
                            Password = EncryptedPassword,
                            Network_Id = networkID,
                            client_id = inputmodel.client_id,
                            client_secret = inputmodel.client_secret,
                            RelationshipDescription = result.RelationshipDescription.ToUpper(),
                            PrincipalMember = result.PrincipalMember.ToUpper()
                        };
                        #endregion


                        try
                        {
                            addresult = repo.AddNewMember(addModel); // Get Recently added MemberID
                        }
                        catch (Exception ex)
                        {
                            ErrorClass.ErrorCode = "AddNewMember Exceptionn";
                            ErrorClass.ErorDesc = "Error Occured whle adding new user" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                            ErrorClass.TypeError = "Error";
                            ErrorClass.ErrorExp = ex.InnerException + " || " + ex.Message;

                            objeCommon.InsertErrorLogs(ErrorClass);
                        }

                        if (addresult > 0) // If Member Added Successfully
                        {
                            #region Send SMS

                            SmsModel smsModel = new SmsModel()
                            {
                                OS_CLAIM_NO = "",
                                OS_LANG_FLAG = "AR",
                                OS_MOBILE_NO = "965" + inputmodel.PhoneNumber,
                                OS_TEXT = "عزيزي العميل, الرقم السري هو" + " " + password + " " + " الرجاء استخدامه لاكمال التسجيل"


                            };

                            try
                            {


                                smsAdded = repo.AddSMS(smsModel);//temp sms disabled
                                if (smsAdded > 0)
                                {
                                    ErrorClass.ErrorCode = "SMS Send Success Mobile Registration";
                                    ErrorClass.ErorDesc = "SMS Send Success Mobile Registration" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + "Psw" + password + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                                    ErrorClass.TypeError = "Success";
                                    ErrorClass.ErrorExp = "SMS Send Success Mobile Registration Without Exception";
                                    //ErrorClass.ErrorExp = "Success sending sms from mobile " + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret; ;
                                    objeCommon.InsertErrorLogs(ErrorClass);
                                }
                                else
                                {
                                    ErrorClass.ErrorCode = "SMS Send Error Mobile Registration";
                                    ErrorClass.ErorDesc = "SMS Send Error Mobile Registration" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                                    ErrorClass.ErrorExp = "SMS Send Error Mobile Registration  Without Exception Prod test mobile";
                                    ErrorClass.TypeError = "Error";

                                    objeCommon.InsertErrorLogs(ErrorClass);
                                }
                            }
                            catch (Exception ex)
                            {

                                ErrorClass.ErrorCode = "SMS Send Exception Mobile Registration";
                                ErrorClass.ErorDesc = "SMS Send Exception Mobile Registration" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                                ErrorClass.TypeError = "Exception";
                                ErrorClass.ErrorExp = ex.InnerException + " || " + ex.Message;

                                objeCommon.InsertErrorLogs(ErrorClass);
                            }

                            #endregion

                            RegisterCompleteUserResponce AddResponce = new RegisterCompleteUserResponce()
                            {
                                UID = addresult.ToString(),
                                MemberId = inputmodel.MedicalInsuranceCardNumber,
                                UserName = result.FullName,
                                MedicalInsuranceCard = inputmodel.MedicalInsuranceCardNumber,
                                UserNameArabic = result.FullName
                            };
                            GenericModel obj = new GenericModel();
                            obj.name = AddResponce.UserName;
                            obj.arabicName = AddResponce.UserNameArabic;
                            obj.civilId = inputmodel.CivilId;
                            obj.medicalNo = memberID;
                            obj.policyNo = policyNo;
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-03",
                                Type = "Post/Add",
                                Description = "Member Successfully Registered",
                                DescriptionArabic = "Member Successfully Registered",
                                Data = obj

                            };

                            return Ok(code);

                        }
                        else // Error Adding member user
                        {

                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-04",
                                Type = "POST/ADD",
                                Description = "Error Adding/Registring Member User",
                                DescriptionArabic = "Error Adding/Registring Member User"

                            };
                            return Ok(code);
                        }


                    }
                    #endregion }
                }


                else if (IsValidUserResponce == "InvalidCivilId")
                {


                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-00",
                        Type = "Post/Model",
                        Description = "Invalid Civil ID Entered",
                        DescriptionArabic = "الرقم المدني غير صحيح",
                    };
                    return Ok(code);
                }
                else if (IsValidUserResponce == "InvalidMemberId")
                {


                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-00",
                        Type = "Post/Model",
                        Description = "Invalid Member Number Entered",
                        DescriptionArabic = "رقم المشترك غير صحيح"
                    };
                    return Ok(code);
                }
                else if (IsValidUserResponce == "InvalidPhoneNo")
                {
                    var CropNumber = repo.LastFourDigits(inputmodel.CivilId);
                    string fullNo = CropNumber;
                    if (!string.IsNullOrEmpty(CropNumber) && CropNumber.Length >= 8)
                    {
                        CropNumber = CropNumber.Substring(4, CropNumber.Length - 4);
                    }
                    else
                    {
                        CropNumber = "";
                    }

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-00",
                        Type = "Post/Model",
                        Result = "****" + CropNumber,
                        ResultAR = CropNumber + "****",
                        Description = "Invalid Mobile Number, Please Use the Registered Number Ends With " + "****" + CropNumber,
                        DescriptionArabic = " رقم الجوال غير صحيح، يرجى استخدام الرقم الصحيح الذي ينتهي" + " " + CropNumber + "****",
                        PhoneNumber = fullNo
                    };
                    return Ok(code);
                }


                else
                {


                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-00",
                        Type = "Post/Model",
                        Description = "Please Contact Call Center Number: 1802080",
                        DescriptionArabic = "يرجى الاتصال بخدمة العملاء على 1802080"
                    };
                    return Ok(code);
                }


            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
                return Ok(code);
            }

        }//ENd MEthod



        //[Authorize]
        [Route("RegisterComplete")]
        [HttpPost]
        public IHttpActionResult UpdateRegisterMemberComplete(RegisterMemberCompleteModel inputmodel)
        {
            if (inputmodel.CivilId != "" & inputmodel.Password != "")
            {   //Encrypt passord remaining
                inputmodel.Password = Common.Encrypt(inputmodel.Password);

                var Id = repo.UpdateRegisterMemberComplete(inputmodel); // Update Sttatus


                if (Id > 0)
                {  // This is used to return memeber profile after regisrter success

                    IMemberUser profile = new MemberUserDAL();
                    var memberProfileDetails = profile.GetMemberProfile(inputmodel.CivilId, inputmodel.policyNumber);

                    bool IsFirstPasswordChaged = profile.IsFirstPasswordChange(inputmodel.CivilId);
                    //  var result = repo.GetMemberProfileDetails(Id.ToString());
                    string result = "First Password Chaged " + IsFirstPasswordChaged;
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "Registration Completed, Please Create your Password to use for Login",
                        DescriptionArabic = "تم اكتمال التسجيل, يرجى  انشاء  رقمك السري  لغرض تسجيل الدخول",
                        Result = "First Password Changed :" + IsFirstPasswordChaged.ToString()
                    };
                    return Ok(code);

                }
                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-04",
                        Type = "Post/Update",
                        Description = "Invalid Password, Please Enter Correct Password Again.",
                        DescriptionArabic = "كلمة المرور غير صالحة، الرجاء إدخال كلمة المرور الصحيحة مرة أخرى.",
                    };
                    return Ok(code);
                }


            }/// model

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
                return Ok(code);
            }
        }


        //[Authorize]
        [Route("ForgotPassword")]
        [HttpPost]
        public IHttpActionResult ForgotPassword(ResetPasswordModel inputmodel)
        {
            if (inputmodel.CivilId != "")
            {
                var result = repo.ForgotPassword(inputmodel);

                if (result != null)
                {
                   
                    var randomPassword = Common.GetRandomPasswordReset();
                    result.Password = randomPassword;
                    var EncryptedPasswords = Common.Encrypt(randomPassword);
                  
                
                    var updated = repo.UpdatePassword(inputmodel.CivilId, EncryptedPasswords);
                    //
                    //SMS tbl Insertion
                    SmsModel smsModel = new SmsModel()
                    {
                        OS_CLAIM_NO = "",
                        OS_LANG_FLAG = "AR",
                        OS_MOBILE_NO = "965" + result.MobileNo,
                        OS_TEXT = "عزيزي العميل, الرقم السري هو" + " " + result.Password
                    };

                    int smsAdded = repo.AddSMS(smsModel);

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "Password Sent to Registered Mobile Number",
                        DescriptionArabic = "تم أرسال كلمة المرور إلى رقم الهاتف المسجل",
                        Data = result
                    };
                    return Ok(code);
                    //Password Send Work Remaining
                }

                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-02",
                        Type = "Get",
                        Description = "Password Retrieve Failure, Incorrect Member Details",
                        DescriptionArabic = "لم يتم استرجاع كلمة المرور, معلومات المشترك غير صحيحة"

                    };
                    return Ok(code);

                }

            }
            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
                return Ok(code);
            }
        }


        [Route("ResendOtp")]
        [HttpPost]
        public IHttpActionResult ResendFourDigits(RegisterMemberModel inputmodel)
        {
            int smsAdded = 0;
            string password = string.Empty;
            string EncryptedPassword = string.Empty;


            password = Common.GetRandomPassword(); // Get Random passowrd 4 digits
            if (password == "" || password == null)
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-02",
                    Type = "Get",
                    Description = "Password Not Generated",
                    DescriptionArabic = "Password Not Generated"

                };
                return Ok(code);

            }

            #region Send SMS

            SmsModel smsModel = new SmsModel()
            {
                OS_CLAIM_NO = "",
                OS_LANG_FLAG = "AR",
                OS_MOBILE_NO = "965" + inputmodel.PhoneNumber,
                OS_TEXT = "عزيزي العميل, الرقم السري هو" + " " + password + " " + " الرجاء استخدامه لاكمال التسجيل"
            };

            try
            {
                string EncryptedPasswords = Common.Encrypt(password);
                var updated = repo.UpdatePassword(inputmodel.CivilId, EncryptedPasswords);
                smsAdded = repo.AddSMS(smsModel);
                ErrorClass.ErrorCode = "SMS Send Success Mobile Registration";
                ErrorClass.ErorDesc = "SMS Send Success Mobile Registration" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + "Psw" + password + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                ErrorClass.TypeError = "Success";
                ErrorClass.ErrorExp = "SMS Send Success Mobile Registration Without Exception";

                objeCommon.InsertErrorLogs(ErrorClass);
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-03",
                    Type = "Post/Add",
                    Description = "SMS Resent with OTP on your Registered Mobile Number",
                    DescriptionArabic = "تم اعادة الإرسال لرقم الهاتف المسجل",


                };

                return Ok(code);

            }
            catch (Exception ex)
            {

                ErrorClass.ErrorCode = "SMS Send Exception  Mobile Registration";
                ErrorClass.ErorDesc = "SMS Send Exception  Mobile Registration" + " CID " + inputmodel.CivilId + " MemberID " + inputmodel.MedicalInsuranceCardNumber + " Mobile " + inputmodel.PhoneNumber + "Source " + inputmodel.client_secret;
                ErrorClass.TypeError = "Exception";
                ErrorClass.ErrorExp = ex.InnerException + " || " + ex.Message;

                objeCommon.InsertErrorLogs(ErrorClass);

                APIResponceCodes codes = new APIResponceCodes()
                {
                    Code = "CD-03",
                    Type = "Post/Add",
                    Description = "SMS Not Send",
                    DescriptionArabic = "SMS Not Send",
                    Data = ex.ToString()

                };

                return Ok(codes);
            }
            #endregion
        }


        [Route("RegistrationFailEmailCRM")]
        [HttpGet]
        public IHttpActionResult SendEmail(string CivilID, string Mob)
        {
            if (!string.IsNullOrEmpty(CivilID) && !string.IsNullOrEmpty(Mob))
            {
                try
                {
                    string EmailAddress = string.Empty;
                    var NameEmailInfo = repo.GetName(CivilID);
                    if (NameEmailInfo != null)
                    {
                        EmailAddress = NameEmailInfo.Email;
                    }
                    string emailMessage = "";
                    emailMessage = GetAddComplainttemplate;

                    IComplaints sendMail = new ComplaintDAL();
                    var settingResult = sendMail.GetSMTPSetting().FirstOrDefault();
                    var emailTo = ConfigurationManager.AppSettings["EmailOnRegistrationFailure"];
                    MailAddress sendFrom = new MailAddress(settingResult.EmailForm);
                    MailAddress sendTo = new MailAddress(emailTo);
                    MailMessage message = new MailMessage(sendFrom, sendTo);
                    string Response = "Request Type:2" + "$#$MobileNumber:" + Mob + "$#$Email:" + EmailAddress;

                    emailMessage = emailMessage.Replace("[Response]", Response.ToString());
                    message.Subject = "Registration Failure";
                    message.Body = emailMessage;
                    message.IsBodyHtml = true;

                    System.Net.NetworkCredential nc = new System.Net.NetworkCredential(settingResult.UserName, settingResult.Password);
                    SmtpClient mailClient = new SmtpClient(settingResult.SMTPHost, settingResult.SMTPPort);
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = nc;
                    mailClient.Send(message);

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "Post/Model",
                        Description = "Please Contact GIG Call Center for Further Assistance 1802080",
                        DescriptionArabic = "يرجى الاتصال بخدمة العملاء على 1802080",
                        Result = "Email send to CRM for registrtion failure"
                    };
                    return Ok(code);
                }
                catch (Exception ex)
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-00",
                        Type = "Post/Model",
                        Description = ex.ToString()
                    };
                    return Ok(code);
                }
            }
            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
                return Ok(code);
            }
        }

        public string GetAddComplainttemplate = @"<html>
                <head>
                    <title></title>
                </head>
                    <body>
                        <div>
                        
                        
                          <p>[Response]</p><br/> 
                        
                        </div>
                    </body>

                </html>";
    }
}
