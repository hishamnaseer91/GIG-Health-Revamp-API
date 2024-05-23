using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class PatientBasicInfo : IDataMapper
    {
        public string CIVIL_ID { get; set; }//Request ID
        public string Patient_Name { get; set; }//incident Number
        public bool VIP { get; set; }//Company
        public string Policy_Number { get; set; }//hosptal Neme
        public DateTime DOB { get; set; }
        public string Assured_NAME { get; set; }//hosptal Neme
        public int Mobile_No { get; set; }//Estimated Amount
        public DateTime Member_EXpiryDate { get; set; }//Assigned to USER

        public DateTime Member_EffecctdDate { get; set; }//new added with change

        public string Exclusions { get; set; }//
        public string MemberNumber { get; set; }//
        public string AUTH_STATUS { get; set; }
        [DisplayFormat(DataFormatString = "{0,d}")]
        public DateTime Policy_ExpiryDate { get; set; }
        [DisplayFormat(DataFormatString = "{0,d}")]
        public DateTime policyEffective_Date { get; set; }
        public int DEFAULT_NETWORK_ID { get; set; }
        List<PlanDDL> PlanDDld { get; set; }
        public List<MEMBEREXCLUSION> Excluionslist { get; set; }
        public DateTime DELETIONDATE { get; set; }
        //new added for address new fields.
        public string Governorate { get; set; }
        public string Area { get; set; }
        public string Block { get; set; }
        public string StreetNo { get; set; }

        //latest address changes
        public string Building { get; set; }
        public string FloorNo { get; set; }
        public string FlatNo { get; set; }

        public string AlternateMobile_No { get; set; }

        public int IsPhoneNumberUpdate { get; set; }
        public int IsAddressUpdate { get; set; }
        public int IsCardPrinted { get; set; }


        public string FreeText { get; set; }
        public string Location { get; set; }
        public string By { get; set; }
        public int ID { get; set; }//only for print card add/update history
        public string POLICY_VALID_DATE
        {
            get
            {
                return string.Format("{0} - {1} ", policyEffective_Date.ToString("dd/MM/yyyy"), Policy_ExpiryDate.ToString("dd/MM/yyyy"));
            }
        }
        public string Member_VALID_DATE
        {
            get
            {
                return string.Format("{0} - {1} ", Member_EffecctdDate.ToString("dd/MM/yyyy"), Member_EXpiryDate.ToString("dd/MM/yyyy"));
            }
        }
        public string MemberStatus
        {
            get
            {

                return DELETIONDATE > default(DateTime) ? "InActive" : "Active";


            }
        }
        public string PolicyStatus
        {
            get
            {

                return Policy_ExpiryDate > DateTime.Now ? "Active" : "InActive";


            }
        }
        public void MapProperties(DbDataReader dr)
        {
            Patient_Name = dr.GetString("NAME").ToString();
            //MemberStatus = dr.GetDecimal("NAME").ToString();
            Policy_Number = dr.GetDecimal("POLICY_NUMBER").ToString();
            Assured_NAME= dr.GetString("POLICY_HOLDER").ToString();
            CIVIL_ID = dr.GetString("NATIONAL_IDENTITY").ToString();
            Policy_ExpiryDate= dr.GetDateTime("POLICYEXPIRY");
            DELETIONDATE = dr.GetDateTime("DELETION_DATE");
            //PolicyStatus = dr.GetString("POLICY_HOLDER").ToString();
        }

        public class MEMBEREXCLUSION
        {
            public string SPECIAL_EXCLUSION_NAME { get; set; }
            public DateTime WAIVER_DATE { get; set; }
            //a.WAIVER_DATE,a.SPECIAL_EXCLUSION_NAME
        }


        public class ClaimFiles
        {
            public string FileName { get; set; }
            public string Authno { get; set; }
            public string upladedBy { get; set; }

            //a.WAIVER_DATE,a.SPECIAL_EXCLUSION_NAME
        }

        public class PlanDDL
        {
            public string BenefitCode { get; set; }//incident Number
            public string BenifitDesc { get; set; }//incident Number
            public string Displayname
            {
                get
                {
                    return string.Format("{0} || {1}", BenefitCode, BenifitDesc);
                }

            }
        }
    }
}
