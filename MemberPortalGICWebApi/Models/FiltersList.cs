using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class FiltersList
    {

        static List<ClaimStausModel> claimStatuslist = new List<ClaimStausModel>
        {
            new ClaimStausModel { Id=1 , Description="ApprovedRejected"},
            new ClaimStausModel { Id=2 , Description="Approved" },
            new ClaimStausModel { Id=3 , Description="Rejected" }

        };



        static List<IncidentType> incidentTypelist = new List<IncidentType>
        {
            new IncidentType { Id=1 , Description="Hosptlization"},
            new IncidentType { Id=2 , Description="OutPatient" },
              new IncidentType { Id=3 , Description="Dental" },
              new IncidentType { Id=4 , Description="Chronic" }

        };

        public IList<ClaimStausModel> GetClaimStatus()
        { return claimStatuslist; }

        public IList<IncidentType> GetIncidentTypelist()
        { return incidentTypelist; }

    }





}