using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class MemberUser : IDataMapper
    {
        public string Name { get; set; }
        public string ArabicName { get; set; }

        public string Mobile_Number { get; set; }
        public string Alternate_Number { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string POLICY_NUMBER { get; set; }

        public string CivilId { get; set; }

        public string ADDRESS { get; set; }
        public string Governerate_iD { get; set; }
        public string Area_ID { get; set; }
        public string BlockNo { get; set; }
        public string StreetNo { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNo { get; set; }
        public string FlatNo { get; set; }

        public string Assured_Name { get; set; }

        public DateTime DATE_OF_BIRTH { get; set; }

        public DateTime Policy_Start_Date { get; set; }

        public DateTime Policy_End_Date { get; set; }
       // public string PolicyHolder { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            Name = dr.GetString("MEMBER_NAME");
            ArabicName = dr.GetString("MEMBER_NAME_AR");
            Mobile_Number = dr.GetString("MOBILE_NUMBER");
            Alternate_Number = dr.GetString("OPTIONAL_NUMBER");
            //ADDRESS = dr.GetString("ADDRESS");
            Governerate_iD = dr.GetInt32("REGION_ID").ToString();
            Area_ID = dr.GetInt32("DISTRICT_ID").ToString();

            BlockNo = dr.GetString("BLOCK_NO");
            StreetNo = dr.GetString("STREET_NO");
            BuildingNo = dr.GetString("BUILDING_NO");
            FloorNo = dr.GetString("FLOOR_NO");
            FlatNo = dr.GetString("FLAT_NO");


            MEMBER_NUMBER = dr.GetInt32("MEM_ID").ToString();
            POLICY_NUMBER = dr.GetInt32("POL_ID").ToString();

            CivilId = dr.GetString("NATIONAL_IDENTITY");
            Assured_Name = dr.GetString("POLICY_HOLDER");
            Policy_Start_Date = dr.GetDateTime("EFFECTIVE_DATE");
            Policy_End_Date = dr.GetDateTime("EXPIRY_DATE");
            DATE_OF_BIRTH = dr.GetDateTime("DATE_OF_BIRTH");


        }




    }

    ////////////// Profile User 
    public class UserProfileModel : IDataMapper
    {
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Mobile_Number { get; set; }
        public string Alternate_Number { get; set; }
        public string MEMBER_NUMBER { get; set; }
        public string POLICY_NUMBER { get; set; }

        public string CivilId { get; set; }

        public string ADDRESS { get; set; }
        public string Governerate_iD { get; set; }
        public string Area_ID { get; set; }
        public string BlockNo
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
        public string StreetNo
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
        public string BuildingNo
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
        public string FloorNo
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
        public string FlatNo
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
        public string Assured_Name { get; set; }

        public DateTime DATE_OF_BIRTH { get; set; }

        public DateTime Policy_Start_Date { get; set; }

        public DateTime Policy_End_Date { get; set; }
        public string PrincipalIndicator { get; set; }
        public string RelationshipDescription { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            Name = dr.GetString("NAME");
            ArabicName = dr.GetString("INTERNATIONAL_NAME");
            Mobile_Number = dr.GetString("PHONE_NUMBER1");
            MEMBER_NUMBER = dr.GetInt32("MEMBER_NUMBER").ToString();
            POLICY_NUMBER = dr.GetInt32("POLICY_NUMBER").ToString();
            CivilId = dr.GetString("NATIONAL_IDENTITY");
            Assured_Name = dr.GetString("POLICY_HOLDER");
            ADDRESS = dr.GetString("ADDRESS");
            Governerate_iD = dr.GetString("REGION_ID");
            Area_ID = dr.GetString("DISTRICT_ID");
            Policy_Start_Date = dr.GetDateTime("POLICY_EFFECTIVE_DATE");
            Policy_End_Date = dr.GetDateTime("POLICYEXPIRY");
            DATE_OF_BIRTH = dr.GetDateTime("DATE_OF_BIRTH");
            PrincipalIndicator = dr.GetString("PRINCIPALINDICATOR");
            RelationshipDescription = dr.GetString("RELATION_DESCRIPTION");
        }    
    }

    public class MemberNameModel : IDataMapper
    {
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string CivilId { get; set; }

        public string policyNumber { get; set; }

        public string memNo { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            Name = dr.GetString("NAME");
            CivilId = dr.GetString("NATIONAL_IDENTITY");
            ArabicName = dr.GetString("INTERNATIONAL_NAME");
            memNo = dr.GetInt32("MEMBER_NUMBER").ToString();
            policyNumber = dr.GetInt32("POLICY_NUMBER").ToString();
        }
    }

    public class MemberPolicyandMemberNoModel : IDataMapper {

        public string POLICY_NO { get; set; }
        public string MEMBER_ID { get; set; }
        
        public void MapProperties(DbDataReader dr)
        {
            POLICY_NO = dr.GetString("POLICY_NO");
            MEMBER_ID = dr.GetString("MEMBER_ID");
        
        }
    }
    public class MemberInfoCert : IDataMapper
    {
        public DateTime DATE_OF_BIRTH { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            DATE_OF_BIRTH = dr.GetDateTime("DATE_OF_BIRTH");
        }
    }
    public class MemberUserGender : IDataMapper {


        public string MemberId { get; set; }
        public string GenderId { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            MemberId = dr.GetInt32("MEMBER_NUMBER").ToString();
            GenderId = dr.GetString("SEX_ID");

        }
    }

    public class MemberUsers : IDataMapper
    {
        public decimal ID { get; set; }
        public string CIVIL_ID { get; set; }
        public string POLICY_NO { get; set; }
        public string MEMBER_ID { get; set; }
        public string MEDICAL_INSURANCE_CARD { get; set; }
        public string MOBILE_NO { get; set; }
        public string PASSWORD { get; set; }
        public bool IS_FIRST_LOGIN { get; set; }
        public bool IS_ACTIVE { get; set; }
        public bool REGISTRATION_COMPLETE { get; set; }
        public string DISABLE_REMARKS { get; set; }
        public DateTime DISABLE_DATE { get; set; }
        public string DEFAULT_LANG { get; set; }
        public string PRIMARY_EMAIL { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public DateTime UPDATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string NETWORK_ID { get; set; }
        public string DEVICE_ID { get; set; }
        public string USER_AGENT { get; set; }
        public decimal ISFIRSTPASWORDCHANGED { get; set; }
        public bool AFYA { get; set; }
        public string PRINCIPALMEMBER { get; set; }
        public string RELATIONSHIP_DESCRIPTION { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            ID = dr.GetDecimal("ID");
            CIVIL_ID = dr.GetDecimal("CIVIL_ID").ToString();
            POLICY_NO = dr.GetString("POLICY_NO");
            AFYA = Convert.ToBoolean(dr.GetDecimal("AFYA"));
            MEMBER_ID = dr.GetString("MEMBER_ID");
            MEDICAL_INSURANCE_CARD = dr.GetString("MEDICAL_INSURANCE_CARD");
            MOBILE_NO = dr.GetString("MOBILE_NO");
            PASSWORD = dr.GetString("PASSWORD");
            PRIMARY_EMAIL = dr.GetString("PRIMARY_EMAIL");
            CREATION_DATE = dr.GetDateTime("CREATION_DATE");
            DEVICE_ID = dr.GetString("DEVICE_ID").ToString();
            USER_AGENT = dr.GetString("USER_AGENT").ToString();
            PRINCIPALMEMBER = dr.GetString("PRINCIPALMEMBER");
            RELATIONSHIP_DESCRIPTION = dr.GetString("RELATIONSHIP_DESCRIPTION");
        }

    }
}

