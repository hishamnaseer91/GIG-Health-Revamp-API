using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
   public interface IRegister
    {
        bool IsMemberExist(string civilId);
        bool IsRegistrationComplete(string civilId);
        AddMemberUserModel GetMemberUserDetails(string civilId,string memberID); // used to get only some properties for Add model
        int UpdateRegisterMemberComplete(RegisterMemberCompleteModel model); // used to update values of [isfirstlogin and reg complete]

        MemberUser GetMemberProfileDetails(string Id);
        int AddNewMember(AddMemberUserModel model);
        string IsValidUser(RegisterMemberModel Model);
        ForgotPassoword ForgotPassword(ResetPasswordModel model);
        int AddSMS(SmsModel model);

        int UpdatePassword(string CivilId, string Password);
        MemberNameModel GetMemberName(string civilId);
        ActivePolicyModel GetActivePolicyCivilId(string civilID);
        MemberInfo GetName(string civilID);
        string LastFourDigits(string civilID);
    }
}
