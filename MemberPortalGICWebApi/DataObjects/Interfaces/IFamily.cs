using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
    public interface IFamily
    {

        IList<Policy> GetPolicybyCivilId(string civilId);
        IList<FamilyInfo> GetFamilyDetails(long memberNumber, long policyNumber);
    }
}
