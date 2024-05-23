using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class ClaimsModel : IDataMapper
    {
        public long REQUEST_ID { get; set; }//Request ID
        public string Authno { get; set; }//Request ID
        public string CLM_NO { get; set; }//incident Number
        public string CLM_ASSURED_NAME { get; set; }//Company
        public string CLM_HOSP_NAME { get; set; }//hosptal Neme
        public int CTD_MEM_ID { get; set; }//Member ID
        public DateTime CLM_CR_DT { get; set; }//Authorization register Time
        public string CIVILID { get; set; }

        public string CTD_HOSP_EST_AMT { get; set; }//Estimated Amount

        public string SYSTEM_ESTIMATED_COST_AMOUNT { get; set; }//Estimated Amount
        public string CLM_ASSIGN_TO { get; set; }//Assigned to USER
        public string PKGNO { get; set; }
        public string NETWORK_CODE { get; set; }
        public string NetWorkDescption { get; set; }
        public string STATUS { get; set; }//

        public string POL { get; set; }
        public string RBY { get; set; }
        public string VIP { get; set; }
        public string Connection_ID { get; set; }
        public string Rejection_Desc { get; set; }
        public long REJECTION_ID { get; set; }
        public string REPORT_NOTES { get; set; }

        public string SpecilityID { get; set; }
        public int SEEN { get; set; }
        public string AgentName { get; set; }
        public DateTime ResponseTime { get; set; }
        public long CLM_ASSIGNMENT_ID { get; set; }
        public ARequestsDetails Details { get; set; }

        public List<FamilyDetails> Family { get; set; }
        //public List<MedicalHistory> MedHistory { get; set; } //GicCipDataObjects.BENEFITS_WEB_SERVICE.WsCoverageBalancesOutRecUser
        //   public List<WsCoverageBalancesOutRecUser> Benifits { get; set; }
        public string ATYPE { get; set; }
        public bool ISAppeal { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            POL = dr.GetInt32("POLICY_NUMBER").ToString();
            Authno = dr.GetInt32("REQUEST_ID").ToString();
            CLM_ASSURED_NAME = dr.GetString("POLICY_HOLDER"); // rem
            CLM_CR_DT = dr.GetDateTime("CLAIM_REGISTER_TIME");
            CLM_HOSP_NAME = dr.GetString("HOSPITAL_NAME");
            CLM_NO = dr.GetInt32("CLAIM_NUMBER").ToString();
            SpecilityID = dr.GetString("SPECIALITY_ID");
            CTD_HOSP_EST_AMT = dr.GetString("ESTIMATED_AMOUNT").Trim();
            SYSTEM_ESTIMATED_COST_AMOUNT = dr.GetString("ESTIMATED_AMOUNT").Trim();
            CTD_MEM_ID = dr.GetInt32("MEMBER_ID");
          //  RBY = dr.GetString("RBY");
            STATUS = dr.GetString("AUTHORISATION_STATUS_DESCR");
            REQUEST_ID = dr.GetInt64("REQUEST_ID");
            /// CIVILID = dr.GetString("NATIONAL_IDENTITY");
            //  REJECTION_ID = dr.GetInt64("REJECTION_REASON_ID");
            //  REPORT_NOTES = dr.GetString("NOTE");
            if (STATUS == "Registered" || STATUS == "registered")
            { ResponseTime = dr.GetDateTime("CLAIM_REGISTER_TIME"); }
            else {
                ResponseTime = dr.GetDateTime("MEDNEXT_STATUS_DT");
            }
           

        }

    }
}