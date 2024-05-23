using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.CompalintDAL;
using MemberPortalGICWebApi.DataObjects.Generics;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.RequestCard;
using MemberPortalGICWebApi.Models;
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
    [RoutePrefix("api/RequestCard")]
    public class RequestCardController : ApiController
    {
        private ErrorLogs_DB ErrorClass = new ErrorLogs_DB();
        private DBCommonError objeCommon = new DBCommonError();
        IRequestCard obj = new RequestCard();


        //[Authorize]
        [Route("CardDeliveryRequestOnline")]
        [HttpPost]
        public IHttpActionResult RequestForCardDelivery(CardModel inputmodel)
        {
            if (!string.IsNullOrEmpty(inputmodel.CivilID))
            {
                var isActive = obj.IsActiveMember(inputmodel.CivilID);
                if (isActive)
                {
                    inputmodel.isCardDileveryRequest = 1;
                    inputmodel.isCardRePrintingRequest = 0;
                    var data = obj.AddCardRequest(inputmodel);
                    if (data > 0)
                    {
                        var NameEmailInfo = obj.GetName(inputmodel.CivilID);
                        if (NameEmailInfo != null)
                        {
                            inputmodel.Name = NameEmailInfo.Name;
                        }
                        inputmodel.EmailTo = "EmailOnCardDelivery";
                        inputmodel.Subject = "Card Delivery Request";
                        inputmodel.RequestType = "1";
                        var EmailSend = SendEmail(inputmodel);
                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-03",
                            Type = "POST/Model",
                            Description = "Request has been Forward Successfully.",
                            DescriptionArabic = "تم إعادة توجيه الطلب بنجاح",
                            Result = data.ToString()

                        };
                        return Ok(code);
                    }
                    else
                    {
                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-04",
                            Type = "POST/Model",
                            Description = "Request Failed.",
                            DescriptionArabic = "الطلب فشل"

                        };
                        return Ok(code);
                    }
                }
                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
                        Description = "No Member Found",
                        DescriptionArabic = "لم يتم العثور على سجل"

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


        [Authorize]
        [Route("CardReprintRequestOnline")]
        [HttpPost]
        public IHttpActionResult RequestForCardReprint(CardModel inputmodel)
        {
            if (!string.IsNullOrEmpty(inputmodel.CivilID) && !string.IsNullOrEmpty(inputmodel.PhoneNumber)
                && !string.IsNullOrEmpty(inputmodel.PolicyNumber))
            {
                var isActive = obj.IsValidMemberbyPolicyNumber(inputmodel);
                if (isActive == "Success")
                {
                    var memberNumber = obj.gETMemberbNumber(inputmodel);
                    if (memberNumber > 0)
                    {
                        inputmodel.MemberNumber = memberNumber.ToString();
                        var printCount = obj.GetPrintedCardsCount(inputmodel);
                        if (printCount > 0)
                        {
                            var info = obj.ExistingCardInfo(inputmodel);
                            if (info == null)
                            {
                                info = obj.ExistingCardInfoFromMEDICALCARDRECORD(inputmodel);
                            }
                            if (info != null)
                            {

                                inputmodel.isCardRePrintingRequest = 1;
                                inputmodel.isCardDileveryRequest = 0;
                                inputmodel.Region = info.Region;
                                inputmodel.Area = info.Area;
                                inputmodel.Block = info.Block;
                                inputmodel.StreetNo = info.StreetNo;
                                inputmodel.BuildingNo = info.BuildingNo;
                                inputmodel.FloorNo = info.FloorNo;

                                var data = obj.AddCardRequest(inputmodel);
                                if (data > 0)
                                {
                                    var NameEmailInfo = obj.GetName(inputmodel.CivilID);
                                    if (NameEmailInfo != null)
                                    {
                                        inputmodel.Name = NameEmailInfo.Name;
                                    }
                                    inputmodel.EmailTo = "EmailOnCardReprint";
                                    inputmodel.Subject = "Card Reprint Request";
                                    inputmodel.RequestType = "3";
                                    var EmailSend = SendEmail(inputmodel);
                                    APIResponceCodes code = new APIResponceCodes()
                                    {
                                        Code = "CD-03",
                                        Type = "POST/Model",
                                        Description = "Request has been Forward Successfully.",
                                        DescriptionArabic = "تم إعادة توجيه الطلب بنجاح"

                                    };
                                    return Ok(code);
                                }
                                else
                                {
                                    APIResponceCodes code = new APIResponceCodes()
                                    {
                                        Code = "CD-04",
                                        Type = "POST/Model",
                                        Description = "Request Failed.",
                                        DescriptionArabic = "الطلب فشل"

                                    };
                                    return Ok(code);
                                }
                            }
                            else
                            {
                                APIResponceCodes code = new APIResponceCodes()
                                {
                                    Code = "CD-06",
                                    Type = "POST/Model",
                                    Description = "Existing Card Info Not Found",
                                    DescriptionArabic = "معلومات البطاقة الحالية غير موجودة"

                                };
                                return Ok(code);
                            }

                        }
                        else
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-06",
                                Type = "POST/Model",
                                Description = "Existing Card Info Not Found",
                                DescriptionArabic = "معلومات البطاقة الحالية غير موجودة"

                            };
                            return Ok(code);
                        }
                    }
                    else
                    {
                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-06",
                            Type = "POST/Model",
                            Description = "Existing Card Info Not Found",
                            DescriptionArabic = "معلومات البطاقة الحالية غير موجودة"

                        };
                        return Ok(code);
                    }
                }
                else
                {
                    string EngMessage = "";
                    string ArabicMessage = "";

                    if (isActive == "Invalid Phone Number")
                    {
                        EngMessage = "Invalid Phone Number";
                        ArabicMessage = "رقم الهاتف غير صحيح";
                    }
                    if (isActive == "Invalid Civil ID")
                    {
                        EngMessage = "Invalid Civil ID";
                        ArabicMessage = "معرف مدني غير صالح";
                    }

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
                        Description = EngMessage,
                        DescriptionArabic = ArabicMessage

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


        public int SendEmail(CardModel inputmodel)
        {

            try
            {
                string emailMessage = "";
                emailMessage = GetAddComplainttemplate;

                IComplaints sendMail = new ComplaintDAL();
                var settingResult = sendMail.GetSMTPSetting().FirstOrDefault();
                var emailTo = ConfigurationManager.AppSettings[inputmodel.EmailTo];
                MailAddress sendFrom = new MailAddress(settingResult.EmailForm);
                MailAddress sendTo = new MailAddress(emailTo);
                MailMessage message = new MailMessage(sendFrom, sendTo);
                string Response = "Request Type:" + inputmodel.RequestType + "$#$Name:" + inputmodel.Name + "$#$Avenue:"
                               + inputmodel.Region + "$#$Area:" + inputmodel.Area + "$#$Street:" + inputmodel.StreetNo +
                                "$#$Building:" + inputmodel.BuildingNo + "$#$Floor:" + inputmodel.FloorNo + "$#$Block:" +
                                inputmodel.Block + "$#$Mobile:" + inputmodel.PhoneNumber;


                emailMessage = emailMessage.Replace("[Response]", Response.ToString());
                message.Subject = inputmodel.Subject;
                message.Body = emailMessage;
                message.IsBodyHtml = true;



                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(settingResult.UserName, settingResult.Password);
                SmtpClient mailClient = new SmtpClient(settingResult.SMTPHost, settingResult.SMTPPort);
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = nc;
                mailClient.Send(message);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
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

