using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class UserMemberUpdateModel : IDataMapper
    {
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string NETWORK { get; set; }
        public string NETWORK_ID { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string POLICY_NUMBER { get; set; }
        public string VIP_FLAG { get; set; }
        public string NATIONAL_IDENTITY { get; set; }
        public string POLICY_HOLDER { get; set; }
        public DateTime MEMBER_EFFECTIVE_DATE { get; set; }
        public DateTime MEMBEREXPIRY { get; set; }
        public DateTime DELETION_DATE { get; set; }
        public DateTime POLICY_EFFECTIVE_DATE { get; set; }
        public DateTime POLICYEXPIRY { get; set; }
        public DateTime POLICY_CANCELLATION_DATE { get; set; }

        public string PHONE_NUMBER1 { get; set; }
        public string AlternateMobile_No { get; set; }
        public string ADDRESS_TYPE_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string RELATION_ID { get; set; }
        public string GENDER_ID { get; set; }
        public DateTime EFFECTIVE_DATE { get; set; }
        public string PRINCIPALINDICATOR { get; set; }
        public int INSURANCE_COMPANY_NUMBER { get; set; }
        public DateTime DATE_OF_BIRTH { get; set; }

        public string ADDRESS { get; set; }
        public string REGION_ID { get; set; }
        public string DISTRICT_ID { get; set; }


        public string BlockNo { get; set; }
        public string StreetNo { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNo { get; set; }
        public string FlatNo { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            ADDRESS = dr.GetString("ADDRESS");
            REGION_ID = dr.GetString("REGION_ID");
            DISTRICT_ID = dr.GetString("DISTRICT_ID");

            Name = dr.GetString("NAME");
            ArabicName = dr.GetString("INTERNATIONAL_NAME");
            NETWORK = dr.GetString("NETWORK_DESCRIPTION");
            NETWORK_ID = dr.GetString("DEFAULT_NETWORK_ID");
            MEMBER_NUMBER = dr.GetInt32("MEMBER_NUMBER").ToString();
            POLICY_NUMBER = dr.GetInt32("POLICY_NUMBER").ToString();
            VIP_FLAG = dr.GetString("VIP_FLAG");
            NATIONAL_IDENTITY = dr.GetString("NATIONAL_IDENTITY");
            POLICY_HOLDER = dr.GetString("POLICY_HOLDER");
            PHONE_NUMBER1 = dr.GetString("PHONE_NUMBER1");
            ADDRESS_TYPE_ID = dr.GetString("ADDRESS_TYPE_ID");
            CLASS_ID = dr.GetString("CLASS_ID");
            RELATION_ID = dr.GetString("RELATION_ID");
            GENDER_ID = dr.GetString("GENDER_ID");
            PRINCIPALINDICATOR = dr.GetString("PRINCIPALINDICATOR");

            INSURANCE_COMPANY_NUMBER = dr.GetInt32("INSURANCE_COMPANY_NUMBER");

            MEMBER_EFFECTIVE_DATE = dr.GetDateTime("MEMBER_EFFECTIVE_DATE");
            MEMBEREXPIRY = dr.GetDateTime("MEMBEREXPIRY");
            DELETION_DATE = dr.GetDateTime("DELETION_DATE");
            POLICY_EFFECTIVE_DATE = dr.GetDateTime("POLICY_EFFECTIVE_DATE");
            POLICYEXPIRY = dr.GetDateTime("POLICYEXPIRY");
            POLICY_CANCELLATION_DATE = dr.GetDateTime("POLICY_CANCELLATION_DATE");


            DATE_OF_BIRTH = dr.GetDateTime("DATE_OF_BIRTH");
        }



        public string POLICY_VALID_DATE
        {
            get
            {
                return string.Format("{0} - {1} ", POLICY_EFFECTIVE_DATE.ToString("dd/MM/yyyy"), POLICYEXPIRY.ToString("dd/MM/yyyy"));
            }
        }

        public string Member_VALID_DATE
        {
            get
            {
                return string.Format("{0} - {1} ", MEMBER_EFFECTIVE_DATE.ToString("dd/MM/yyyy"), MEMBEREXPIRY.ToString("dd/MM/yyyy"));
            }
        }



        public string MemberStatus
        {
            get
            {

                return DELETION_DATE > default(DateTime) ? "InActive" : "Active";


            }
        }
        public string PolicyStatus
        {
            get
            {

                return POLICYEXPIRY > DateTime.Now ? "Active" : "InActive";


            }
        }


        //extrafields

        public string Block
        {
            get
            {
                if (ADDRESS != null)
                {
                    if (ADDRESS.Contains('|'))
                    {
                        string[] words = ADDRESS.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length > 0)
                        {
                            return words[1];
                        }
                    }
                }
                return null;
            }
        }
        public string Street
        {
            get
            {
                if (ADDRESS != null)
                {
                    if (ADDRESS.Contains('|'))
                    {
                        string[] words = ADDRESS.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length > 0)
                        {
                            return words[3];
                        }
                    }
                }
                return null;
            }
        }
        public string Building
        {
            get
            {
                if (ADDRESS != null)
                {
                    if (ADDRESS.Contains('|'))
                    {
                        string[] words = ADDRESS.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length > 0)
                        {
                            return words[5];
                        }
                    }
                }
                return null;
            }
        }
        public string Floor
        {
            get
            {
                if (ADDRESS != null)
                {
                    if (ADDRESS.Contains('|'))
                    {
                        string[] words = ADDRESS.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length > 7)
                        {
                            return words[7];
                        }
                    }
                }
                return null;
            }
        }
        public string Flat
        {
            get
            {
                if (ADDRESS != null)
                {
                    if (ADDRESS.Contains('|'))
                    {
                        string[] words = ADDRESS.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length > 9)
                        {
                            return words[9];
                        }
                    }
                }
                return null;
            }
        }

        public string Notes
        {
            get
            {
                if (ADDRESS != null)
                {
                    if (ADDRESS.Contains('|'))
                    {
                        string[] words = ADDRESS.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length > 11)
                        {
                            return words[11];
                        }
                    }
                }
                return null;
            }
        }
    }

    public class ProfileAddress
    {
        public string REGION_ID { get; set; }
        public string DISTRICT_ID { get; set; }
        public string BlockNo { get; set; }
        public string StreetNo { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNo { get; set; }
        public string FlatNo { get; set; }
    }

    public class ClaimsStats : IDataMapper
    {
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            Pending = dr.GetInt32("Pending");
            Approved = dr.GetInt32("Approved");
            Rejected = dr.GetInt32("Rejeceted");
        }
    }
    public class MemberAddressExistence : IDataMapper
    {

        public string AddressNumber { get; set; }
        public string AddressType { get; set; }
        public string MemeberId { get; set; }

        public string Fax { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            AddressNumber = dr.GetInt32("ADDRESS_NUMBER").ToString();
            AddressType = dr.GetString("ADDRESS_TYPE_ID");
            MemeberId = dr.GetInt32("MEMBER_NUMBER").ToString();
            Fax = dr.GetString("FAX");
        }
    }
}
