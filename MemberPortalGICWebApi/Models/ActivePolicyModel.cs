using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class ActivePolicyModel : IDataMapper
    {
        public string POLICY_HOLDER { get; set; }
        public DateTime POLICY_EFFECTIVE_DATE { get; set; }
        public DateTime EXPIRY_DATE { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string POLICY_NUMBER { get; set; }
        public string Network_Id { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            POLICY_HOLDER = dr.GetString("POLICY_HOLDER");
            POLICY_EFFECTIVE_DATE = dr.GetDateTime("POLICY_EFFECTIVE_DATE");
            EXPIRY_DATE = dr.GetDateTime("EXPIRY_DATE");
            POLICY_NUMBER = dr.GetInt32("POLICY_NUMBER").ToString();
            MEMBER_NUMBER = dr.GetInt32("MEMBER_NUMBER").ToString();
            Network_Id = dr.GetString("Network_Id");
        }
    }
}