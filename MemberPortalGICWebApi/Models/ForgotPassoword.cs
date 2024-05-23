using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class ForgotPassoword : IDataMapper
    {
        public string CivilId { get; set; }
        public string MedicalInsuranceCard { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            CivilId = dr.GetDecimal("CIVIL_ID").ToString();
            MedicalInsuranceCard = dr.GetString("MEDICAL_INSURANCE_CARD");
            Password = dr.GetString("PASSWORD");
            MobileNo = dr.GetString("MOBILE_NO");
        }
    }
}