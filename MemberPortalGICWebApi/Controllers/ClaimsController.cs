using MemberPortalGICWebApi.DataObjects.ClaimsDAL;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MemberPortalGICWebApi.Models;
using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.MemberUserDAL;
using System.Globalization;

namespace MemberPortalGICWebApi.Controllers
{
    [RoutePrefix("api/Claims")]
    public class ClaimsController : ApiController
    {
        IClaims repo = new ClaimsDAL();
        FiltersList objFilter = new FiltersList();

        #region Filters lists 
        /// <summary>
        /// Get Claim Status List
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("ListClaimStatus")]
        [HttpGet]
        public IHttpActionResult GetClaimStatusList()
        {
            var list = objFilter.GetClaimStatus();
            // return Ok(list);
            if (list != null)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "GetClaimStatus list Success",
                    DescriptionArabic = "GetClaimStatus list Success",
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
        /// Get Incident Type List
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("ListIncidentType")]
        [HttpGet]
        public IHttpActionResult GetIncidentTypeList()
        {
            var list = objFilter.GetIncidentTypelist();
            if (list != null)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "GetIncidentTypelist list Success",
                    DescriptionArabic = "GetIncidentTypelist list Success",
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
        /// Get User Provider List
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("ProviderList")]
        [HttpGet]
        public IHttpActionResult GetAllProviders()
        {
            var list = repo.GetAllProviders();
            if (list != null)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "AllProviders list Success",
                    DescriptionArabic = "AllProviders list Success",
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
        #endregion

        /// <summary>
        /// Get user All Claims 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [Route("AllClaims")]
        [HttpPost]
        [Authorize]
    
        public IHttpActionResult GetAllClaims(GetClaimRequestModel model)
        {
        
            if (model.Providerid == "All")
            {
                model.Providerid = null;
            }

            if (model.IncidentType == "All")
            {
                model.IncidentType = null;
            }
            
            string status = "A";
           

            IMemberUser userDAL = new MemberUserDAL();

            //var userInfo = userDAL.GetMemberPolicyMemberNumber(model.CivilId);

            //if (userInfo != null)
            //{ 
                // var list = repo.GetpendningAndRejectedEntries(model).OrderByDescending(m => m.STATUS).ToList();
                var list = repo.DALClaimsUser(status, model.PolicyNo, model.MemNo, model.Culture);
                if (list.Count > 0)

                {

                //foreach(var item in list)
                //{
                //    if(item.CTD_HOSP_EST_AMT > 0 && item.SYSTEM_ESTIMATED_COST_AMOUNT > 0)
                //    {
                //        item.CTD_HOSP_EST_AMT = Convert.ToDecimal(item.CTD_HOSP_EST_AMT.ToString("0.000"));
                //        item.SYSTEM_ESTIMATED_COST_AMOUNT = Convert.ToDecimal(item.SYSTEM_ESTIMATED_COST_AMOUNT.ToString("0.000"));
                //    }
                //}

                    if (model.FromDate2 != null && model.ToDate2 != null)
                    {
                        var newLst = list.Where(row => row.CLM_CR_DT >= DateTime.ParseExact(model.FromDate2, "dd/MM/yyyy", new CultureInfo("en-US")) && row.CLM_CR_DT <= DateTime.ParseExact(model.ToDate2, "dd/MM/yyyy", new CultureInfo("en-US"))).ToList();
                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-05",
                            Type = "POST/Model",
                            Description = "GetAllClaims list Success",
                            DescriptionArabic = "GetAllClaims list Success",
                            Data = newLst
                        };
                        return Ok(code);
                    }
                    else
                    {
                    var newLst = list;


                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-05",
                            Type = "POST/Model",
                            Description = "GetAllClaims list Success",
                            DescriptionArabic = "GetAllClaims list Success",
                            Data = newLst
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
            //}
          

            //else {
            //    APIResponceCodes code = new APIResponceCodes()
            //    {
            //        Code = "CD-06",
            //        Type = "POST/Model",
            //        Description = "No Record Found",
            //        DescriptionArabic = "لم يتم العثور على سجل"

            //    };
            //    return Ok(code);

            //}
        }

        /// <summary>
        /// Get procedure Details of speciif claim
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [Authorize]
        [Route("ProcedureDetails")]
        [HttpPost]
        public IHttpActionResult ProcedureloadFromWithMednext(ProcedureloadModel Model)
        {

            var list = repo.GETAuthorizationServicesWithSystemAmount(Model);
            if (list.Count > 0)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-05",
                    Type = "POST/Model",
                    Description = "GetAllProcedureDetails list Success",
                    DescriptionArabic = "GetAllProcedureDetails list Success",
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




    }
}
