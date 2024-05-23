using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class Policy : IDataMapper
    {
        public string Assured_NAME { get; set; }
        public int MemberNumber { get; set; }
        public string NETWORK_ID { get; set; }
        public string Policy_ExpiryDate { get; set; }
        public int Policy_Number { get; set; }

        public string PackageNumber { get; set; }
        public string policyEffective_Date { get; set; }

        //public string POLICY_VALID_DATE
        //{
        //    get
        //    {
        //        return string.Format("{0} - {1} ", policyEffective_Date.ToString("dd/MM/yyyy"), Policy_ExpiryDate.ToString("dd/MM/yyyy"));
        //    }
        //}
        public void MapProperties(DbDataReader dr)
        {
            Assured_NAME = dr.GetString("POLICY_HOLDER");
            MemberNumber = dr.GetInt32("MEMBER_NUMBER");
            NETWORK_ID = dr.GetString("NETWORK_ID");
            Policy_ExpiryDate = dr.GetDateTime("EXPIRY_DATE").ToString("dd/MM/yyyy");
            Policy_Number = dr.GetInt32("POLICY_NUMBER");
            //POLICY_VALID_DATE = dr.GetInt32("POLICY_VALID_DATE");
            policyEffective_Date = dr.GetDateTime("POLICY_EFFECTIVE_DATE").ToString("dd/MM/yyyy");
            PackageNumber = dr.GetInt32("PACKAGE_NUMBER").ToString();
        }
    }
}