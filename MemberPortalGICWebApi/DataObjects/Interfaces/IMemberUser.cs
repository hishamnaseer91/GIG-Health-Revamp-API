using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemberPortalGICWebApi.Models;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
    public interface IMemberUser
    {
        IList<MemberUser> GetMemberAssuredName(string civilId);
        //MemberUser GetMemberProfile(string civilId);
        UserProfileModel GetMemberProfile(string civilId, string pol);
        MemberUsers GetmemberUser(string civilID);
        int ChangePassword(ChangePasswordModel model);
        long isCorrectOldPassword(ChangePasswordModel Model);

        IList<DDLRegion> DALRegionDDL();

        IList<DDLRegionArea> DALAreaDDL(int Region);

        int IsUSerCanUpdateAddress();

        long GetMemberId(string civilId, string polNo);

        UserMemberUpdateModel GetUserDetailModel(string civilId, string polNo);

        int UpdateAddressLocalHistory(UserMemberUpdateModel model, string UserId);

        MemberAddressExistence CheckMemberExistence(string MemberNumber);

        IList<Notifications> GetAllNotifications(string civilIdd);
        bool IsFirstPasswordChange(string civilId);

        MemberPolicyandMemberNoModel GetMemberPolicyMemberNumber(string civilId);
        int DeleteMemberProfile(DeleteMemberModel model);
        int InsertDeleteMemberLog(MemberUsers model, string DELETION_REMARKS);
        ECardValue GetECardDetail(string civilId, string pol, string memNo);
    }
}
