
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.LImitsDAL;
using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MemberPortalGICWebApi.DataObjects.Search;
//using static MemberPortalGICWebApi.DataObjects.MNI_SERVICES;

namespace MemberPortalGICWebApi.Controllers
{
    [RoutePrefix("api/Limits")]
    public class LimitsController : ApiController
    {
        ILimits repo = new LimitsDAL();

        [Authorize]
        [Route("GetPolicies")]
        [HttpPost]
        public IHttpActionResult GetPolicyByCivilID(LimitCheckModel inputmodel)
        {
            if (inputmodel.CivilId != null || inputmodel.CivilId != "")
            {
                var result = repo.GetPolicybyCivilId(inputmodel);
                if (result.Count > 0)
                {
                    // result.Password = Common.Decrypt(result.Password);
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "GetPolicyByCivilID Success",
                        DescriptionArabic = "GetPolicyByCivilID Success",
                        Data = result
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
        [Route("GetPersonalInformation")]
        [HttpPost]
        public IHttpActionResult PolicyVerficationByMPOL(GetPersonalInformationModel inputmodel)
        {
            if (inputmodel.MemberNumber != null & inputmodel.MemberNumber != "" & inputmodel.PolicyNumber != null & inputmodel.PolicyNumber != "")
            {
                var result = repo.GetPatientBasicInfo(inputmodel);
                if (result != null)
                {
                    // result.Password = Common.Decrypt(result.Password);
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "GetPersonalInformation by PolicyNo and MemeberNumber Success",
                        DescriptionArabic = "GetPersonalInformation by PolicyNo and MemeberNumber Success",
                        Data = result
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

        ///[Authorize]
        [Route("GetBenfitsView")]
        [HttpPost]
        public IHttpActionResult GetBenfitsView(GetlimitsModel model)
        {
            if (model.CivilId != "")
            {
                GeneralDAL generalDAL = new GeneralDAL();
                   var inputmodel = repo.GetMemberNo(model.CivilId, model.PolicyNumber);
                if (inputmodel != null)
                {
                    //Gender Call

                    var genderResult = repo.GetMemberGender(inputmodel.MemberNumbers);

                    MNI_SERVICES repoServices = new MNI_SERVICES();

                    var result = repoServices.CustomTOGetBenefitsAndPlans(Convert.ToInt64(model.PolicyNumber), Convert.ToInt64(inputmodel.MemberNumbers));
                    if (result != null && result.Count > 0)
                    {
                        ISearch Object = new SearchDAL();
                        var data = Object.GetBenefitsMappingList();

                        int count1 = 0, count2 = 0;
                        foreach (var item in result)
                        {


                            item.TotlaAmount = String.Format("{0:0.000}", item.TotlaAmounts);
                            item.UsedAmount = String.Format("{0:0.000}", item.UsedAmounts);

                            if (data != null && data.Count() > 0)
                            {
                                var PlanDescrptionObj = data.Where(x => x.PlanNumber == item.PlanNumber).Select(x => new BenefitsMappingList { PlanDescEN = x.PlanDescEN, PlanDescAR = x.PlanDescAR }).FirstOrDefault();

                                if (PlanDescrptionObj != null)
                                {
                                    //item.PlanDescrptions = PlanDescrptionObj.PlanDescEN + " – " + PlanDescrptionObj.PlanDescAR;
                                    item.PlanDescrptions = PlanDescrptionObj.PlanDescEN;
                                    item.PlanDescrptionsAR = PlanDescrptionObj.PlanDescAR;
                                }
                            }


                            //if (item.PlanDescrptions == "Dental Retirees Plan")
                            //{
                            //    item.PlanDescrptions = "Dental Plan – منفعة الأسنان";
                            //}
                            //if (item.PlanDescrptions == "Inpatient Retirees Plan")
                            //{
                            //    item.PlanDescrptions = " Inpatient Plan – منفعة داخل المستشفى";
                            //}
                            //if (count1 == 0 && genderResult.GenderId == "F")
                            //{
                            //    if (item.PlanDescrptions == "Inpatient Maternity KPC Plan" || item.PlanDescrptions == "Outpatient Maternity Plan")
                            //    {
                            //        //item.PlanDescrptions = "Maternity Benefit – منفعة الحمل و الولادة";
                            //        item.PlanDescrptions = "Maternity Benefit";
                            //        item.PlanDescrptionsAR = "منفعة الحمل و الولادة";
                            //        count1++;
                            //    }
                            //}
                            //if (count2 == 0)
                            //{
                            //    if (item.PlanDescrptions == "Chronic Retirees Plan" || item.PlanDescrptions == "Outpatient Retirees Plan")
                            //    {
                            //        //item.PlanDescrptions = "Outpatient and Chronic – منفعة العيادات الخارجية والأمراض المزمنة";
                            //        item.PlanDescrptions = "Outpatient and Chronic";
                            //        item.PlanDescrptionsAR = "منفعة العيادات الخارجية والأمراض المزمنة";
                            //        count2++;
                            //    }
                            //}
                        }
                        if (genderResult.GenderId != "F")
                        {
                            result.RemoveAll(x => x.PlanDescrptions.Contains("Maternity"));
                            //result.RemoveAll(x => x.PlanDescrptions == "Outpatient Maternity Plan");
                            // result.RemoveAll(x => x.PlanDescrptions == "Inpatient Maternity Plan");
                            // result.RemoveAll(x => x.PlanDescrptions == "Outpatient Maternity KPC Plan");
                        }


                        result.RemoveAll(x => x.PlanDescrptions.Contains("Excluded/Exgratia Covered Benefits"));
                        result.RemoveAll(x => x.PlanDescrptions == "Excluded/Exceptionally Covered Benefits");
                        result.RemoveAll(x => x.PlanDescrptions == "Assist America");
                        result.RemoveAll(x => x.PlanDescrptions == "Optical EQUATE Plan");
                        result.RemoveAll(x => x.PlanDescrptions == "Optical KPC Plan");
                        result.RemoveAll(x => x.PlanDescrptions == "AFYA3 GIG & MRe Reinsurance");
                        result.RemoveAll(x => x.PlanDescrptions == "Inpatient Plan -KJO");
                       
                        var genCOfig = generalDAL.GetConfigration();

                        //New Implementaions: 
                        if (model.PolicyNumber == genCOfig.ACTIVE_AFYA_POLICY_NO.ToString() || model.PolicyNumber == genCOfig.ACTIVE_AFYA_3_POLICY_NO.ToString())
                        {
                            if (result.Any(x => x.PlanDescrptions == "Chronic  Plan"))
                            {
                                result.RemoveAll(x => x.PlanDescrptions == "Chronic  Plan");
                                result.Where(w => w.PlanDescrptions == "Outpatient Plan").ToList().ForEach(s => { s.PlanDescrptions = "Outpatient & Chronic Plan"; s.PlanDescrptionsAR = "منفعة العيادات الخارجية وعلاج الأمراض المزمنة"; });
                            }
                        }

                        // result.Password = Common.Decrypt(result.Password);
                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-01",
                            Type = "Get",
                            Description = "GetBenfitsViewInformation by PolicyNo and MemeberNumber Success",
                            DescriptionArabic = "GetBenfitsViewInformation by PolicyNo and MemeberNumber Success",
                            Data = result
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
    }
}