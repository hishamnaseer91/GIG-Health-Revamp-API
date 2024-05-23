

using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.MemberUserDAL;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemberPortalGICWebApi.Controllers
{

    [RoutePrefix("api/Members")]
    public class MemberController : ApiController
    {
        IMemberUser repo = new MemberUserDAL();


        [Authorize]
        [Route("GetRegions")]
        [HttpGet]
        public IHttpActionResult GetRegions()
        {

            var List = repo.DALRegionDDL();

            if (List != null)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "Get Regions List Success",
                    DescriptionArabic = "Get Regions List Success",
                    Data = List
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

        [Authorize]
        [Route("GetArea")]
        [HttpPost]
        public IHttpActionResult GetAreaDistrict(GetAreaModel model)
        {

            if (model.RegionId == "")
            {
                var result = repo.DALAreaDDL(0);
                if (result != null)
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "Get Area/District List Success",
                        DescriptionArabic = "Get Area/District  List Success",
                        Data = result
                    };
                    return Ok(code);
                }


            }

            else
            {
                var result = repo.DALAreaDDL(Convert.ToInt32(model.RegionId));
                if (result != null)
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "Get Area/District List Success",
                        DescriptionArabic = "Get Area/District  List Success",
                        Data = result
                    };
                    return Ok(code);
                }
            }

            APIResponceCodes codeError = new APIResponceCodes()
            {
                Code = "CD-02",
                Type = "Get",
                Description = "No Record Found",
                DescriptionArabic = "لم يتم العثور على سجل"

            };
            return Ok();


        }

        /// <summary>
        /// GetProfile Member
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [Route("GetProfile")]
        [HttpPost]
        public IHttpActionResult GetProfile(GetProfileModel model)
        {

            var List = repo.GetMemberProfile(model.CivilId, model.policyNumber);

            if (List != null)

            {
                //  var result = repo.GetMemberAssuredName(model.CivilId);
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "User Profile Get Success",
                    DescriptionArabic = "User Profile Get Success",
                    Data = List
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
        /// IsAddressUpdateAllowed to check before update Profile
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("IsAddressUpdateAllowed")]
        [HttpGet]

        public IHttpActionResult IsUSerCanUpdateAddress()
        {

            var IsAllowd = repo.IsUSerCanUpdateAddress();
            bool status = true;
            if (IsAllowd == 1)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "Address Update Allowed By Admin",
                    DescriptionArabic = "Address Update Allowed By Admin",
                    Data = status
                };



                return Ok(code);
            }

            else
            {
                status = false;
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "Address Update Not Allowed By Admin",
                    DescriptionArabic = "Address Update Not Allowed By Admin",
                    Data = status
                };
                return Ok(code);
            }
        }


        [Authorize]
        [Route("UpdateProfileAddress")]
        [HttpPost]

        public IHttpActionResult UpdateProfileAddress(ProfileAddressModel AdressModel)
        {

            if (!string.IsNullOrEmpty(AdressModel.CivilId) && !string.IsNullOrEmpty(AdressModel.policyNumber))
            {
                var memberId = repo.GetMemberId(AdressModel.CivilId, AdressModel.policyNumber);
                if (memberId != -1)
                {
                    var userModel = repo.GetUserDetailModel(AdressModel.CivilId, AdressModel.policyNumber);
                    //iF SOME VALUES MISSING NULL EXCEPTON
                    if (userModel != null)
                    {
                        userModel.BlockNo = AdressModel.BlockNo;
                        userModel.BuildingNo = AdressModel.BuildingNo;
                        userModel.FlatNo = AdressModel.FlatNo;
                        userModel.FloorNo = AdressModel.FloorNo;
                        userModel.StreetNo = AdressModel.StreetNo;
                        userModel.REGION_ID = AdressModel.REGION_ID;
                        userModel.DISTRICT_ID = AdressModel.DISTRICT_ID;
                        userModel.PHONE_NUMBER1 = AdressModel.MobileNumber;
                        var Mniuser = ConfigurationManager.AppSettings["MNIWSUSER"];

                        int IsUpdatedLocal = repo.UpdateAddressLocalHistory(userModel, memberId.ToString());


                        if (IsUpdatedLocal > 0)
                        {
                            MemberAddressUpdateWs AdressObje = new MemberAddressUpdateWs();
                            userModel.ADDRESS = string.Format("Block|{0}| Street|{1}| Building|{2}| Floor|{3}| Flat|{4}| Notes|{5}", userModel.BlockNo, userModel.StreetNo, userModel.BuildingNo, userModel.FloorNo, userModel.FlatNo, userModel.Notes);
                            var Objeresult = repo.CheckMemberExistence(userModel.MEMBER_NUMBER);
                            if (Objeresult != null)
                            {
                                var resultd = AdressObje.UpdateMethod(userModel, Mniuser, userModel.Name, Objeresult.Fax);

                                if (resultd == true)
                                {
                                    APIResponceCodes code1 = new APIResponceCodes()
                                    {
                                        Code = "CD-03",
                                        Type = "POST/ADD",
                                        Description = "Profile Updated Successfully",
                                        DescriptionArabic = "تم تحديث الملف الشخصي بنجاح"

                                    };
                                    return Ok(code1);
                                }

                                else
                                {
                                    APIResponceCodes code1 = new APIResponceCodes()
                                    {
                                        Code = "CD-03",
                                        Type = "POST/ADD",
                                        Description = "Profile not Updated",
                                        DescriptionArabic = "Profile not Updated"

                                    };
                                    return Ok(code1);
                                }

                            }

                            else
                            {


                                var resultd = AdressObje.CreateAddresForMember(userModel, Mniuser, userModel.Name);
                                if (resultd == true)
                                {
                                    APIResponceCodes code1 = new APIResponceCodes()
                                    {
                                        Code = "CD-03",
                                        Type = "POST/ADD",
                                        Description = "Adding MemberUser Details WS Success",
                                        DescriptionArabic = "Adding/Add MemberUser Details WS Success"

                                    };
                                    return Ok(code1);
                                }

                                else
                                {
                                    APIResponceCodes code1 = new APIResponceCodes()
                                    {
                                        Code = "CD-03",
                                        Type = "POST/ADD",
                                        Description = "Adding Member User Details Failed",
                                        DescriptionArabic = "Adding Member User Details Failed"

                                    };
                                    return Ok(code1);
                                }
                            }


                        }
                        else
                        {

                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-04",
                                Type = "POST/ADD",
                                Description = "Error Updating Member User Details",
                                DescriptionArabic = "Error Updating Member User Details"

                            };
                            return Ok(code);
                        }

                    }

                    else
                    {
                        APIResponceCodes codeError = new APIResponceCodes()
                        {
                            Code = "CD-02",
                            Type = "Get",
                            Description = "No Record Found",
                            DescriptionArabic = "لم يتم العثور على سجل"

                        };
                        return Ok();
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

        //var memberId = repo.GetMemberId(AdressModel.CivilId);
        //if (memberId == -1)
        //{
        //    APIResponceCodes code = new APIResponceCodes()
        //    {
        //        Code = "CD-01",
        //        Type = "Get",
        //        Description = "Get Regions List Success",
        //        DescriptionArabic = "Get Regions List Success",
        //        Data = List
        //    };

        //}

        //var result = repo.GetUserDetailModel(memberId);

        //if (result != null)
        //{
        //    result.BlockNo = AdressModel.BlockNo;
        //    result.BuildingNo = AdressModel.BuildingNo;
        //    result.FlatNo = AdressModel.FlatNo;
        //    result.FloorNo = AdressModel.FloorNo;
        //    result.StreetNo = AdressModel.StreetNo;
        //    ///////////////////////////////////
        //    result.REGION_ID = AdressModel.REGION_ID;
        //    result.DISTRICT_ID = AdressModel.DISTRICT_ID;
        //}

        //else
        //{ 

        //}

        //APIResponceCodes code = new APIResponceCodes()
        //{
        //    Code = "CD-00",
        //    Type = "Post/Model",
        //    Description = "input Model Value Null",
        //    DescriptionArabic = "input Model Value Null"
        //};
        //return Ok(code);


        /// <summary>
        /// Change Passowrd after Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [Route("ChangePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(ChangePasswordModel model)
        {

            if (model.CivilId != "" & model.Password != "")
            {
                if(model.IsOldPasswordRequired)
                {
                    if(model.OldPassword != "")
                    {
                        model.OldPassword = Common.Encrypt(model.OldPassword);
                        model.Password = Common.Encrypt(model.Password);
                        var isCorrectOldPassword = repo.isCorrectOldPassword(model);
                        if(isCorrectOldPassword != -1)
                        {
                            int IsSuccess = repo.ChangePassword(model);
                            if (IsSuccess != 0)
                            {  // This is used to return memeber profile after regisrter success
                               // var result = repo.GetMemberProfileDetails(Id.ToString());

                                APIResponceCodes code = new APIResponceCodes()
                                {
                                    Code = "CD-03",
                                    Type = "Post",
                                    Description = "Password Updated Successfully",
                                    DescriptionArabic = "تم تحديث كلمة المرور بنجاح",
                                    //Data = result
                                };
                                return Ok(code);
                            }
                            else
                            {
                                APIResponceCodes code = new APIResponceCodes()
                                {
                                    Code = "CD-04",
                                    Type = "Post/Update",
                                    Description = "Update Password Failure",
                                    DescriptionArabic = "Update Password Failure",
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
                                Description = "Old Password is Incorrect",
                                DescriptionArabic = "كلمة المرور القديمة غير صحيحة"
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
                else
                {
                    model.Password = Common.Encrypt(model.Password);
                    int IsSuccess = repo.ChangePassword(model);
                    if (IsSuccess != 0)
                    {  // This is used to return memeber profile after regisrter success
                       // var result = repo.GetMemberProfileDetails(Id.ToString());

                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-03",
                            Type = "Post",
                            Description = "Password Updated Successfully",
                            DescriptionArabic = "تم تحديث كلمة المرور بنجاح",
                            //Data = result
                        };
                        return Ok(code);
                    }
                    else
                    {
                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-04",
                            Type = "Post/Update",
                            Description = "Update Password Failure",
                            DescriptionArabic = "Update Password Failure",
                        };
                        return Ok(code);
                    }
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
        [Route("Notifications")]
        [HttpPost]
        public IHttpActionResult GetNotifications(NotificationModel model)
        {

            var List = repo.GetAllNotifications(model.CivilId);

            if (List != null && List.Count > 0)

            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-01",
                    Type = "Get",
                    Description = "New Notification Recieved Success",
                    DescriptionArabic = "New Notification Recieved Success",
                    Data = List
                };



                return Ok(code);
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-02",
                    Type = "Get",
                    Description = "No Recent Notification",
                    DescriptionArabic = "لا إعلام جديد"

                };
                return Ok(code);
            }

        }



        [Route("FirstPasswordChange")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult IsFirstPasswordChange(NotificationModel model)
        {

            bool IsPasswordChange = repo.IsFirstPasswordChange(model.CivilId);
            APIResponceCodes code = new APIResponceCodes()
            {
                Code = "CD-01",
                Type = "Post",
                Description = "Is First Password Change: ",
                DescriptionArabic = "Is First Password Change: ",
                Result = IsPasswordChange.ToString()
            };
            return Ok(code);
        }

        [Authorize]
        [Route("DeleteMemberProfile")]
        [HttpPost]
        public IHttpActionResult DeleteMemberProfile(DeleteMemberModel model)
        {

            if (!string.IsNullOrEmpty(model.CivilId) || !string.IsNullOrEmpty(model.MemberNumber) || !string.IsNullOrEmpty(model.PolicyNumber) || !string.IsNullOrEmpty(model.DeletionComments))
            {
                //var memberDetail = repo.GetMemberProfile(model.CivilId, model.PolicyNumber);
                var memberDetail = repo.GetmemberUser(model.CivilId);
                if (memberDetail != null)
                {
                    int IsDeleted = repo.DeleteMemberProfile(model);
                    if (IsDeleted > 0)
                    {
                        int LogInserted = repo.InsertDeleteMemberLog(memberDetail ,model.DeletionComments);
                        if (LogInserted > 0)
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-03",
                                Type = "Post/Delete",
                                Description =  "Account is successfully deleted",
                                DescriptionArabic = "تم حذف الحساب بنجاح",
                                Data = "Success"
                            };
                            return Ok(code);
                        }
                        else
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-03",
                                Type = "Post/Delete",
                                Description = "Account is successfully deleted. Failed to Insert deletion log",
                                DescriptionArabic = "تم حذف الحساب بنجاح. فشل في إدراج سجل الحذف",
                                Data = "Error"
                            };
                            return Ok(code);
                        }
                    }
                    else
                    {
                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-04",
                            Type = "Post/Delete",
                            Description = "Account Deletion Failed",
                            DescriptionArabic = "فشل حذف الحساب",
                            Data = "Error"
                        };
                        return Ok(code);
                    }
                }

                else
                {
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-02",
                        Type = "GET",
                        Description = "Failed to delete account , contact IT support",
                        DescriptionArabic = "لم يتم حذف الحساب لوجود خطأ، يرجى التواصل مع قسم الدعم الفني",
                        Data = "Error"

                    };
                    return Ok(code);
                }
            
             
            }
            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-00",
                    Type = "Post/Delete",
                    Description = "Input Value Missing",
                    DescriptionArabic = "الرجاء إدخال النص المفقود"
                };
                return Ok(code);
            }

        }


        #region ecard
        [Route("GetMemberCard")]
        [HttpPost]
         [Authorize]
        public IHttpActionResult GetMemberCard(GetCardModel model)
        {
            try
            {
                ECardModel eCardModel = new ECardModel()
                {
                    CardLabels = new ECardLabels(),
                    CardValues = new ECardValue()
                };
                eCardModel.CardValues = repo.GetECardDetail(model.CivilId, model.PolicyNo, model.MemberNo);
                if (!string.IsNullOrEmpty(eCardModel.CardValues.CidNo))
                {
                    eCardModel.CardValues.CardToShow = GetCardToShow(eCardModel.CardValues.ProductDescription.ToUpper(), eCardModel.CardValues.Plan.ToUpper());
                    eCardModel.CardLabels = GetCardLabels(eCardModel.CardValues.CardToShow);
                    if (eCardModel.CardValues.Dob != "" && eCardModel.CardValues.CardToShow == ECardTypes.Reaya)
                    {
                        eCardModel.CardValues.Dob = eCardModel.CardValues.Dob.Split('/')[2];
                    }
                    //Get Co Pay
                    if (eCardModel.CardValues.CardToShow == ECardTypes.Asfar || eCardModel.CardValues.CardToShow == ECardTypes.FayDual || eCardModel.CardValues.CardToShow == ECardTypes.FayAXA)
                    {
                        eCardModel.CardValues.Co_Pay = GetCoPay(eCardModel.CardValues);
                    }
                    else
                    {
                        eCardModel.CardValues.Co_Pay = "";
                    }
                    ////Get Deductable
                    //if (eCardModel.CardValues.CardToShow == ECardTypes.Fay || eCardModel.CardValues.CardToShow == ECardTypes.FayDual || eCardModel.CardValues.CardToShow == ECardTypes.FayAXA || eCardModel.CardValues.CardToShow == ECardTypes.Reaya)
                    //{
                    //    eCardModel.CardValues.Deductable = GetDedutable(eCardModel.CardValues);
                    //}
                    //else
                    //{
                    //    eCardModel.CardValues.Deductable = "";
                    //}
                    ////Get Waiting Period
                    //if (eCardModel.CardValues.CardToShow == ECardTypes.Asfar || eCardModel.CardValues.CardToShow == ECardTypes.FayDual || eCardModel.CardValues.CardToShow == ECardTypes.FayAXA)
                    //{
                    //    eCardModel.CardValues.Waiting_P = GetWaitingPeriod(eCardModel.CardValues);
                    //}
                    //else
                    //{
                    //    eCardModel.CardValues.Waiting_P = "";
                    //}
                    //Asfar Plan Type
                    if (eCardModel.CardValues.CardToShow == ECardTypes.Asfar)
                    {
                        eCardModel.CardValues.Plan = eCardModel.CardValues.PackageNumber + " - " + eCardModel.CardValues.Plan;
                    }
                    //Get Co Pay Dental for Fay AXA and Regional
                    if (eCardModel.CardValues.CardToShow == ECardTypes.FayAXA || (eCardModel.CardValues.ProductDescription.ToUpper().Contains(ECardTypes.FayRegional) || eCardModel.CardValues.Plan.ToUpper().Contains(ECardTypes.FayRegional)))
                    {
                        eCardModel.CardValues.CoPayDT = GetCoPayDT(eCardModel.CardValues.CoPayDT);
                        eCardModel.CardValues.ShowCoPayDental = true;
                    }
                    else
                    {
                        eCardModel.CardValues.ShowCoPayDental = false;
                    }
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-01",
                        Type = "Get",
                        Description = "Member Card Get Success",
                        DescriptionArabic = "Member Card Get Success",
                        Data = eCardModel
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
            catch (Exception ex)
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
        public string GetCardToShow(string policy, string plan)
        {
            if (policy.Contains(ECardTypes.FayDual) || plan.Contains(ECardTypes.FayDual) || policy.Contains(ECardTypes.FayRegional) || plan.Contains(ECardTypes.FayRegional))
                return ECardTypes.FayDual;
            else if (policy.Contains(ECardTypes.FayAXA) || policy.Contains(ECardTypes.FayInternational) || policy.Contains(ECardTypes.FayGlobal) || plan.Contains(ECardTypes.FayAXA) || plan.Contains(ECardTypes.FayInternational) || plan.Contains(ECardTypes.FayGlobal))
                return ECardTypes.FayAXA;
            else if (policy.Contains(ECardTypes.FayKidsCare) || plan.Contains(ECardTypes.FayKidsCare))
                return ECardTypes.FayKidsCare;
            else if (policy.Contains(ECardTypes.FayPlus) || plan.Contains(ECardTypes.FayPlus) || policy.Contains(ECardTypes.Fay) || plan.Contains(ECardTypes.Fay))
                return ECardTypes.Fay;
            else if (policy.Contains(ECardTypes.Asfar) || plan.Contains(ECardTypes.Asfar))
                return ECardTypes.Asfar;
            else if (policy.Contains(ECardTypes.PGH) || plan.Contains(ECardTypes.PGH))
                return ECardTypes.PGH;
            else if (policy.Contains(ECardTypes.Heston) || plan.Contains(ECardTypes.Heston))
                return ECardTypes.Heston;
            else if (policy.Contains(ECardTypes.Equate) || plan.Contains(ECardTypes.Equate))
                return ECardTypes.Equate;
            else if (policy.Contains(ECardTypes.Afya) || plan.Contains(ECardTypes.Afya))
                return ECardTypes.Afya;
            else if (policy.Contains(ECardTypes.KNPC) || policy.Contains("KPC") || policy.Contains("KUWAIT PETROLEUM CORPORATION") || plan.Contains(ECardTypes.KNPC) || plan.Contains("KPC") || plan.Contains("KUWAIT PETROLEUM CORPORATION"))
                return ECardTypes.KNPC;
            else if (policy.Contains(ECardTypes.Reaya) || plan.Contains(ECardTypes.Reaya))
                return ECardTypes.Reaya;
            else
                return ECardTypes.Default;
        }
        public ECardLabels GetCardLabels(string cardType)
        {
            ECardLabels eCardLabels = new ECardLabels();
            if (cardType == ECardTypes.Afya)
            {
                eCardLabels.Mem_no = "MEMBER No";
                eCardLabels.Enrolment = "EFFECTIVE";
            }
            else if (cardType == ECardTypes.KNPC)
            {
                eCardLabels.PolicyHolder = "Policy Holder";
                eCardLabels.ExpiryDate = "Exp. Date";
                eCardLabels.Cidno = "C.I.D. No";
            }
            else if (cardType == ECardTypes.Heston)
            {
                eCardLabels.PolicyHolder = "Policy Holder";
                eCardLabels.ExpiryDate = "Exp. Date";
            }
            else if (cardType == ECardTypes.Equate)
            {
                eCardLabels.PolicyHolder = "Policy Holder";
                eCardLabels.ExpiryDate = "Exp. Date";
            }
            else if (cardType == ECardTypes.Asfar)
            {
                eCardLabels.Mem_name = "Name";
                eCardLabels.Enrolment = "Enrolment";
            }
            else if (cardType == ECardTypes.FayAXA)
            {
                eCardLabels.Plan = "Plan";
                eCardLabels.Enrolment = "Enrolment";
            }
            else if (cardType == ECardTypes.FayDual)
            {
                eCardLabels.Mem_name = "Name";
                eCardLabels.Plan = "Network";
                eCardLabels.Enrolment = "Enrolment";
                eCardLabels.ExpiryDate = "Expiry";
                eCardLabels.CardNo = "Card. No";
            }
            else if (cardType == ECardTypes.FayKidsCare)
            {
                eCardLabels.Mem_name = "Mem. Name";
                eCardLabels.Plan = "Plan";
                eCardLabels.Enrolment = "Enrolment";
                eCardLabels.ExpiryDate = "Exp. Date";
                eCardLabels.CardNo = "Card. No";
            }
            else if (cardType == ECardTypes.Fay)
            {
                eCardLabels.Plan = "Plan";
                eCardLabels.Enrolment = "Enrolment";
                eCardLabels.ExpiryDate = "Exp. Date";
            }
            else if (cardType == ECardTypes.Reaya)
            {
                eCardLabels.Enrolment = "St. Date";
                eCardLabels.ExpiryDate = "Exp. Date";
            }
            else if (cardType == ECardTypes.Default)
            {
                eCardLabels.PolicyHolder = "Policy Holder";
                eCardLabels.ExpiryDate = "Exp. Date";
            }
            else
            {

            }
            return eCardLabels;
        }
        //public string GetWaitingPeriod(ECardValue eCardValue)
        //{
        //    string waitingP = string.Empty;
        //    //Waiting P will be empty for policy type of InPatient 25-10-2023
        //    if (!(eCardValue.ProductDescription.ToUpper().Contains("INPATIENT ONLY") || eCardValue.Plan.ToUpper().Contains("INPATIENT ONLY") || eCardValue.ProductDescription.ToUpper().Contains("IN-PATIENT ONLY") || eCardValue.Plan.ToUpper().Contains("IN-PATIENT ONLY")))
        //    {
        //        if (eCardValue.CardToShow == ECardTypes.Asfar)
        //        {
        //            if (!string.IsNullOrEmpty(eCardValue.Chronic_Waiting_P))
        //            {
        //                waitingP += "Chronic Med " + eCardValue.Chronic_Waiting_P + "M";
        //            }
        //            else
        //            {
        //                waitingP += "Chronic Med M";
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.MaternityIn_Waiting_P))
        //            {
        //                waitingP += ", Maternity (IP & OP) " + eCardValue.MaternityIn_Waiting_P + "M";
        //            }
        //            else if (!string.IsNullOrEmpty(eCardValue.MaternityOut_Waiting_P))
        //            {
        //                waitingP += ", Maternity (IP & OP) " + eCardValue.MaternityOut_Waiting_P + "M";
        //            }
        //            else
        //            {
        //                waitingP += ", Maternity (IP & OP) M";
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.Wellness_Waiting_P))
        //            {
        //                waitingP += ", Wellness " + eCardValue.Wellness_Waiting_P + "M";
        //            }
        //            else
        //            {
        //                waitingP += ", Wellness M";
        //            }
        //        }
        //        else if (eCardValue.CardToShow == ECardTypes.FayDual)
        //        {
        //            if (!string.IsNullOrEmpty(eCardValue.Dental_Waiting_P))
        //            {
        //                waitingP += "Dental " + eCardValue.Dental_Waiting_P + "M";
        //            }
        //            else
        //            {
        //                waitingP += "Dental M";
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.Chronic_Waiting_P))
        //            {
        //                waitingP += "Chronic Med " + eCardValue.Chronic_Waiting_P + "M";
        //            }
        //            else
        //            {
        //                waitingP += ", Chronic Med M";
        //            }
        //        }
        //        else if (eCardValue.CardToShow == ECardTypes.FayAXA)
        //        {
        //            if (!string.IsNullOrEmpty(eCardValue.Dental_Waiting_P))
        //            {
        //                waitingP += "Dental " + eCardValue.Dental_Waiting_P + "M";
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.MaternityIn_Waiting_P))
        //            {
        //                if (string.IsNullOrEmpty(waitingP))
        //                {
        //                    waitingP += "Maternity " + eCardValue.MaternityIn_Waiting_P + "M";
        //                }
        //                else
        //                {
        //                    waitingP += ", Maternity " + eCardValue.MaternityIn_Waiting_P + "M";
        //                }
        //            }
        //            else if (!string.IsNullOrEmpty(eCardValue.MaternityOut_Waiting_P))
        //            {
        //                if (string.IsNullOrEmpty(waitingP))
        //                {
        //                    waitingP += "Maternity " + eCardValue.MaternityOut_Waiting_P + "M";
        //                }
        //                else
        //                {
        //                    waitingP += ", Maternity " + eCardValue.MaternityOut_Waiting_P + "M";
        //                }
        //            }
        //            else
        //            {

        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //    return waitingP;
        //}
        //public string GetDedutable(ECardValue eCardValue)
        //{
        //    string deductable = string.Empty;
        //    //Deductible will be Nil for Consultation for policy type of InPatient 25-10-2023
        //    if (eCardValue.ProductDescription.ToUpper().Contains("INPATIENT ONLY") || eCardValue.Plan.ToUpper().Contains("INPATIENT ONLY") || eCardValue.ProductDescription.ToUpper().Contains("IN-PATIENT ONLY") || eCardValue.Plan.ToUpper().Contains("IN-PATIENT ONLY"))
        //    {
        //        deductable += "Nil for Consultation";
        //    }
        //    else
        //    {
        //        if (eCardValue.CardToShow == ECardTypes.FayDual)
        //        {
        //            if(eCardValue.Plan.ToUpper().Contains(ECardTypes.FayRegional))
        //            {
        //                deductable += "Nil for Consultation";
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(eCardValue.Deductable))
        //                {
        //                    string deductable2 = string.Empty;
        //                    try
        //                    {
        //                        if (eCardValue.Plan.Contains("-"))
        //                        {
        //                            deductable2 = "/Nil in " + eCardValue.Plan.Replace("-", "").Replace(" ", ",").Split(',')[2];
        //                        }
        //                        else
        //                        {
        //                            deductable2 = "/Nil in " + eCardValue.Plan.Replace(" ", ",").Split(',')[2];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        deductable2 = "/Nil in " + eCardValue.Plan.Replace("Fay Dual ", "").Trim();
        //                    }
        //                    int value = 0;
        //                    bool isValid = int.TryParse(eCardValue.Deductable, out value);
        //                    if (isValid && value > 0)
        //                    {
        //                        deductable += "KD " + eCardValue.Deductable + " for Consultation (KWT)" + deductable2;
        //                    }
        //                    else
        //                    {
        //                        deductable += "KD  for Consultation (KWT)" + deductable2;
        //                    }
        //                }
        //                else
        //                {
        //                    string deductable2 = string.Empty;
        //                    try
        //                    {
        //                        if (eCardValue.Plan.Contains("-"))
        //                        {
        //                            deductable2 = "/Nil in " + eCardValue.Plan.Replace("-", "").Replace(" ", ",").Split(',')[2];
        //                        }
        //                        else
        //                        {
        //                            deductable2 = "/Nil in " + eCardValue.Plan.Replace(" ", ",").Split(',')[2];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        deductable2 = "/Nil in " + eCardValue.Plan.Replace("Fay Dual ", "").Trim();
        //                    }
        //                    deductable += "KD  for Consultation (KWT)" + deductable2;
        //                }
        //            }
        //        }
        //        else if (eCardValue.CardToShow == ECardTypes.Fay)
        //        {
        //            if (!string.IsNullOrEmpty(eCardValue.CoPayIn))
        //            {
        //                deductable += "IP " + eCardValue.CoPayIn + "%";
        //            }
        //            else
        //            {
        //                deductable += "IP 0%";
        //            }
        //            if (!(eCardValue.ProductDescription.ToUpper().Contains("INPATIENT") || eCardValue.Plan.ToUpper().Contains("INPATIENT") || eCardValue.ProductDescription.ToUpper().Contains("IN-PATIENT") || eCardValue.Plan.ToUpper().Contains("IN-PATIENT")))
        //            {
        //                if (!string.IsNullOrEmpty(eCardValue.CoPayOut))
        //                {
        //                    deductable += ", OP " + eCardValue.CoPayOut + "%";
        //                }
        //                else
        //                {
        //                    deductable += ", OP 0%";
        //                }
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.Deductable))
        //            {
        //                int value = 0;
        //                bool isValid = int.TryParse(eCardValue.Deductable, out value);
        //                deductable += ", CS KD " + value;
        //            }
        //            if (eCardValue.ProductDescription.ToUpper().Contains(ECardTypes.FayPlus) || eCardValue.Plan.ToUpper().Contains(ECardTypes.FayPlus))
        //            {
        //                if (!string.IsNullOrEmpty(eCardValue.CoPayDT))
        //                {
        //                    int value = 0;
        //                    bool isValid = int.TryParse(eCardValue.CoPayDT, out value);
        //                    deductable += ", DT " + value + "%";
        //                }
        //            }
        //        }
        //        else if (eCardValue.CardToShow == ECardTypes.Reaya)
        //        {
        //            if (!string.IsNullOrEmpty(eCardValue.CoPayIn))
        //            {
        //                deductable += "IP " + eCardValue.CoPayIn + "%";
        //            }
        //            else
        //            {
        //                deductable += "IP 0%";
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.CoPayOut))
        //            {
        //                deductable += ", OP " + eCardValue.CoPayOut + "%";
        //            }
        //            else
        //            {
        //                deductable += ", OP 0%";
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.Deductable))
        //            {
        //                int value = 0;
        //                bool isValid = int.TryParse(eCardValue.Deductable, out value);
        //                deductable += ", CS " + value + " KD";
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.CoPayDT))
        //            {
        //                int value = 0;
        //                bool isValid = int.TryParse(eCardValue.CoPayDT, out value);
        //                deductable += ", DT " + value + "%";
        //            }
        //        }
        //        else if (eCardValue.CardToShow == ECardTypes.FayAXA)
        //        {
        //            deductable = "Nil";
        //        }
        //        else
        //        {

        //        }
        //    }
        //    return deductable;
        //}
        //Co Pay New Implementation 05-11-2023
        public string GetCoPay(ECardValue eCardValue)
        {
            string coPay = eCardValue.Co_Pay;
            if (!string.IsNullOrEmpty(coPay) && eCardValue.CardToShow == ECardTypes.FayDual && !string.IsNullOrEmpty(eCardValue.CoPayDT) &&
                !(eCardValue.ProductDescription.ToUpper().Contains(ECardTypes.FayRegional) || eCardValue.Plan.ToUpper().Contains(ECardTypes.FayRegional)))
            {
                coPay += "DENTAL " + eCardValue.CoPayDT;
            }
            return coPay;
        }
        //public string GetCoPay(ECardValue eCardValue)
        //{
        //    string coPay = string.Empty;
        //    //CoPay will be Nil for policy type of InPatient 25-10-2023
        //    if (eCardValue.ProductDescription.ToUpper().Contains("INPATIENT ONLY") || eCardValue.Plan.ToUpper().Contains("INPATIENT ONLY") || eCardValue.ProductDescription.ToUpper().Contains("IN-PATIENT ONLY") || eCardValue.Plan.ToUpper().Contains("IN-PATIENT ONLY"))
        //    {
        //        coPay += "Nil";
        //    }
        //    else
        //    {
        //        if (eCardValue.CardToShow == ECardTypes.Asfar)
        //        {
        //            if (!string.IsNullOrEmpty(eCardValue.CoPayIn))
        //            {
        //                coPay += "IP " + eCardValue.CoPayIn + "%";
        //            }
        //            else
        //            {
        //                coPay += "IP 0%";
        //            }
        //            if (!string.IsNullOrEmpty(eCardValue.CoPayOut))
        //            {
        //                coPay += ", OP " + eCardValue.CoPayOut + "%";
        //            }
        //            else
        //            {
        //                coPay += ", OP 0%";
        //            }
        //        }
        //        else
        //        {
        //            if (eCardValue.CardToShow == ECardTypes.FayAXA || (eCardValue.ProductDescription.ToUpper().Contains(ECardTypes.FayRegional) || eCardValue.Plan.ToUpper().Contains(ECardTypes.FayRegional)))
        //            {
        //                coPay = "Nil";
        //            }
        //            else if (eCardValue.CardToShow == ECardTypes.FayDual)
        //            {
        //                if (!string.IsNullOrEmpty(eCardValue.CoPayOut))
        //                {
        //                    coPay += eCardValue.CoPayOut + "% ON MED";
        //                }
        //                else
        //                {
        //                    coPay += " % ON MED";
        //                }
        //                if (!string.IsNullOrEmpty(eCardValue.CoPayDT))
        //                {
        //                    coPay += ", DIAG TEST & PT - DENTAL " + eCardValue.CoPayDT + "%";
        //                }
        //                else
        //                {
        //                    coPay += ", DIAG TEST & PT";
        //                }
        //            }
        //            else
        //            {

        //            }
        //        }
        //    }
        //    return coPay;
        //}
        public string GetCoPayDT(string coPayDental)
        {
            string coPayDT = string.Empty;
            if (!string.IsNullOrEmpty(coPayDental))
            {
                coPayDT = "DT " + coPayDental;
            }
            return coPayDT;
        }
        #endregion
    }
}
