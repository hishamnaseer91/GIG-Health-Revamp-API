using MemberPortalGICWebApi.DataObjects;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class DbModel : IDataMapper
    {
        public string CIVIL_ID { get; set; }//Request ID
        public string Policy_Number { get; set; }//hosptal Neme
        public string MEMBER_ID { get; set; }//hosptal Neme

        public string MEDICAL_INSURANCE_CARD { get; set; }

        public string MOBILE_NO { get; set; }

        public bool IS_FIRST_LOGIN
        {
            get; set;
        }
        public bool IS_ACTIVE
        {
            get; set;
        }
        public bool REGISTRATION_COMPLETE
        {
            get; set;
        }

   public string password { get; set; }
        public DateTime CREATION_DATE { get; set; }

        public string NETWORK_ID { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            CIVIL_ID = dr.GetDecimal("CIVIL_ID").ToString();
            Policy_Number = dr.GetString("POLICY_NO");
            MEMBER_ID = dr.GetString("MEMBER_ID");
            MEDICAL_INSURANCE_CARD = dr.GetString("MEDICAL_INSURANCE_CARD");
            MOBILE_NO = dr.GetString("MOBILE_NO");
            password = Common.Decrypt(dr.GetString("PASSWORD"));
            IS_FIRST_LOGIN= dr.GetBooleanExtra("IS_FIRST_LOGIN");
            IS_ACTIVE = dr.GetBooleanExtra("IS_ACTIVE");
            REGISTRATION_COMPLETE = dr.GetBooleanExtra("REGISTRATION_COMPLETE");
            CREATION_DATE =  dr.GetDateTime("CREATION_DATE");
            NETWORK_ID = dr.GetString("NETWORK_ID");
            ClientId = dr.GetString("DEVICE_ID");
            ClientSecret = dr.GetString("USER_AGENT");

        }
    }
}