using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class GetPolicies : IDataMapper
    {
        public string Policy_Number { get; set; }//hosptal Neme
        public string MemberNumber { get; set; }//hosptal Neme

        public string Assured_NAME { get; set; }//hosptal Neme

        public DateTime Policy_ExpiryDate { get; set; }

        public DateTime policyEffective_Date { get; set; }

        public string POLICY_VALID_DATE
        {
            get
            {
                return string.Format("{0} - {1} ", policyEffective_Date.ToString("dd/MM/yyyy"), Policy_ExpiryDate.ToString("dd/MM/yyyy"));
            }
        }
        public void MapProperties(DbDataReader dr)
        {
            Policy_Number = dr.GetDecimal("POLICY_NUMBER").ToString();
            MemberNumber = dr.GetDecimal("MEMBER_NUMBER").ToString();
            Assured_NAME = dr.GetString("POLICY_HOLDER");
            policyEffective_Date = dr.GetDateTime("POLICY_EFFECTIVE_DATE");
            Policy_ExpiryDate = dr.GetDateTime("EXPIRY_DATE");
        }
    }

    public class PolicyMemberNumber : IDataMapper {

        public string PolicyNumber { get; set; }
        public string MemberNumber { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            PolicyNumber = dr.GetString("POLICY_NO").ToString();
            MemberNumber = dr.GetString("MEMBER_ID").ToString();
          
        }
    }

    public class MemberNumber : IDataMapper
    {

        public string PolicyNumbers { get; set; }
        public string MemberNumbers { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            PolicyNumbers = dr.GetInt32("POLICY_NO").ToString();
            MemberNumbers = dr.GetInt32("MEMBER_ID").ToString();

        }
    }
}