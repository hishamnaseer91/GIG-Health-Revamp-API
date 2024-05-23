using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.Generics;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.LoginDAL;
using MemberPortalGICWebApi.DataObjects.Search;
using MemberPortalGICWebApi.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.Core
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        #region[ValidateClientAuthentication]
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        }
        #endregion

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            DBCommonError objeCommon = new DBCommonError();
            ErrorLogs_DB ErrorClass = new ErrorLogs_DB();
            string client_id = string.Empty;
            var requestHeaders = await context.Request.ReadFormAsync();
            var result = requestHeaders.ToList();

            if (result.Any(x => x.Key.ToLowerInvariant().Contains("client_id")))
            {
                if (result.Count > 4)
                {
                    var key = result.Where(x => x.Key == "client_id").FirstOrDefault();
                    client_id = key.Value[0].ToString().Trim(); ;
                    string requesObjStr = "";
                    foreach (var value in result.ToList())
                    {
                        requesObjStr = " " + requesObjStr + value.Key + " " + value.Value[0] + " ";
                    }

                    ErrorClass.ErrorCode = "Login request Params";
                    ErrorClass.ErorDesc = "Reqeust Count: " + result.Count() + " client_id:  " + key.Value[0].ToString();
                    ErrorClass.TypeError = "Success";
                    ErrorClass.ErrorExp = "Reqeust Obj: " + requesObjStr;



                    //objeCommon.InsertErrorLogs(ErrorClass);

                    if (client_id.StartsWith("I") || client_id.StartsWith("i")) //iphone codition only
                    {
                        if (client_id != "iPhone_forceUpdateImplemented")
                        {
                            var form = await context.Request.ReadFormAsync();

                            if (string.Equals(form["localization"], "en", StringComparison.OrdinalIgnoreCase))
                            {
                                context.SetError("invalid_grant", "An update is available for the app. Please update.");
                                return;
                            }
                            context.SetError("invalid_grant", "هنالك تحديث متوفر للتطبيق. الرجاء التحديث.");
                            return;
                        }
                    }


                }
            }
           



            var a = Common.Encrypt(context.Password);
            var s = Common.Decrypt("r4H6HLg6/Js=");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            //var Password = Common.Decrypt(context.Password);
            //using (AuthRepository _repo = new AuthRepository())
            //{
            //IdentityUser user = await _repo.FindUser(context.UserName, context.Password);
            ILoginUser login = new LoginUser();
            UserModel model = new UserModel() { UserName = context.UserName, Password = context.Password };
            var Result = login.CheckUserExistence(model);

            if (Result == -1 | Result == 0)

            {
                var form = await context.Request.ReadFormAsync();

                if (string.Equals(form["localization"], "en", StringComparison.OrdinalIgnoreCase))
                {
                    context.SetError("invalid_grant", "The Username or Password is Incorrect");
                    return;
                }
                context.SetError("invalid_grant", "الرقم المدني او كلمة المرور غير صحيح");
                return;
            }

            var userActive = login.UserIsActiveOrNot(Convert.ToInt64(Result));

            if (userActive != 1)
            {

                var form = await context.Request.ReadFormAsync();
                if (string.Equals(form["localization"], "en", StringComparison.OrdinalIgnoreCase))
                {
                    context.SetError("invalid_grant", "User Is InActive");
                    return;
                }
                context.SetError("invalid_grant", "المستخدم غير فعال");
                return;
            }
            //var data = login.BIZUserModel(Result.ToString());
            var data = login.BIZUserModel(model.UserName.ToString());
            if (data != null)
            {
                //To be uncommented as commmented for testing only 
                if (DateTime.Now >= data.MEMBEREXPIRY)
                {
                    var updatedPolicyModel = login.GetActivePolicyCivilId(model.UserName);

                    if (updatedPolicyModel == null)
                    {
                        var form = await context.Request.ReadFormAsync();

                        if (string.Equals(form["localization"], "en", StringComparison.OrdinalIgnoreCase))
                        {
                            context.SetError("invalid_grant", "No Policy Found Against Civil ID");
                            return;
                        }
                        context.SetError("invalid_grant", "لا توجد عضوية فعالة على الرقم المدني المسجل");
                        return;
                    }

                    if (data.POLICY_NUMBER != updatedPolicyModel.POLICY_NUMBERs)
                    {
                        int isUpdated = login.UpdatePolicyNumber(updatedPolicyModel);

                    }

                }
            }

            var claims = new List<Claim>()
                    {
                        new Claim("sub", context.UserName),
                       new Claim("role", "user")
                    };
            ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                        context.Options.AuthenticationType);

            var pkgNBR = data != null ? data.PackageNumber : "0";

            var properties = CreateProperties(model.UserName, pkgNBR, data.PRINCIPALINDICATOR, data.RelationshipDescription, data.PrincipalMemberName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);

        }


        #region[TokenEndpoint]
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        #endregion

        #region[CreateProperties]
        public static AuthenticationProperties CreateProperties(string userName, string packageNumber, string principleMember, string relationshipDesc, string PrincipalMemberName)
        {
            ISearch repo = new SearchDAL();




            var isDiscountApplicableData = repo.isDiscountOptionApplicable();
            var isDiscountOptionApplicablefromGlocalConfig = false;
            var isDiscountApplicablefromAfyaAndCurrentdate = false;
            var isDiscountOptionApplicable = false;

            var isClaimRegisterSmsVisibility = false;
            var isSmsClaimRegisterApplicable = false;

            var AfyaPolicyNumber = repo.ClaimRegisterSmsVisibility(userName);
            if (AfyaPolicyNumber != "-1")
            {
                isClaimRegisterSmsVisibility = true;

                var IsCivilIDExcluded = repo.ClaimRegisterEnable(userName);
                if (IsCivilIDExcluded == "-1")
                {
                    isSmsClaimRegisterApplicable = true;
                }
            }

            if (isDiscountApplicableData != null)
            {
                if (isDiscountApplicableData == 1)
                {
                    isDiscountOptionApplicablefromGlocalConfig = true;
                }
            }
            var CardPrintDate = repo.getCardPrintDate(userName);
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


            var list = repo.DALNetworkListDDLbyCivilIDInToken("en", userName);

            if (list.Count() > 0)
            {
                if (list.Count() == 1)
                {

                    IDictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "IsMoreThanOneActivePolicy", "False" }, {"PolicyNo", list[0].PolicyNo.ToString() } , {"MemNo", list[0].MemNo.ToString()  },
                        { "NetworkID", list[0].Network_Code.ToString() } , {"PackageNumber", packageNumber  }, {"isDiscountOptionApplicable",
                            isDiscountOptionApplicable.ToString()  } ,{ "isClaimRegisterSmsVisibility", isClaimRegisterSmsVisibility.ToString()  } ,{ "isSmsClaimRegisterApplicable", isSmsClaimRegisterApplicable.ToString()  }
                        ,{ "IsPrincipleMember",principleMember},{ "RelationshipDesc",relationshipDesc},{ "PrincipleMemberName",PrincipalMemberName}
                    };
                    return new AuthenticationProperties(data);
                }
                else
                {

                    IDictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "IsMoreThanOneActivePolicy", "True" }, {"PolicyNo", "" } , {"MemNo", ""  },
                        { "NetworkID", ""  } , {"PackageNumber", packageNumber  }, {"isDiscountOptionApplicable",
                            isDiscountOptionApplicable.ToString()  } ,{ "isClaimRegisterSmsVisibility", isClaimRegisterSmsVisibility.ToString()  } ,{ "isSmsClaimRegisterApplicable", isSmsClaimRegisterApplicable.ToString()  }
                            ,{ "IsPrincipleMember",principleMember},{ "RelationshipDesc",relationshipDesc},{ "PrincipleMemberName",PrincipalMemberName}
                    };
                    return new AuthenticationProperties(data);
                }
            }
            else
            {
                IDictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "IsMoreThanOneActivePolicy", "No Active Policies Found" }, {"PolicyNo", ""  } ,
                    { "NetworkID", ""  } , {"PackageNumber", packageNumber  },
                    { "isDiscountOptionApplicable", isDiscountOptionApplicable.ToString()  } ,{ "isClaimRegisterSmsVisibility", isClaimRegisterSmsVisibility.ToString()  } ,{ "isSmsClaimRegisterApplicable", isSmsClaimRegisterApplicable.ToString()  }
                        ,{ "IsPrincipleMember",principleMember},{ "RelationshipDesc",relationshipDesc},{ "PrincipleMemberName",PrincipalMemberName}
                };
                return new AuthenticationProperties(data);
            }

        }
        #endregion
    }
}
