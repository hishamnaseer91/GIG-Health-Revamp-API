using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
    interface ILimits
    {
        
        IList<GetPolicies> GetPolicybyCivilId(LimitCheckModel model);
        PatientBasicInfo GetPatientBasicInfo(GetPersonalInformationModel model);
        MemberUserGender GetMemberGender(string memberId);
        PolicyMemberNumber GetMemberPolicayNo(string civilID);

        MemberNumber GetMemberNo(string civilID, string pol);
    }
}
