using Rotativa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MemberPortalGICWebApi.Models;
using MemberPortalGICWebApi.DataObjects.TravelDAL;
using MemberPortalGICWebApi.DataObjects.Interfaces;

namespace MemberPortalGICWebApi.Controllers
{
    public class PrintController : Controller
    {
        ITravel travelDAL = new TravelDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PrintPDF(string memberNo, string policyNo)
        {
            try
            {
                if (!string.IsNullOrEmpty(memberNo) && !string.IsNullOrEmpty(policyNo))
                {
                    var memberInfo = travelDAL.GetMemberInfo(memberNo);
                    var certExist = travelDAL.GetCertDetails(memberNo, policyNo);
                    if (certExist != null && certExist.MEMBER_NO == memberNo && certExist.POLICY_NO == policyNo)
                    {
                        certExist.DOB = memberInfo.DATE_OF_BIRTH.ToString("dd/MM/yyyy");
                        return new PartialViewAsPdf("_PrintPdf", certExist)
                        {
                            FileName = "TravelInsuranceCertificate.pdf",
                            PageSize = Rotativa.Options.Size.Letter,
                             IsLowQuality = false,
                              MinimumFontSize = 16,
                               //PageWidth = 600
                            //PageMargins = new Rotativa.Options.Margins() { Right = 0 }
                        };
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }
    }
}