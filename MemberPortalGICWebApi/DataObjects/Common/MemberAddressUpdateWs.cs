using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects
{
    public class MemberAddressUpdateWs
    {
        public bool UpdateMethod(UserMemberUpdateModel Info, string gicuser, string username, string fax)
        {
            // decimal autono = default(decimal);
            try
            {


                MemberAddressUpdate.cs.LifeAddressRequestSDO Request = new MemberAddressUpdate.cs.LifeAddressRequestSDO();
                MemberAddressUpdate.cs.LifeAdressDSClient Client = new MemberAddressUpdate.cs.LifeAdressDSClient();
                MemberAddressUpdate.cs.LifeAddressResponseSDO Resp = new MemberAddressUpdate.cs.LifeAddressResponseSDO();
                var dd = Convert.ToDecimal(Info.MEMBER_NUMBER);
                Request.MemberNumberSpecified = true;
                Request.MemberNumber = dd;
                Request.AddressTypeId = "H";
                Request.MailingIndicator = "Y";
                Request.CountryCode = "KW";
                Request.Username = gicuser;
                Request.AddressText = Info.ADDRESS;
                Request.RegionCode = Info.REGION_ID;
                Request.DistrictCode = Info.DISTRICT_ID;
                Request.PhoneNumber1 = Info.PHONE_NUMBER1;

                Request.FaxNumber = fax;
                //  Request.PhoneNumber1 = Info.PHONE_NUMBER1 == null ? "" : Info.PHONE_NUMBER1;
                //Request.PhoneNumber2 = Info.AlternateMobile_No;

                Resp = Client.updateLifeAddress(Request);
                if (Resp.MemberNumber == Convert.ToDecimal(Info.MEMBER_NUMBER))
                {

                    return true;
                }
                else
                {
                    Logs ErrorClass = new Logs();
                    ErrorClass.ErrorCode = "MemberPortalEWsLifeAddressOver";
                    ErrorClass.ErorDesc = "Error MemberUpdateAddress MemberPortal  Update memberno =" + Info.MEMBER_NUMBER + " Over All Execution of WS MNI Issue for user " + username + " URL is /MedNeXtWebServices-Model/LifeAdressDS ";
                    ErrorClass.ErrorExp = "";
                    AddErrors Objec = new AddErrors();
                    Objec.InsertErrorLog(ErrorClass);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Logs ErrorClass = new Logs();
                ErrorClass.ErrorCode = "MemberPortalEWsLifeAddressOverAll";
                ErrorClass.ErorDesc = "Error MemberUpdateAddress MemberPortal Update memberno =" + Info.MEMBER_NUMBER + " Over All Execution of WS MNI Issue for user " + username + " URL is /MedNeXtWebServices-Model/LifeAdressDS ";
                ErrorClass.ErrorExp = ex.Message + "||" + ex.InnerException;
                AddErrors Objec = new AddErrors();
                Objec.InsertErrorLog(ErrorClass);
                return false;
            }


        }


        public bool CreateAddresForMember(UserMemberUpdateModel Info, string gicuser, string username)
        {
            // decimal autono = default(decimal);
            try
            {
                MemberAddressUpdate.cs.LifeAddressRequestSDO Request = new MemberAddressUpdate.cs.LifeAddressRequestSDO();
                MemberAddressUpdate.cs.LifeAdressDSClient Client = new MemberAddressUpdate.cs.LifeAdressDSClient();
                MemberAddressUpdate.cs.LifeAddressResponseSDO Resp = new MemberAddressUpdate.cs.LifeAddressResponseSDO();
                var dd = Convert.ToDecimal(Info.MEMBER_NUMBER);
                Request.MemberNumberSpecified = true;
                Request.MemberNumber = dd;
                Request.AddressTypeId = "H";
                Request.MailingIndicator = "Y";
                Request.CountryCode = "KW";
                Request.Username = gicuser;
                Request.AddressText = Info.ADDRESS;
                Request.RegionCode = Info.REGION_ID;
                Request.DistrictCode = Info.DISTRICT_ID;
                Request.PhoneNumber1 = Info.PHONE_NUMBER1 == null ? "" : Info.PHONE_NUMBER1; //Changed for null value

                Resp = Client.createLifeAddress(Request);
                if (Resp.MemberNumber == Convert.ToDecimal(Info.MEMBER_NUMBER))
                {
                    return true;
                }
                else
                {
                    Logs ErrorClass = new Logs();
                    ErrorClass.ErrorCode = "MemberPortalEWsLifeAddressOver";
                    ErrorClass.ErorDesc = "Error MemberUpdateAddress MemberPortal Creation memberno =" + Info.MEMBER_NUMBER + " Over All Execution of WS MNI Issue for user " + username + " URL is /MedNeXtWebServices-Model/LifeAdressDS ";
                    ErrorClass.ErrorExp = "";
                    AddErrors Objec = new AddErrors();
                    Objec.InsertErrorLog(ErrorClass);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logs ErrorClass = new Logs();
                ErrorClass.ErrorCode = "MemberPortalEWsLifeAddressOverAll";
                ErrorClass.ErorDesc = "Error MemberUpdateAddress MemberPortal Creation memberno =" + Info.MEMBER_NUMBER + " Over All Execution of WS MNI Issue for user " + username + " URL is /MedNeXtWebServices-Model/LifeAdressDS ";
                ErrorClass.ErrorExp = ex.Message + "||" + ex.InnerException;
                AddErrors Objec = new AddErrors();
                Objec.InsertErrorLog(ErrorClass);
                return false;
            }

        }
    }
}