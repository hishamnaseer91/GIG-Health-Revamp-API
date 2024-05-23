using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.CompalintDAL;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.MemberUserDAL;
using MemberPortalGICWebApi.DataObjects.RequestCard;
using MemberPortalGICWebApi.DataObjects.TravelDAL;
using MemberPortalGICWebApi.Models;
//using PdfSharp;
//using PdfSharp.Pdf;
//using PdfSharp;
//using PdfSharp.Pdf;
//using Syncfusion.Pdf.Barcode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
//using TheArtOfDev.HtmlRenderer.PdfSharp;
//using System.Web.Mail;
//using TheArtOfDev.HtmlRenderer.PdfSharp;
//using Rotativa;
//using RotativaHQ.MVC5;
//using Syncfusion.Pdf;
//using Syncfusion.DocIO;
//using Syncfusion.DocIO.DLS;
//using Syncfusion.DocToPDFConverter;
//using Aspose.Tasks;
//using System.Xml.Linq;
//using Aspose.Words;
//using Aspose.Words.Saving;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using MailMessage = System.Net.Mail.MailMessage;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.Controllers
{

    [RoutePrefix("api/Travel")]
    public class TravelController : ApiController
    {
        ITravel travelDAL = new TravelDAL();
        GeneralDAL generalDAL = new GeneralDAL();
        IRequestCard cardReq = new RequestCard();
        [Authorize]
        [Route("CheckCardReceived")]
        [HttpPost]
        public IHttpActionResult CheckCardReceived(CertificateRequest model)
        {
            APIResponceCodes code = null;
            if (model != null && !string.IsNullOrEmpty(model.CivilId) && !string.IsNullOrEmpty(model.MemberNo) && !string.IsNullOrEmpty(model.PolicyNo))
            {
                var genConfig = generalDAL.GetConfigration();
                if (Convert.ToInt32(model.PolicyNo) == genConfig.ACTIVE_AFYA_POLICY_NO || Convert.ToInt32(model.PolicyNo) == genConfig.ACTIVE_AFYA_3_POLICY_NO)
                {
                    var printCount = cardReq.GetPrintedCardsCount(new CardModel() { MemberNumber = model.MemberNo, PolicyNumber = model.PolicyNo });
                    if (printCount > 0)
                    {
                        code = new APIResponceCodes()
                        {
                            Code = "CD-03",
                            Type = "Post/Model",
                            Description = "Member Received Card.",
                            //DescriptionArabic = "الرجاء إدخال النص المفقود",
                            Data = true
                        };
                    }
                    else
                    {
                        code = new APIResponceCodes()
                        {
                            Code = "CD-06",
                            Type = "Post/Model",
                            Description = "Existing Card Info Not Found",
                            DescriptionArabic = "معلومات البطاقة الحالية غير موجودة",
                            Data = false
                        };
                    }
                }
                else
                {
                    code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "Post/Model",
                        Description = "No Record Found",
                        DescriptionArabic = "لم يتم العثور على سجل",
                        Data = false
                    };
                }
            }
            else
            {
                code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
            }
            return Ok(code);
        }

        //[Authorize]
        [Route("GenerateCertificateLocal")]
        [HttpPost]
        public IHttpActionResult GenerateCertificateLocal(CertificateRequest model)
        {
            APIResponceCodes code = null;
            if (model != null && !string.IsNullOrEmpty(model.CivilId) && !string.IsNullOrEmpty(model.MemberNo) &&
                !string.IsNullOrEmpty(model.MemberName) && !string.IsNullOrEmpty(model.PassportNo) && !string.IsNullOrEmpty(model.PolicyNo))
            {
                #region other libraries
                //Byte[] bytes = File.ReadAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate.pdf"));
                //String file = Convert.ToBase64String(bytes);


                // //Load an existing Word document
                // WordDocument wordDocument = new WordDocument(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English and Arabic.doc"), FormatType.Doc);
                // //Create an instance of DocToPDFConverter
                // DocToPDFConverter converter = new DocToPDFConverter();
                // //Convert Word document into PDF document
                // PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);
                // //Save the PDF file
                // pdfDocument.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English and Arabic.pdf"));
                // //Close the instance of document objects
                // pdfDocument.Close(true);
                // wordDocument.Close();
                // //This will open the PDF file so, the result will be seen in default PDF viewer
                ////System.Diagnostics.Process.Start("WordtoPDF.pdf");


                //// For complete examples and data files, please go to https://github.com/aspose-words/Aspose.Words-for-.NET
                //// Load the document from disk.
                //Document doc = new Document(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English and Arabic.doc"));

                //PdfSaveOptions pso = new PdfSaveOptions();
                //pso.Compliance = PdfCompliance.Pdf17;
                //// Save the document in PDF format.
                //doc.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English and Arabic.pdf"), pso);


                //PdfDocument pdf = PdfGenerator.GeneratePdf(html, PageSize.Letter);
                //pdf.PageLayout = PdfPageLayout.SinglePage;
                //pdf.PageMode = PdfPageMode.FullScreen;
                //pdf.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English.pdf"));
                #endregion
                try
                {
                    var certExist = travelDAL.GetCertDetails(model.MemberNo, model.PolicyNo);
                    if (certExist != null && certExist.MEMBER_NO == model.MemberNo && certExist.POLICY_NO == model.PolicyNo)
                    {
                        code = new APIResponceCodes()
                        {
                            Code = "CD-06",
                            Type = "Post/Model",
                            Description = "Certificate Already Exist.",
                            //DescriptionArabic = "لم يتم العثور على سجل",
                            //Data = false
                        };
                    }
                    else
                    {
                        var memberInfo = travelDAL.GetMemberInfo(model.MemberNo);
                        string fileHash = DateTime.UtcNow.ToString("ddMMyyyyHHmmss");
                        //string sourceHtmlFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/TravelInsuranceCertificate.htm");
                        //string destHtmlFileName = "TravelInsuranceCertificate - Latest.htm";
                        string destPdfFileName = fileHash + "_TravelInsuranceCertificate.pdf";
                        //string destHtmlFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + destHtmlFileName);
                        string destPdfFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + destPdfFileName);
                        //string html = File.ReadAllText(sourceHtmlFile);
                        //html = html.Replace("@PolicyNo", model.PolicyNo).Replace("@MemberName", model.MemberName).Replace("@PassportNo", model.PassportNo).Replace("@CivilId", model.CivilId).Replace("@DOB", memberInfo.DATE_OF_BIRTH.ToString("dd/MM/yyyy"));
                        //File.WriteAllText(destHtmlFile, html);
                        //var Renderer = new IronPdf.ChromePdfRenderer();
                        //Renderer.RenderingOptions.MarginTop = 0;  //millimeters
                        //Renderer.RenderingOptions.MarginBottom = 0;
                        //Renderer.RenderingOptions.MarginLeft = 10;
                        //Renderer.RenderingOptions.MarginRight = 0;
                        //var PDF = Renderer.RenderHtmlFileAsPdf(destHtmlFile);
                        //PDF.SaveAs(destPdfFile);

                        //var pdf = PdfHelper.GetPdf("~/Files/Afya Travel Insurance Certificate - English.cshtml");

                        //PdfDocument pdf = PdfGenerator.GeneratePdf(html, PageSize.Letter);
                        //pdf.PageLayout = PdfPageLayout.SinglePage;
                        //pdf.PageMode = PdfPageMode.FullScreen;
                        //pdf.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + destPdfFileName));

                        //Byte[] bytes = File.ReadAllBytes(destPdfFile);
                        //String file = Convert.ToBase64String(bytes);

                        //Save in database
                        //string destPdfFileName = model.CivilId + "_" + model.PolicyNo + "_TravelInsuranceCertificate.pdf";
                        //string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Print/PrintPDF" + "?memberNo=" + model.MemberNo + "&policyNo=" + model.PolicyNo;
                        string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Files/" + destPdfFileName;

                        //Word to PDF
                       
                        string sourceDocFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/AfyaTravelInsuranceCertificateOriginal.docx");
                        string destDocFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + fileHash + "_TravelInsuranceCertificate.docx");
                        if (!File.Exists(destDocFile))
                        {
                            File.Copy(sourceDocFile, destDocFile);
                        }
                        AfyaTravelCert certObj = new AfyaTravelCert()
                        {
                            MEMBER_NAME = model.MemberName,
                            PASSPORT_NO = model.PassportNo,
                            CIVIL_ID = model.CivilId,
                            DOB = memberInfo.DATE_OF_BIRTH.ToString("dd/MM/yyyy")
                        };
                        ConvertDoc(certObj, destDocFile, destPdfFile, WdSaveFormat.wdFormatPDF);
                        if (File.Exists(destDocFile))
                        {
                            File.Delete(destDocFile);
                        }
                        AfyaTravelCert cert = new AfyaTravelCert()
                        {
                            CERT_FILE = destPdfFileName,
                            CERT_HASH = fileHash,
                            CIVIL_ID = model.CivilId,
                            MEMBER_NAME = model.MemberName,
                            MEMBER_NO = model.MemberNo,
                            PASSPORT_NO = model.PassportNo,
                            POLICY_NO = model.PolicyNo,
                            SOURCE = "api"
                        };
                        int save = travelDAL.SaveCertDetails(cert);
                        if (save > 0)
                        {

                            code = new APIResponceCodes()
                            {
                                Code = "CD-03",
                                Type = "Post/Model",
                                Description = "Certificate Generated Successfully.",
                                DescriptionArabic = "تم إنشاء الشهادة بنجاح",
                                Data = printPath
                            };
                        }
                        else
                        {
                            code = new APIResponceCodes()
                            {
                                Code = "CD-06",
                                Type = "Post/Model",
                                Description = "Certificate Not Generated",
                                DescriptionArabic = "لم يتم إنشاء الشهادة",
                                //Data = false
                            };
                        }
                    }

                }
                catch (Exception ex)
                {
                    code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "Post/Model",
                        Description = ex.Message,
                        //DescriptionArabic = "لم يتم العثور على سجل",
                        //Data = false
                    };
                }

            }
            else
            {
                code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
            }
            return Ok(code);
        }

        [Route("GenerateCertificate")]
        [HttpPost]
        public async Task<IHttpActionResult> GenerateCertificate(CertificateRequest model)
        {
            APIResponceCodes code = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var handler = new HttpClientHandler();
                handler.UseCookies = false;
                using (var httpClient = new HttpClient(handler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), ConfigurationManager.AppSettings["FileGen"] + "/api/Travel/GenerateCertificateLocal"))
                    {
                        // request.Headers.TryAddWithoutValidation("Authorization", "Bearer yYgrpT1hj8mqcTUqWLwHJfsSmlMvJBCxZw0ZdjSNf3XYfu5RfXPPVGna_ZJckfFZ5Ml7uONCmKPEzOUCViVpHvPhWSK4pbdeuXbCuIwTbVjypXsIwRq4HHypRUeeEYnMS8Rtd9_TiNOR5xoZ5Qz0HGIXQ0hwxZgcXHIlX_5DrFJFZQIzD1so_iUaG5PVyvgCWYHVCbaaj4Yjgu3WEIWfZIqGox8vafC1BXyuqcSuTLbC2Ts68p-BZbxM3gqY9fdNhdSjJAWsyEJNld1Yji0ld0nPRDwEkpRX0Rz5hqkN2snuDqYuEQICZ-nx7UVrleWIfWs2PzmMKyqeVUMc8bfjbMXCWeS6BPp7gx9Yn64pWy1MrraeVE4ERayXeio38SFQlgUywkkMguCL9aGSdt2LRvlnlP-BVvh1pIPPSfBpHW0eTYfgw2iB5eLjlRw4dHKSXELI-kKWwD6tD63zaHCzXudGxLZ7A1mU19aSlO2BsECzQ9inWN6QEMFhKbzrRshas58LrbrHCJzaPgufVli_uA");
                        request.Headers.TryAddWithoutValidation("Cookie", "session-cookie=171d42dda6e2d4e09c011cac18991a24e5634a8fa8ce6c99d2aed80229f1cd1c643cc4a332227202d8125b5c1ee0eddc");
                        var serlizeOnbj = JsonConvert.SerializeObject(model);

                        request.Content = new StringContent(serlizeOnbj);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        var response = await httpClient.SendAsync(request);
                        var resContent = response.Content.ReadAsStringAsync().Result;
                        var responseConvert = JsonConvert.DeserializeObject<APIResponceCodes>(resContent);
                        //if (responseConvert.Description == "Email Sent Successfully.")
                        //{
                        //    responseConvert.success = true;
                        //}
                        //else { responseConvert.success = false; }

                        return Ok(responseConvert);

                    }
                }


            }
            catch (Exception ex)
            {
                code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "Post/Model",
                    Description = ex.Message,
                    //DescriptionArabic = "لم يتم العثور على سجل",
                    //Data = false
                    //success = false
                };
            }



            #region prev api work
            ////APIResponceCodes code = null;
            //if (model != null && !string.IsNullOrEmpty(model.CivilId) && !string.IsNullOrEmpty(model.MemberNo) &&
            //    !string.IsNullOrEmpty(model.MemberName) && !string.IsNullOrEmpty(model.PassportNo) && !string.IsNullOrEmpty(model.PolicyNo))
            //{
            //    #region other libraries
            //    //Byte[] bytes = File.ReadAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate.pdf"));
            //    //String file = Convert.ToBase64String(bytes);


            //    // //Load an existing Word document
            //    // WordDocument wordDocument = new WordDocument(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English and Arabic.doc"), FormatType.Doc);
            //    // //Create an instance of DocToPDFConverter
            //    // DocToPDFConverter converter = new DocToPDFConverter();
            //    // //Convert Word document into PDF document
            //    // PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);
            //    // //Save the PDF file
            //    // pdfDocument.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English and Arabic.pdf"));
            //    // //Close the instance of document objects
            //    // pdfDocument.Close(true);
            //    // wordDocument.Close();
            //    // //This will open the PDF file so, the result will be seen in default PDF viewer
            //    ////System.Diagnostics.Process.Start("WordtoPDF.pdf");


            //    //// For complete examples and data files, please go to https://github.com/aspose-words/Aspose.Words-for-.NET
            //    //// Load the document from disk.
            //    //Document doc = new Document(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English and Arabic.doc"));

            //    //PdfSaveOptions pso = new PdfSaveOptions();
            //    //pso.Compliance = PdfCompliance.Pdf17;
            //    //// Save the document in PDF format.
            //    //doc.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English and Arabic.pdf"), pso);


            //    //PdfDocument pdf = PdfGenerator.GeneratePdf(html, PageSize.Letter);
            //    //pdf.PageLayout = PdfPageLayout.SinglePage;
            //    //pdf.PageMode = PdfPageMode.FullScreen;
            //    //pdf.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Afya Travel Insurance Certificate - English.pdf"));
            //    #endregion
            //    try
            //    {
            //        var certExist = travelDAL.GetCertDetails(model.MemberNo, model.PolicyNo);
            //        if (certExist != null && certExist.MEMBER_NO == model.MemberNo && certExist.POLICY_NO == model.PolicyNo)
            //        {
            //            code = new APIResponceCodes()
            //            {
            //                Code = "CD-06",
            //                Type = "Post/Model",
            //                Description = "Certificate Already Exist.",
            //                //DescriptionArabic = "لم يتم العثور على سجل",
            //                //Data = false
            //            };
            //        }
            //        else
            //        {
            //            var memberInfo = travelDAL.GetMemberInfo(model.MemberNo);
            //            string fileHash = DateTime.UtcNow.ToString("ddMMyyyyHHmmss");
            //            //string sourceHtmlFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/TravelInsuranceCertificate.htm");
            //            //string destHtmlFileName = "TravelInsuranceCertificate - Latest.htm";
            //            string destPdfFileName = fileHash + "_TravelInsuranceCertificate.pdf";
            //            //string destHtmlFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + destHtmlFileName);
            //            string destPdfFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + destPdfFileName);
            //            //string html = File.ReadAllText(sourceHtmlFile);
            //            //html = html.Replace("@PolicyNo", model.PolicyNo).Replace("@MemberName", model.MemberName).Replace("@PassportNo", model.PassportNo).Replace("@CivilId", model.CivilId).Replace("@DOB", memberInfo.DATE_OF_BIRTH.ToString("dd/MM/yyyy"));
            //            //File.WriteAllText(destHtmlFile, html);
            //            //var Renderer = new IronPdf.ChromePdfRenderer();
            //            //Renderer.RenderingOptions.MarginTop = 0;  //millimeters
            //            //Renderer.RenderingOptions.MarginBottom = 0;
            //            //Renderer.RenderingOptions.MarginLeft = 10;
            //            //Renderer.RenderingOptions.MarginRight = 0;
            //            //var PDF = Renderer.RenderHtmlFileAsPdf(destHtmlFile);
            //            //PDF.SaveAs(destPdfFile);

            //            //var pdf = PdfHelper.GetPdf("~/Files/Afya Travel Insurance Certificate - English.cshtml");

            //            //PdfDocument pdf = PdfGenerator.GeneratePdf(html, PageSize.Letter);
            //            //pdf.PageLayout = PdfPageLayout.SinglePage;
            //            //pdf.PageMode = PdfPageMode.FullScreen;
            //            //pdf.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + destPdfFileName));

            //            //Byte[] bytes = File.ReadAllBytes(destPdfFile);
            //            //String file = Convert.ToBase64String(bytes);

            //            //Save in database
            //            //string destPdfFileName = model.CivilId + "_" + model.PolicyNo + "_TravelInsuranceCertificate.pdf";
            //            //string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Print/PrintPDF" + "?memberNo=" + model.MemberNo + "&policyNo=" + model.PolicyNo;
            //            string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Files/" + destPdfFileName;

            //            //Word to PDF

            //            string sourceDocFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/AfyaTravelInsuranceCertificateOriginal.docx");
            //            string destDocFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + fileHash + "_TravelInsuranceCertificate.docx");
            //            if (!File.Exists(destDocFile))
            //            {
            //                File.Copy(sourceDocFile, destDocFile);
            //            }
            //            AfyaTravelCert certObj = new AfyaTravelCert()
            //            {
            //                MEMBER_NAME = model.MemberName,
            //                PASSPORT_NO = model.PassportNo,
            //                CIVIL_ID = model.CivilId,
            //                DOB = memberInfo.DATE_OF_BIRTH.ToString("dd/MM/yyyy")
            //            };
            //            ConvertDoc(certObj, destDocFile, destPdfFile, WdSaveFormat.wdFormatPDF);
            //            if (File.Exists(destDocFile))
            //            {
            //                File.Delete(destDocFile);
            //            }
            //            AfyaTravelCert cert = new AfyaTravelCert()
            //            {
            //                CERT_FILE = destPdfFileName,
            //                CERT_HASH = fileHash,
            //                CIVIL_ID = model.CivilId,
            //                MEMBER_NAME = model.MemberName,
            //                MEMBER_NO = model.MemberNo,
            //                PASSPORT_NO = model.PassportNo,
            //                POLICY_NO = model.PolicyNo,
            //                SOURCE = "api"
            //            };
            //            int save = travelDAL.SaveCertDetails(cert);
            //            if (save > 0)
            //            {

            //                code = new APIResponceCodes()
            //                {
            //                    Code = "CD-03",
            //                    Type = "Post/Model",
            //                    Description = "Certificate Generated Successfully.",
            //                    DescriptionArabic = "تم إنشاء الشهادة بنجاح",
            //                    Data = printPath
            //                };
            //            }
            //            else
            //            {
            //                code = new APIResponceCodes()
            //                {
            //                    Code = "CD-06",
            //                    Type = "Post/Model",
            //                    Description = "Certificate Not Generated",
            //                    DescriptionArabic = "لم يتم إنشاء الشهادة",
            //                    //Data = false
            //                };
            //            }
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        code = new APIResponceCodes()
            //        {
            //            Code = "CD-06",
            //            Type = "Post/Model",
            //            Description = ex.Message,
            //            //DescriptionArabic = "لم يتم العثور على سجل",
            //            //Data = false
            //        };
            //    }

            //}
            //else
            //{
            //    code = new APIResponceCodes()
            //    {
            //        Code = "CD-00",
            //        Type = "Post/Model",
            //        Description = "Input Value Missing",
            //        DescriptionArabic = "الرجاء إدخال النص المفقود"
            //    };
            //}
            #endregion

            return Ok(code);
        }

        [Authorize]
        [Route("GetQuestionsAndAnswers")]
        [HttpGet]
        public IHttpActionResult GetQuestionsAndAnswers()
        {
            APIResponceCodes code = null;
            //Byte[] bytes = File.ReadAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Questions And Answers.pdf"));
            //String file = Convert.ToBase64String(bytes);
            string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Files/Important Information for Afya Travel-Q-A.pdf";
            code = new APIResponceCodes()
            {
                Code = "CD-01",
                Type = "Get",
                Description = "Sent Successfully.",
                //DescriptionArabic = "الرجاء إدخال النص المفقود",
                Data = printPath
            };
            return Ok(code);
        }

        [Authorize]
        [Route("GetTermsAndConditions")]
        [HttpGet]
        public IHttpActionResult GetTermsAndConditions()
        {
            APIResponceCodes code = null;
            //Byte[] bytes = File.ReadAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Terms And Conditions.pdf"));
            //String file = Convert.ToBase64String(bytes);
            string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Files/AFYA Wording - Client Contract.pdf";
            code = new APIResponceCodes()
            {
                Code = "CD-01",
                Type = "Get",
                Description = "Sent Successfully.",
                //DescriptionArabic = "الرجاء إدخال النص المفقود",
                Data = printPath
            };
            return Ok(code);
        }

        [Authorize]
        [Route("PrintCertificate")]
        [HttpPost]
        public IHttpActionResult PrintCertificate(CertificateRequest model)
        {
            APIResponceCodes code = null;
            if (model != null && !string.IsNullOrEmpty(model.CivilId) && !string.IsNullOrEmpty(model.MemberNo) && !string.IsNullOrEmpty(model.PolicyNo))
            {
                var certExist = travelDAL.GetCertDetails(model.MemberNo, model.PolicyNo);
                if (certExist != null && certExist.MEMBER_NO == model.MemberNo && certExist.POLICY_NO == model.PolicyNo)
                {
                    //Byte[] bytes = File.ReadAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + certExist.CERT_FILE));
                    //String file = Convert.ToBase64String(bytes);
                    //string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Print/PrintPDF" + "?memberNo=" + model.MemberNo + "&policyNo=" + model.PolicyNo;
                    string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Files/" +certExist.CERT_FILE;
                    code = new APIResponceCodes()
                    {
                        Code = "CD-03",
                        Type = "Post/Model",
                        Description = "Certificate Printed Successfully.",
                        DescriptionArabic = "تمت طباعة الشهادة بنجاح",
                        Data = printPath
                    };
                }
                else
                {
                    code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "Post/Model",
                        Description = "Certificate Not Found.",
                        DescriptionArabic = "لم يتم العثور على الشهادة",
                        //Data = false
                    };
                }
            }
            else
            {
                code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
            }
            return Ok(code);
        }
        [Authorize]
        [Route("GetCertificateDetails")]
        [HttpPost]
        public IHttpActionResult GetCertificateDetails(CertificateRequest model)
        {
            APIResponceCodes code = null;
            if (model != null && !string.IsNullOrEmpty(model.CivilId) && !string.IsNullOrEmpty(model.MemberNo) && !string.IsNullOrEmpty(model.PolicyNo))
            {
                var certExist = travelDAL.GetCertDetails(model.MemberNo, model.PolicyNo);
                if (certExist != null && certExist.MEMBER_NO == model.MemberNo && certExist.POLICY_NO == model.PolicyNo)
                {
                    model.PrintDate = certExist.CERT_PRINT_DATE;
                    model.MemberName = certExist.MEMBER_NAME;
                    model.PassportNo = certExist.PASSPORT_NO;
                    code = new APIResponceCodes()
                    {
                        Code = "CD-03",
                        Type = "Post/Model",
                        Description = "Sent Successfully.",
                        //DescriptionArabic = "الرجاء إدخال النص المفقود",
                        Data = model
                    };
                }
                else
                {
                    code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "Post/Model",
                        Description = "Certificate Not Found.",
                        //DescriptionArabic = "لم يتم العثور على سجل",
                        //Data = false
                    };
                }

            }
            else
            {
                code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
            }
            return Ok(code);
        }

        //[Authorize]
        [Route("SendEmail")]
        [HttpPost]
        public IHttpActionResult SendEmail(CertificateRequest model)
        {
            APIResponceCodes code = null;
            if (model != null && !string.IsNullOrEmpty(model.CivilId) && !string.IsNullOrEmpty(model.MemberNo) && !string.IsNullOrEmpty(model.PolicyNo) && !string.IsNullOrEmpty(model.Email))
            {
                try
                {
                    var certExist = travelDAL.GetCertDetails(model.MemberNo, model.PolicyNo);
                    if (certExist != null && certExist.MEMBER_NO == model.MemberNo && certExist.POLICY_NO == model.PolicyNo)
                    {
                        //string printPath = ConfigurationManager.AppSettings["PrintPath"] + "/Print/PrintPDF" + "?memberNo=" + model.MemberNo + "&policyNo=" + model.PolicyNo;
                        //string emailMessage = "عزيزي العميل <br><br>يرجى الضغط على الرابط  لتحميل شهادة تأمين السفر  <br><a href='" + printPath + "'>Download</a> <br><br>";
                        string emailMessage = "<table border='0' style=' width: 100%;'><tr><td style='text-align:right'><span lang=AR-SA dir=RTL>السيد / السيدة حامل بطاقة عافية المحترم <br>يسرنا أن نرفق لكم شهادة تأمين السفر وشروط الوثيقة ومعلومات هامة بهذا الخصوص .<br>متمنين لكم رحلة سعيدة  <br><br>مجموعة الخليج للتامين<br><br></span></td></tr></table>";
                        IComplaints sendMail = new ComplaintDAL();
                        var settingResult = sendMail.GetSMTPSetting().FirstOrDefault();
                        var emailTo = ConfigurationManager.AppSettings["EmailOnRegistrationFailure"];
                        MailAddress sendFrom = new MailAddress(settingResult.EmailForm);
                        MailAddress sendTo = new MailAddress(model.Email);
                        MailMessage message = new MailMessage(sendFrom, sendTo);
                        Attachment attachmentCert = new Attachment(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/" + certExist.CERT_FILE));
                        Attachment attachmentQnA = new Attachment(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Important Information for Afya Travel-Q-A.pdf"));
                        Attachment attachmentTerms = new Attachment(System.Web.Hosting.HostingEnvironment.MapPath("~/Files/AFYA Wording - Client Contract.pdf"));
                        message.Attachments.Add(attachmentCert);
                        message.Attachments.Add(attachmentQnA);
                        message.Attachments.Add(attachmentTerms);
                        message.Subject = "تأمين السفر- عافية";
                        message.Body = emailMessage;
                        message.IsBodyHtml = true;


                        System.Net.NetworkCredential nc = new System.Net.NetworkCredential(settingResult.UserName, settingResult.Password);
                        SmtpClient mailClient = new SmtpClient(settingResult.SMTPHost, settingResult.SMTPPort);
                        mailClient.UseDefaultCredentials = false;
                        mailClient.Credentials = nc;
                        mailClient.Send(message);

                        code = new APIResponceCodes()
                        {
                            Code = "CD-03",
                            Type = "Post/Model",
                            Description = "Email Sent Successfully.",
                            DescriptionArabic = "تم إرسال البريد الإلكتروني بنجاح",
                            Data = true
                        };
                    }
                    else
                    {
                        code = new APIResponceCodes()
                        {
                            Code = "CD-06",
                            Type = "Post/Model",
                            Description = "Certificate Not Found.",
                            DescriptionArabic = "لم يتم العثور على الشهادة",
                            //Data = false
                        };
                    }
                }
                catch (Exception ex)
                {
                    code = new APIResponceCodes()
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
                code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
            }
            return Ok(code);
        }

        [Authorize]
        [Route("MemberCertificateExist")]
        [HttpPost]
        public IHttpActionResult MemberCertificateExist(CertificateRequest model)
        {
            APIResponceCodes code = null;
            if (model != null && !string.IsNullOrEmpty(model.CivilId) && !string.IsNullOrEmpty(model.MemberNo) && !string.IsNullOrEmpty(model.PolicyNo))
            {
                var certExist = travelDAL.GetCertDetails(model.MemberNo, model.PolicyNo);
                if (certExist != null && certExist.MEMBER_NO == model.MemberNo && certExist.POLICY_NO == model.PolicyNo)
                {
                    code = new APIResponceCodes()
                    {
                        Code = "CD-03",
                        Type = "Post/Model",
                        Description = "Member Certificate Exist.",
                        //DescriptionArabic = "الرجاء إدخال النص المفقود",
                        Data = true
                    };
                }
                else
                {
                    code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "Post/Model",
                        Description = "Certificate Not Found.",
                        //DescriptionArabic = "لم يتم العثور على سجل",
                        Data = false
                    };
                }

            }
            else
            {
                code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
            }
            return Ok(code);
        }
        public void ConvertDoc(AfyaTravelCert model, string input, string output, WdSaveFormat format)
        {
            // Create an instance of Word.exe
            Word._Application oWord = new Word.Application();

            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object isVisible = true;
            object readOnly = false;
            object oInput = input;
            object oOutput = output;
            object oFormat = format;

            // Load a document into our instance of word.exe
            Word._Document oDoc = oWord.Documents.Open(ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Make this document the active document.
            oDoc.Activate();

            //Replace Text
            FindAndReplace(oWord as Application, "{MemberName}", model.MEMBER_NAME);
            FindAndReplace(oWord as Application, "{PassportNo}", model.PASSPORT_NO);
            FindAndReplace(oWord as Application, "{CivilId}", model.CIVIL_ID);
            FindAndReplace(oWord as Application, "{DOB}", model.DOB);

            // Save this document in Word 2003 format.
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            //Close Document
            oDoc.Close();
            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
        }
        private void FindAndReplace(Microsoft.Office.Interop.Word.Application doc, object findText, object replaceWithText)
        {
            //options
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            //execute find and replace
            doc.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
                ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }
    
        
        }
}
