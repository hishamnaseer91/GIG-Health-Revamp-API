using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class MappingUserModel : IDataMapper

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
        public string ADDRESS_TYPE_ID { get; set; }
        public string CLASS_ID { get; set; }
        public string RELATION_ID { get; set; }
        public string GENDER_ID { get; set; }
        public DateTime EFFECTIVE_DATE { get; set; }
        
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

        public string PackageNumber { get; set; }
        public string PRINCIPALINDICATOR { get; set; }
        public string RelationshipDescription { get; set; }
        public string PrincipalMemberName { get; set; }

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
            PackageNumber = dr.GetInt32("PACKAGE_NUMBER").ToString();

            DATE_OF_BIRTH = dr.GetDateTime("DATE_OF_BIRTH");
            RelationshipDescription = dr.GetString("RELATION_DESCRIPTION");
            PrincipalMemberName = dr.GetString("PRINCIPAL_NAME");
        }
     
    }

    public class ActivePolicyModels : IDataMapper
    {
        public string CivilId { get; set; }
        public string POLICY_HOLDERs { get; set; }
        public DateTime POLICY_EFFECTIVE_DATEs { get; set; }
        public DateTime EXPIRY_DATEs { get; set; }
        public string MEMBER_NUMBERs { get; set; }
        public string POLICY_NUMBERs { get; set; }
        public string Network_Ids { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            CivilId = dr.GetString("NATIONAL_IDENTITY");
            POLICY_HOLDERs = dr.GetString("POLICY_HOLDER");
            POLICY_EFFECTIVE_DATEs = dr.GetDateTime("POLICY_EFFECTIVE_DATE");
            EXPIRY_DATEs = dr.GetDateTime("EXPIRY_DATE");
            POLICY_NUMBERs = dr.GetInt32("POLICY_NUMBER").ToString();
            MEMBER_NUMBERs = dr.GetInt32("MEMBER_NUMBER").ToString();
            Network_Ids = dr.GetString("Network_Id");
        }
    }
}