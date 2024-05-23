using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
    public interface ITravel
    {
        AfyaTravelCert GetCertDetails(string memberNo, string policyNo);
        int SaveCertDetails(AfyaTravelCert model);
        MemberInfoCert GetMemberInfo(string memberNo);
    }
}
