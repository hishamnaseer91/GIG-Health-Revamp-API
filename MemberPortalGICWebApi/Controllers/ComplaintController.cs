using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.CompalintDAL;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.MemberUserDAL;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace MemberPortalGICWebApi.Controllers
{
    [RoutePrefix("api/Complaint")]
    public class ComplaintController : ApiController
    {
        IComplaints repo = new ComplaintDAL();
        /// <summary>
        /// Get Categories List at time of adding new complaint
        /// </summary>
        /// <returns></returns>

        [Authorize]
        [Route("GetCategorieslist")]
        [HttpPost]
        public IHttpActionResult GetAllCategories(CategoryListModel model)
        {
            var list = repo.GetAllCategories(model); 
            if (list != null)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "GetCategorieslist Success",
                    DescriptionArabic = "GetCategorieslist Success",
                    Data = list
                };
                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-02",
                    Type = "Get",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }

        }

        /// <summary>
        /// Register a new complaint
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [Route("AddComplaint")]
        [HttpPost]
        public IHttpActionResult AddNewComplaint(AddComplaintModel model)
        {
            
            if (model.Subject != "" & model.Description != ""  & model.CategoryName != "" & model.CivilId != "")
            {
                IMemberUser memberRepo = new MemberUserDAL();
                var memberInfo = memberRepo.GetMemberProfile(model.CivilId, model.policyNumber);

                model.CreationDate = DateTime.Now;

                int IsAddedComplaint = repo.AddNewComplaint(model);
                

                if (IsAddedComplaint != -1)
                {

                    model.ComplaintId = IsAddedComplaint;
                    Task.Run(() => SendEmailToAdmin(model.CivilId,model.CategoryName,model.Subject,model.Description,memberInfo.Name,memberInfo.POLICY_NUMBER,memberInfo.MEMBER_NUMBER));
                    

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-03",  //Success
                        Type = "Post/Add",
                        Description = "Complaint Regsitered",
                        DescriptionArabic = "تم تسجيل الشكوى.",
                        Data = model

                    };

                    return Ok(code);
                }
                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-04",
                        Type = "Post/Add",
                        Description = "Complaint not Registered, Please Try Again Later",
                        DescriptionArabic = "لم يتم تسجيل الشكوى. الرجاء المحاولة لاحقا",
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

        async void SendEmailToAdmin(string CivilId, string CategoryName, string Subject, string Description, string Name, string POLICY_NUMBER, string MEMBER_NUMBER)
        {
            // This method runs asynchronously.
            int i = await Task.Run(() => SendEmail(CivilId, CategoryName, Subject, Description,Name,POLICY_NUMBER,MEMBER_NUMBER));

        }

        public int SendEmail(string CivilId, string CategoryName, string Subject, string Description,string Name, string POLICY_NUMBER, string MEMBER_NUMBER)
        {

            try
            {
                string emailMessage = "";
                //GEt TEmpalte and put values
                EmailTemplates template = new EmailTemplates();
                emailMessage = template.GetAddComplainttemplate;
                emailMessage = emailMessage.Replace("[CivilId]", CivilId);
                emailMessage = emailMessage.Replace("[Category]", CategoryName);
                emailMessage = emailMessage.Replace("[Subject]", Subject);
                emailMessage = emailMessage.Replace("[Decription]", Description);
               

                emailMessage = emailMessage.Replace("[PolicyNo] ", POLICY_NUMBER);
                //// emailMessage = emailMessage.Replace("[NetworkId]", TempData["NetworkID"].ToString());
                emailMessage = emailMessage.Replace("[Name]", Name);
                emailMessage = emailMessage.Replace("[MemberNo]", MEMBER_NUMBER);

                IComplaints sendMail = new ComplaintDAL();
                var settingResult = sendMail.GetSMTPSetting().FirstOrDefault();
                var emailTo = ConfigurationManager.AppSettings["EmailTo"];
                MailAddress sendFrom = new MailAddress(settingResult.EmailForm);
                MailAddress sendTo = new MailAddress(emailTo);
                MailMessage message = new MailMessage(sendFrom, sendTo);
                //MailAddress sendCC = new MailAddress(Email.EmailAgent);

                message.Subject = "New Complaint Registered";
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
    }
}
