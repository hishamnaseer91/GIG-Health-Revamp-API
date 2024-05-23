using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class AgentRequests : IDataMapper
    {
        public long REQUEST_ID { get; set; }//Request ID
        public string Authno { get; set; }//Request ID
        public string CLM_NO { get; set; }//incident Number
        public string CLM_ASSURED_NAME { get; set; }//Company
        public string CLM_HOSP_NAME { get; set; }//hosptal Neme
        public int CTD_MEM_ID { get; set; }//Member ID
        public DateTime CLM_CR_DT { get; set; }//Authorization register Time
        public string CIVILID { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public string CTD_HOSP_EST_AMT { get; set; }//Estimated Amount
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal SYSTEM_ESTIMATED_COST_AMOUNT { get; set; }//Estimated Amount
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
            CLM_ASSURED_NAME = dr.GetString("COMPANY");
            CLM_CR_DT = dr.GetDateTime("CLM_TIME");
            CLM_HOSP_NAME = dr.GetString("HOSPITAL_NAME");
            CLM_NO = dr.GetInt32("CLAIM_NUMBER").ToString();
            SpecilityID = dr.GetString("SPECIALITY_ID");
            CTD_HOSP_EST_AMT = dr.GetInt32("ESTIMATED_AMOUNT").ToString();
            CTD_MEM_ID = dr.GetInt32("MEMBER_ID");
            RBY = dr.GetString("RBY");
            STATUS = dr.GetString("STATUS");
            REQUEST_ID = dr.GetInt64("ARID");
            CIVILID = dr.GetString("NATIONAL_IDENTITY");
            REJECTION_ID = dr.GetInt64("REJECTION_REASON_ID");
            REPORT_NOTES = dr.GetString("NOTES_REPORT");
            SYSTEM_ESTIMATED_COST_AMOUNT = dr.GetDecimal("SYSTEM_ESTIMATED_COST_AMOUNT");
            ResponseTime = dr.GetDateTime("MEDNEXT_STATUS_DT");
            AgentName = dr.GetString("CLM_ASSIGN_TO");
            if (dr["VIP"] != DBNull.Value)
            {
                VIP = Convert.ToString(dr["VIP"]);
                if (VIP == "Y")
                {
                   VIP = "Yes";
                }
                else
                {
                    VIP = "NO";
                }
            }
            else
            {
                VIP = default(string);
            }
        }
    }
    public class FamilyDetails
    {
        //public FamilyDetails();

        public string CIVIL_ID { get; set; }
        public DateTime DOB { get; set; }
        public int FaamilyID { get; set; }
        public string gender { get; set; }
        public string GUID { get; set; }
        public List<MemberAllClaims> MAllCalims { get; set; }
        public long Member_id { get; set; }
        public string Name { get; set; }
        public long Policy_Number { get; set; }
        public string PrincIpleCID { get; set; }
        public string PrincipleMember { get; set; }
        public string Relation { get; set; }
    }
    public class ARequestsDetails
    {

        public long REQUEST_ID { get; set; }//Request ID
        public long AuthNo { get; set; }
        public string CIVILID { get; set; }
        public string Patient_Name { get; set; }//incident Number
        public string VIP { get; set; }//Company
        public int Policy_Number { get; set; }//hosptal Neme
        public int Incident_Number { get; set; }//Member ID
        public string Assured_Code_NAME { get; set; }//hosptal Neme
        public string Mobile_No { get; set; }//Estimated Amount
        public string ICD_10_Diagnosis { get; set; }//Assigned to USER

        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }

        public string Diagnosis_Description { get; set; }//Assigned to USER
        public string Exclusions { get; set; }//
        public string AssignedToUser { get; set; }
        public string View_Attached { get; set; }
        public string AttachmentCount { get; set; }
        public string RBY { get; set; }
        public string DOCTOR_NAME { get; set; }



    }

    public class MemberAllClaims
    {
        // public MemberAllClaims();

        public string AUTH_STATUS { get; set; }
        public string AUTHORISATION_NUMBER { get; set; }
        public long Claim_No { get; set; }
        public string Company { get; set; }
        public string Created_By { get; set; }
        public DateTime Date_created { get; set; }
        public string Hospital { get; set; }
        public string MEDNEXT_STATUS_DT { get; set; }
        public long Member_ID { get; set; }
        public string NOTES { get; set; }
        public long Policy_number { get; set; }
        public string Remarks { get; set; }
        public string REQUEST_ID { get; set; }
        public string SpecilityID { get; set; }
        public decimal Total_Amount { get; set; }
    }

    public enum AStatusEnum
    {
        ApprovedRejected = 1,
        Approved = 2,
        Rejected = 3,
    }
}