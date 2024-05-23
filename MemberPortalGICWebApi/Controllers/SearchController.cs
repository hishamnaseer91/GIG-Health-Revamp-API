using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.Family;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.Search;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MemberPortalGICWebApi.Controllers
{
    [RoutePrefix("api/Search")]
    public class SearchController : ApiController
    {
        ISearch repo = new SearchDAL();

        [Route("Plans")]
        [HttpPost]
        public IHttpActionResult DALPPlansypeDDL(DDLProviderTypeInput model)
        {


            var list = repo.DALPlanListDDL(model.Culture);
            if (list.Count > 0)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-05",
                    Type = "POST/Model",
                    Description = "Get Plans list Success",
                    DescriptionArabic = "Get Plans list Success",
                    Data = list
                };
                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "POST/Model",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }
            //return Ok(list);

        }

        [Route("NetworkPolicy")]
        [HttpPost]

        public IHttpActionResult DALPolicyDDL(DDLProviderTypeInput model)
        {
            if (string.IsNullOrEmpty(model.CivilId) && model.PlanId > 0)
            {

                var list = repo.DALNetworkListDDL(model.Culture, model.PlanId);
                if (list.Count > 0)

                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "POST/Model",
                        Description = "Get Plans list Success",
                        DescriptionArabic = "Get Plans list Success",
                        Data = list
                    };
                    return Ok(code);
                }
                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
                        Description = "No Record Found",
                        DescriptionArabic = "لم يتم العثور على سجل"

                    };
                    return Ok(code);
                }
            }
            else if (!string.IsNullOrEmpty(model.CivilId) && model.PlanId <= 0)
            {

                var list = repo.DALNetworkListDDLbyCivilID(model.Culture, model.CivilId);
                if (list.Count > 0)

                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "POST/Model",
                        Description = "Get Plans list Success",
                        DescriptionArabic = "Get Plans list Success",
                        Data = list
                    };
                    return Ok(code);
                }
                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
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
                    Type = "Get/Model",
                    Description = "Input Value is Missing",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }
            //return Ok(list);

        }

        [Route("ProviderType")]
        [HttpPost]

        public IHttpActionResult DALProviderTypeDDL(DDLProviderTypeInput model)
        {


            var list = repo.DALProviderTypeDDL(model.Culture).ToList();
            if (list.Count > 0)

            {
                //list.RemoveAll(x => x.ProviderTypeID == "O"); //remove optical 
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-05",
                    Type = "POST/Model",
                    Description = "Get ProviderType list Success",
                    DescriptionArabic = "Get ProviderType list Success",
                    Data = list
                };
                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "POST/Model",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }
            //return Ok(list);

        }

        [Route("Providers")]
        [HttpPost]

        public IHttpActionResult DALProviderDDL(DDLProviderInput model)
        {
            if (model.ProviderTypeID != "")
            {
                var list = repo.DALProviderDDL(model.Culture, model.ProviderTypeID);
                if (list.Count > 0)
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "POST/Model",
                        Description = "Get Provider list Success",
                        DescriptionArabic = "Get Provider list Success",
                        Data = list
                    };
                    return Ok(code);
                }

                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
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
                    Code = "CD-07",
                    Type = "POST/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"

                };
                return Ok(code);
            }
            //return Ok(list);
        }

        [Route("Speciality")]
        [HttpPost]
        public IHttpActionResult DALSpecialtyDDL(DDLSpecialtyInput model)
        {

            var list = repo.DALSpecialtyDDL(model.Culture).ToList();

            if (list.Count > 0)
            {
                list.RemoveAll(x => x.SpecialtyName == null);
                //ODN
                list.RemoveAll(x => x.SpecialtyID == "ODN");
                //
                if (string.IsNullOrEmpty(model.ProviderTypeID))
                {

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "POST/Model",
                        Description = "Get Speciality list Success",
                        DescriptionArabic = "Get Speciality list Success",
                        Data = list
                    };
                    return Ok(code);
                }

                else
                {
                    if (model.ProviderTypeID == "D")
                    {
                        list.RemoveAll(x => x.SpecialtyID == "GDN");
                    }
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "POST/Model",
                        Description = "Get Speciality list Success",
                        DescriptionArabic = "Get Speciality list Success",
                        Data = list
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
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }
            //return Ok(list);
        }


        [HttpGet]
        public IHttpActionResult GetNetworkListCardPickup(int planID, string Culture)
        {

            var searchFuncs = repo.BIZNetworkListDDLCardPickup(Culture, planID) as List<DDLNetworkCardMapping>;

            if (searchFuncs != null && searchFuncs.Count > 0)
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-05",
                    Type = "POST/Model",
                    Description = "GetNetworkListCardPickup Success",
                    DescriptionArabic = "GetNetworkListCardPickup Success",
                    Data = searchFuncs
                };
                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "POST/Model",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }
        }

        [Route("Regions")]
        [HttpPost]

        public IHttpActionResult DALRegionDDL(DDLRegionInput model)
        {

            var list = repo.DALRegionDDL(model.Culture);
            if (list.Count > 0)
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-05",
                    Type = "POST/Model",
                    Description = "Get Regions list Success",
                    DescriptionArabic = "Get Regions list Success",
                    Data = list
                };
                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "POST/Model",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }
            //return Ok(list);
        }

        [Route("LocationTypes")]
        [HttpPost]

        public IHttpActionResult DALLocationDDL(DDLLocationTypes model)
        {

            var list = repo.DALLocationTypeDDL(model.Culture);
            if (list.Count > 0)
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-05",
                    Type = "POST/Model",
                    Description = "Get Location Types Success",
                    //  DescriptionArabic = "Get Regions list Success",
                    Data = list
                };
                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "POST/Model",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }
            //return Ok(list);
        }

        [Route("Area")]
        [HttpPost]

        public IHttpActionResult DALAreaDDL(DALAreaDDLInput model)
        {

            if (model.RegionId != "")
            {
                var list = repo.DALAreaDDL(model.Culture, Convert.ToInt32(model.RegionId));
                if (list.Count > 0)
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "POST/Model",
                        Description = "Get Area list Success",
                        DescriptionArabic = "Get Area list Success",
                        Data = list
                    };
                    return Ok(code);
                }

                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
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
                    Code = "CD-07",
                    Type = "POST/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"

                };
                return Ok(code);
            }

            //return Ok(list);
        }

        [Route("SearchProvider")]
        [HttpPost]
        public IHttpActionResult Search(SearchModel modelinput)
        {

            if (modelinput.ProviderType != "" & modelinput.CivilId != "")
            {
                // var networkId = repo.GetNetworkId(modelinput.CivilId);
                var networkId = Convert.ToInt32(!string.IsNullOrEmpty(modelinput.NetworkId) ? modelinput.NetworkId : "0");
                if (networkId > 0)
                {
                    SearchResultModel Model = new SearchResultModel();
                    modelinput.NetworkId = networkId.ToString();
                    Model.Search = modelinput;
                    var list = repo.DALSearchResult(modelinput, networkId.ToString());

                    if (list.Count > 0)
                    {
                        if (modelinput.ProviderType == "E" || modelinput.ProviderType == "D")
                        {

                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-05",
                                Type = "POST/Model",
                                Description = "Get Search Result list Success",
                                DescriptionArabic = "Get Search Resultlist Success",
                                Data = list
                            };
                            return Ok(code);

                        }

                        else
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-05",
                                Type = "POST/Model",
                                Description = "Get Search Result list Success",
                                DescriptionArabic = "Get Search Resultlist Success",
                                Data = list
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
                        Description = "GetNetworkId Failure",
                        DescriptionArabic = "GetNetworkId Failue"

                    };
                    return Ok(code);
                }

            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-07",
                    Type = "POST/Model",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"

                };
                return Ok(code);
            }
        }


        [Route("CoveregeBalance")]
       [Authorize]
        [HttpPost]
        public IHttpActionResult GetCoveregeBalanceDocument(string PackageNumber)
        {
             
            var CoveregeName = string.Empty;
            if (!string.IsNullOrEmpty(PackageNumber))
            {
                CoveregeName = repo.GetCoveregeName(PackageNumber);
            }
            if (CoveregeName != "-1")

            {
                //var AccesCoveregeDocument = ConfigurationManager.AppSettings["CoveregeDocURL"] + CoveregeName;
                string imgbase64_get =   GetFileAsBase64(CoveregeName);
                Coverege AccesCoveregeDocument = new Coverege();
                AccesCoveregeDocument.Base64= imgbase64_get;
                AccesCoveregeDocument.FileName = CoveregeName;
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-05",
                    Type = "POST/Model",
                    Description = "Get Coverge Balance Success",
                    Data = AccesCoveregeDocument
                };
                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "POST/Model",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"

                };
                return Ok(code);
            }
        }

        public string GetFileAsBase64(string fileName)
        {
            string GlobalVar_FileFtpPath = ConfigurationManager.AppSettings["GlobalVar_FileFtpPath"];
            string GlobalVar_FileNetworkUser = ConfigurationManager.AppSettings["GlobalVar_FileNetworkUser"];
            string GlobalVar_FileNetworkPass = ConfigurationManager.AppSettings["GlobalVar_FileNetworkPass"];
            // FTP server settings
            string ftpServer = GlobalVar_FileFtpPath + fileName;
            //string remoteFilePath = GicCipCommon.GlobalVar.InsuranceAdviceFilePath + fileName;

            using (WebClient client = new WebClient())
            {
                try
                {
                    // Set FTP credentials
                    client.Credentials = new NetworkCredential(GlobalVar_FileNetworkUser, GlobalVar_FileNetworkPass);

                    // Download the file from the FTP server 
                    byte[] fileData = client.DownloadData(ftpServer);

                    // Encode the file data as base64 data:image/jpeg;base64,
                    return Convert.ToBase64String(fileData);
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    //return "Error: " + ex.Message; // You might want to handle errors more gracefully
                    return "";
                }
            }
        }
        [Route("SmsVisibility")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult SmsVisibilityDetails(string CivilID, string PolicyNo)
        {
            SmsVisibility response = new SmsVisibility();
            response.isClaimRegisterSmsVisibility = "False";
            response.isSmsClaimRegisterApplicable = "False";

            var AfyaPolicyNumber = repo.ClaimRegisterSmsVisibility(CivilID, PolicyNo);
            if (AfyaPolicyNumber != "-1")
            {
                response.isClaimRegisterSmsVisibility = "True";

                var IsCivilIDExcluded = repo.ClaimRegisterEnable(CivilID);
                if (IsCivilIDExcluded == "-1")
                {
                    response.isSmsClaimRegisterApplicable = "True";
                }
            }

            APIResponceCodes code = new APIResponceCodes()
            {
                Code = "CD-06",
                Type = "POST/Model",
                Description = "SmsVisibilityDetails",
                Data = response
            };
            return Ok(code);
        }


        [Route("ADS")]
        [HttpGet]
        public IHttpActionResult GetAdsDetails(string CivilID, string PolicyNo)
        {

            AddsMapping response = new AddsMapping();
            response.isClaimRegisterSmsVisibility = "False";
            response.isSmsClaimRegisterApplicable = "False";

            var AfyaPolicyNumber = repo.ClaimRegisterSmsVisibility(CivilID, PolicyNo);
            if (AfyaPolicyNumber != "-1")
            {
                response.isClaimRegisterSmsVisibility = "True";

                var IsCivilIDExcluded = repo.ClaimRegisterEnable(CivilID);
                if (IsCivilIDExcluded == "-1")
                {
                    response.isSmsClaimRegisterApplicable = "True";
                }
            }

            var isDiscountApplicableData = repo.isDiscountOptionApplicable();
            var isDiscountOptionApplicablefromGlocalConfig = false;
            var isDiscountApplicablefromAfyaAndCurrentdate = false;
            var isDiscountOptionApplicable = false;

            if (isDiscountApplicableData != null)
            {
                if (isDiscountApplicableData == 1)
                {
                    isDiscountOptionApplicablefromGlocalConfig = true;
                }
            }
            var CardPrintDate = repo.getCardPrintDate(CivilID);
            if (CardPrintDate != "-1")
            {
                var AfyaOfferExpiry = repo.getAfyaOfferExpiry();
                if (!string.IsNullOrEmpty(AfyaOfferExpiry))
                {
                    if (Convert.ToDateTime(CardPrintDate) <= Convert.ToDateTime(AfyaOfferExpiry))
                    {
                        isDiscountApplicablefromAfyaAndCurrentdate = true;
                    }
                }
            }

            if (isDiscountOptionApplicablefromGlocalConfig || isDiscountApplicablefromAfyaAndCurrentdate)
            {
                isDiscountOptionApplicable = true;
            }


            if (!string.IsNullOrEmpty(CivilID))
            {

                IFamily obj = new FamilyDAL();
                IList<Policy> data = obj.GetPolicybyCivilId(CivilID);
                if (data != null && data.Count() > 0)
                {
                    var ActiveAfyaPolicyNumber = repo.getAfyaActivePolicyNumber();
                    if (ActiveAfyaPolicyNumber != null && ActiveAfyaPolicyNumber.ACTIVE_AFYA_POLICY_NO > 0)
                    {
                        var isAddApplicable = false; ;
                        foreach (var item in data)
                        {
                            if (item.Policy_Number == ActiveAfyaPolicyNumber.ACTIVE_AFYA_POLICY_NO)
                            {
                                isAddApplicable = true;

                            }
                        }
                        if (isAddApplicable)
                        {
                            var addsDetails = repo.getActiveAddsDetils(ActiveAfyaPolicyNumber.ACTIVE_AFYA_POLICY_NO);


                            if (addsDetails != null)
                            {



                                response.AD_ID = addsDetails.AD_ID;
                                response.AD_IMG_URL = addsDetails.AD_IMG_URL;
                                response.AD_DETAIL_TEXT_EN = addsDetails.AD_DETAIL_TEXT_EN;
                                response.AD_DETAIL_TEXT_AR = addsDetails.AD_DETAIL_TEXT_AR;
                                response.ADD_EXPIRE_DATE = addsDetails.EXPIRY_DATE.ToString("MM/dd/yyyy h:mm tt");
                                response.isActiveAfyaPolicyFound = true;
                                response.IsAddApplicable = true;
                                response.isDiscountOptionApplicable = isDiscountOptionApplicable.ToString();
                                APIResponceCodes code = new APIResponceCodes()
                                {
                                    Code = "CD-05",
                                    Type = "POST/Model",
                                    Description = "GetAdsDetails Success",
                                    Data = response
                                };
                                return Ok(code);
                            }
                            else
                            {
                                response.AD_ID = 0;
                                response.AD_IMG_URL = "";
                                response.AD_DETAIL_TEXT_EN = "";
                                response.AD_DETAIL_TEXT_AR = "";
                                response.ADD_EXPIRE_DATE = "";
                                response.isActiveAfyaPolicyFound = false;
                                response.IsAddApplicable = false;
                                response.isDiscountOptionApplicable = isDiscountOptionApplicable.ToString();
                                APIResponceCodes code = new APIResponceCodes()
                                {
                                    Code = "CD-06",
                                    Type = "POST/Model",
                                    Description = "GetAdsDetails no response",
                                    Data = response
                                };
                                return Ok(code);
                            }

                        }
                        else
                        {
                            response.AD_ID = 0;
                            response.AD_IMG_URL = "";
                            response.AD_DETAIL_TEXT_EN = "";
                            response.AD_DETAIL_TEXT_AR = "";
                            response.ADD_EXPIRE_DATE = "";
                            response.isActiveAfyaPolicyFound = false;
                            response.IsAddApplicable = false;
                            response.isDiscountOptionApplicable = isDiscountOptionApplicable.ToString();

                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-06",
                                Type = "POST/Model",
                                Description = "No Record Found",
                                DescriptionArabic = "لم يتم العثور على سجل",
                                Data = response

                            };
                            return Ok(code);
                        }
                    }
                    else
                    {
                        response.AD_ID = 0;
                        response.AD_IMG_URL = "";
                        response.AD_DETAIL_TEXT_EN = "";
                        response.AD_DETAIL_TEXT_AR = "";
                        response.ADD_EXPIRE_DATE = "";
                        response.isActiveAfyaPolicyFound = false;
                        response.IsAddApplicable = false;
                        response.isDiscountOptionApplicable = isDiscountOptionApplicable.ToString();

                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-06",
                            Type = "POST/Model",
                            Description = "No Record Found",
                            DescriptionArabic = "لم يتم العثور على سجل",
                            Data = response

                        };
                        return Ok(code);
                    }
                }
                else
                {
                    response.AD_ID = 0;
                    response.AD_IMG_URL = "";
                    response.AD_DETAIL_TEXT_EN = "";
                    response.AD_DETAIL_TEXT_AR = "";
                    response.ADD_EXPIRE_DATE = "";
                    response.isActiveAfyaPolicyFound = false;
                    response.IsAddApplicable = false;
                    response.isDiscountOptionApplicable = isDiscountOptionApplicable.ToString();

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
                        Description = "No Record Found",
                        DescriptionArabic = "لم يتم العثور على سجل",
                        Data = response

                    };
                    return Ok(code);
                }

            }
            else
            {
                response.AD_ID = 0;
                response.AD_IMG_URL = "";
                response.AD_DETAIL_TEXT_EN = "";
                response.AD_DETAIL_TEXT_AR = "";
                response.ADD_EXPIRE_DATE = "";
                response.isActiveAfyaPolicyFound = false;
                response.IsAddApplicable = false;
                response.isDiscountOptionApplicable = isDiscountOptionApplicable.ToString();

                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "POST/Model",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل",
                    Data = response

                };
                return Ok(code);
            }
        }



        [Route("Discount")]
        [HttpGet]

        public IHttpActionResult GetDiscountDetails(string CivilID, string Culture)
        {
            DiscountCouponMapping response = new DiscountCouponMapping();
            var isDiscountApplicable = false;
            var isDiscountDetails = false;
            if (!string.IsNullOrEmpty(CivilID))
            {
                var CardPrintDate = repo.getCardPrintDate(CivilID);
                if (CardPrintDate != "-1")
                {
                    var AfyaOfferExpiry = repo.getAfyaOfferExpiry();
                    if (!string.IsNullOrEmpty(AfyaOfferExpiry))
                    {
                        if (Convert.ToDateTime(CardPrintDate) <= Convert.ToDateTime(AfyaOfferExpiry))
                        {
                            isDiscountApplicable = true;
                        }
                    }
                }

                if (isDiscountApplicable)
                {
                    var discountCouponDetails = repo.getDiscountCouponDetils(CivilID);
                    if (discountCouponDetails != null)
                    {
                        isDiscountDetails = true;

                        response.CIVIL_ID = discountCouponDetails.CIVIL_ID;
                        if (Culture.ToLower() == "ar")
                            response.COUPN_STATUS = getCouponStatusArabic(discountCouponDetails.COUPN_STATUS);
                        else
                            response.COUPN_STATUS = discountCouponDetails.COUPN_STATUS;

                        response.COUPN_EXPIRY_DATE = discountCouponDetails.COUPN_EXPIRY_DATE;
                        response.REDEEMED_DATE = string.IsNullOrEmpty(discountCouponDetails.REDEEMED_DATE) ? "" : discountCouponDetails.REDEEMED_DATE;
                        response.REDEEMED_POLICY_TYPE = string.IsNullOrEmpty(discountCouponDetails.REDEEMED_POLICY_TYPE) ? "" : discountCouponDetails.REDEEMED_POLICY_TYPE;
                        response.REDEEMED_POLICY_NO = string.IsNullOrEmpty(discountCouponDetails.REDEEMED_POLICY_NO) ? "" : discountCouponDetails.REDEEMED_POLICY_NO;
                        response.IS_DISCOUNT_OFFER_APPLICABLE = true;
                        response.OFFER_TERMS_AND_CONDITION_AR = "";
                        response.OFFER_TERMS_AND_CONDITION_EN = "";
                    }
                }

                if (isDiscountApplicable && isDiscountDetails)
                {
                    var getOffertermsandconditions = repo.getAfyaActivePolicyNumber();
                    if (getOffertermsandconditions != null)
                    {
                        response.OFFER_TERMS_AND_CONDITION_AR = getOffertermsandconditions.OFFER_TERMS_CONDITION_AR;
                        response.OFFER_TERMS_AND_CONDITION_EN = getOffertermsandconditions.OFFER_TERMS_CONDITION_EN;
                    }

                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "POST/Model",
                        Description = "GetDiscountDetails Success",
                        Data = response
                    };
                    return Ok(code);
                }
                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
                        Description = "Discount coupon offer is only valid when you collect your new AFYA medical card",
                        DescriptionArabic = "عرض كوبون الخصم سيفعل عند استلامك بطاقة عافية الجديدة",

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
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل",

                };
                return Ok(code);
            }
        }


        public static class CouponStatus
        {
            public const string ACTIVE = "فعال";
            public const string EXPIRED = "منتهي الصلاحية";
            public const string REDEEMED = "محرر";
        }
        public string getCouponStatusArabic(string value)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ACTIVE", "فعال");
            dict.Add("EXPIRED", "منتهي الصلاحية");
            dict.Add("REDEEMED", "محرر");


            string ArabicFromDictionaryByKey = dict[value];
            return ArabicFromDictionaryByKey;
        }


        [Authorize]
        [Route("SmsApplicable")]
        [HttpGet]
        public IHttpActionResult AddSmsApplicable(string CivilID, string isSmsApplicable)
        {

            if (!string.IsNullOrEmpty(CivilID) && !string.IsNullOrEmpty(isSmsApplicable))
            {


                int IsUpdated = repo.UpdateExcludeMemeber(CivilID, Convert.ToBoolean(isSmsApplicable));

                if (IsUpdated >= 0)
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-03",
                        Type = "Post/Add",
                        Description = "Updated Successfully",
                        DescriptionArabic = "Updated Successfully",

                    };

                    return Ok(code);
                }
                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-04",
                        Type = "Post/Add",
                        Description = "Not Updated",
                        DescriptionArabic = "Not Updated",
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



        [Route("AppVersion")]
        [HttpGet]
        public IHttpActionResult GetAppVersion(string Device, string CurrentVersion)
        {
            AppUpdate response = new AppUpdate();
            if (!string.IsNullOrEmpty(Device) && !string.IsNullOrEmpty(CurrentVersion))
            {

                var data = repo.getAppVersionDetils();
                if (data != null)
                {
                    if (Convert.ToBoolean(data.ENFORCE_UPDATE))
                    {
                        if (Device.ToUpper().Trim() == "IOS")
                        {


                            var version1 = new Version(CurrentVersion);
                            var version2 = new Version(data.IOS_CURRENT_VERSION);

                            if (version1 >= version2)
                            {
                                response.MessageENG = "";
                                response.MessageAR = "";
                                response.EnforceUpdate = "False";
                            }
                            else
                            {
                                response.MessageENG = data.MESSAGE_ENG;
                                response.MessageAR = data.MESSAGE_AR;
                                response.EnforceUpdate = "True";
                            }
                        }

                        if (Device.ToUpper().Trim() == "ANDROID")
                        {
                            var version1 = new Version(CurrentVersion);
                            var version2 = new Version(data.ANDROID_CURRENT_VERSION);

                            if (version1 >= version2)
                            {
                                response.MessageENG = "";
                                response.MessageAR = "";
                                response.EnforceUpdate = "False";
                            }
                            else
                            {
                                response.MessageENG = data.MESSAGE_ENG;
                                response.MessageAR = data.MESSAGE_AR;
                                response.EnforceUpdate = "True";
                            }
                        }
                    }
                    else
                    {
                        response.MessageENG = "";
                        response.MessageAR = "";
                        response.EnforceUpdate = "False";
                    }
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "POST/Model",
                        Description = "AppVersion",
                        DescriptionArabic = "AppVersion",
                        Data = response

                    };
                    return Ok(code);
                }
                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-06",
                        Type = "POST/Model",
                        Description = "No Record Found",
                        DescriptionArabic = "لم يتم العثور على سجل",
                        Data = response

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
                    DescriptionArabic = "الرجاء إدخال النص المفقود",
                    Data = response
                };
                return Ok(code);
            }


        }
    }
}
