using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
    public interface IClaims
    {
        IList<AgentsOnline> GetAllProviders();
        
       //  IList<ClaimsModel> GetpendningAndRejectedEntries(string Status, string policyNumber, string membernumber);
        IList<AgentRequests> GetpendningAndRejectedEntries(GetClaimRequestModel model);
        IList<AuthorizationServices> GETAuthorizationServicesWithSystemAmount(ProcedureloadModel model);

        IList<ClaimsModel> DALClaimsUser(string Status, string policyNumber, string membernumber, string Culture);


        }
}
