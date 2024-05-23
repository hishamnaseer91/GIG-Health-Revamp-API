using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.Family;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.Search;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemberPortalGICWebApi.Controllers
{
    [RoutePrefix("api/Family")]
    public class FamilyController : ApiController
    {
        IFamily obj = new FamilyDAL();

        #region ActivePolicies

        [HttpGet]
       
        public IHttpActionResult GetActivePolicyByCivilID(string civilId)
        {
            if (!string.IsNullOrEmpty(civilId))
            {
                IList<Policy> data = obj.GetPolicybyCivilId(civilId);

                if (data != null)
                {

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "Get Active Policies through Civil ID",
                        DescriptionArabic = "Get Active Policies through Civil ID",
                        Data = data
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
                        DescriptionArabic = "No Record Found",
                        Data = data
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

        #endregion

        #region"FamilyDetails"

       
        [Route("GetFamilyDetails")]
        [HttpGet]       
        public IHttpActionResult FamilyDetails(string MemNo = "", string PolNo = "")
        {

            if (!string.IsNullOrEmpty(MemNo)  && !string.IsNullOrEmpty(PolNo))
            {
                var data = obj.GetFamilyDetails(Convert.ToInt64(MemNo), Convert.ToInt64(PolNo));
                data = data.Where(x => x.PrincipleMember == "N").ToList();

                if (data != null && data.Count() > 0)
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "Get FamilyDetails through policy and memno",
                        DescriptionArabic = "Get FamilyDetails through policy and memno",
                        Data = data
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
                        //DescriptionArabic = "لم يتم العثور على سجل",
                        DescriptionArabic = "لم يتم العثور على سجلات",
                        Data = data
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


        [Route("GetBenefitsDetails")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult BenefitsLimitsDetails(string MemNo = "", string PolNo = "")
        {

            if (!string.IsNullOrEmpty(MemNo) && !string.IsNullOrEmpty(PolNo))
            {
                MNI_SERVICES MNI = new MNI_SERVICES();
                var result = MNI.CustomTOGetBenefitsAndPlans(Convert.ToInt64(PolNo), Convert.ToInt64(MemNo));
                if (result != null)
                {
                    ISearch Object = new SearchDAL();
                    var data = Object.GetBenefitsMappingList();
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
                    }



                    result.RemoveAll(x => x.PlanDescrptions == "Excluded/Exgratia Covered Benefits");
                    result.RemoveAll(x => x.PlanDescrptions == "Excluded/Exceptionally Covered Benefits");
                    result.RemoveAll(x => x.PlanDescrptions == "Assist America");
                    result.RemoveAll(x => x.PlanDescrptions == "Optical EQUATE Plan");
                    result.RemoveAll(x => x.PlanDescrptions == "Optical KPC Plan");
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "Get BenefitsLimitsDetails through policy and memno",
                        DescriptionArabic = "Get BenefitsLimitsDetails through policy and memno",
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
                        DescriptionArabic = "لم يتم العثور على سجل",
                        Data = result
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



        #endregion

    }
}
