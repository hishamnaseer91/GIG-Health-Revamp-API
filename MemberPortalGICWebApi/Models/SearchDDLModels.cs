using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class SearchDDLModels
    {
    }

    public class DDLProviderType : IDataMapper
    {
        public string ProviderTypeID { get; set; }
        public string ProviderType { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            ProviderTypeID = dr.GetString("Value");
            ProviderType = dr.GetString("Name");
        }
    }

    public class DDLProviderTypeInput {

        public string Culture { get; set; }

        public string CivilId { get; set; }

        public int PlanId { get; set; }

    }


    public class DDLNetworkCardMapping : IDataMapper
    {
        public string planid { get; set; }

        public string policy_no { get; set; }
        public string policy_desc { get; set; }
        public string showpolicy_list { get; set; }
        public string locationtype_id { get; set; }

        public string default_selection { get; set; }
        public string stopedate { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            planid = dr.GetInt32("PLAN_ID").ToString();
            policy_no = dr.GetInt32("POLICY_NO").ToString();
            policy_desc = dr.GetString("POLICY_DESC").ToString();
            showpolicy_list = !string.IsNullOrEmpty(dr.GetInt32("SHOW_POLICY_LIST").ToString()) ? Convert.ToBoolean(dr.GetInt32("SHOW_POLICY_LIST")).ToString() : "False";
            default_selection = !string.IsNullOrEmpty(dr.GetInt32("DEFAULT_SELECTION").ToString()) ? Convert.ToBoolean(dr.GetInt32("DEFAULT_SELECTION")).ToString() : "False";
            locationtype_id = dr.GetInt32("LOCATION_TYPE_ID").ToString();
        }
    }

    public class DDLPlans : IDataMapper
    {
        public int Plan_Code { get; set; }
        public string Plan_Desc { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            Plan_Code = dr.GetInt32("PLAN_ID");
            Plan_Desc = dr.GetString("PLAN_DESC");
        }
    }


    public class DDLLocationTypes : IDataMapper
    {
        public string Location_Type_ID { get; set; }
        public string Location_Type_Name { get; set; }

        public string Culture { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            Location_Type_ID = dr.GetInt32("ID").ToString();
            Location_Type_Name = dr.GetString("TYPE_NAME");
        }
    }

    public class DDLNetwork : IDataMapper
    {
        public int Network_Code { get; set; }
        public string Network_Desc { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            Network_Code = dr.GetInt32("NETWORK_ID");
            Network_Desc = dr.GetString("POLICY_DESC");
        }
    }

    public class DDLNetworkByCivilId : IDataMapper
    {
        public string Network_Code { get; set; }
        public string Network_Desc { get; set; }
        public string PolicyNo { get; set; }
        public string MemNo { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            Network_Code = dr.GetString("NETWORK_ID");
            Network_Desc = dr.GetString("POLICY_DESC");
            MemNo = dr.GetInt32("MEMBER_NUMBER").ToString();
            PolicyNo = dr.GetInt32("POLICY_NUMBER").ToString();
        }
    }

    public class DDLProviders : IDataMapper
    {
        public string ProviderName { get; set; }
        public string ProviderID { get; set; }
        public void MapProperties(DbDataReader dr)
        {

            ProviderID = dr.GetString("Value");
            ProviderName = dr.GetString("Name");
        }
    }

    public class DDLProviderInput
    {

        public string Culture { get; set; }

        public string ProviderTypeID { get; set; }
    }

    public class DDLSpecialty : IDataMapper
    {
        public string SpecialtyName { get; set; }
        public string SpecialtyID { get; set; }
        public void MapProperties(DbDataReader dr)
        {

            SpecialtyID = dr.GetString("Value");
            SpecialtyName = dr.GetString("Name");
        }
    }
    public class DDLSpecialtyInput
    {

        public string Culture { get; set; }
        public string ProviderTypeID { get; set; }

    }

    public class DDLRegionInput
    {

        public string Culture { get; set; }

    }

    public class DALAreaDDLInput
    {
        public string Culture { get; set; }
        public string RegionId { get; set; }
    }

    public class SearchModel
    {

        public string ProviderType { get; set; }
        public string Providers { get; set; }
        public string Specialty { get; set; }
        public string Region { get; set; }
        public string FreeTextBox { get; set; }
        public string Area { get; set; }
        public string CivilId { get; set; }
        public string NetworkId { get; set; }
    }


    public class SearchResult : IDataMapper
    {
        public string HOSPTIALID { get; set; }
        public string PROVIDER_ID { get; set; }
        public string PROVIDER_NAME { get; set; }
        public string PROVIDER_NAME_AR { get; set; }
        public string PROVIDER_TYPE_ID { get; set; }
        public string PROVIDER_TYPE_DESCRIPTION { get; set; }
        public string PROVIDER_TYPE_DESCRIPTION_AR { get; set; }
        public string MAIN_PROVIDER_NAME { get; set; }
        public string MAIN_PROVIDER_NAME_AR { get; set; }
        public string SPECIALITY_ID { get; set; }
        public string SPECIALITY_DESCRIPTION { get; set; }
        public string SPECIALITY_DESCRIPTION_AR { get; set; }
        public string SEX_ID { get; set; }
        public string SEX_DESCRIPTION { get; set; }
        public string SEX_DESCRIPTION_AR { get; set; }
        public string LOCATION_TYPE_ID { get; set; }
        public string LOCATION_TYPE_DESCRIPTION { get; set; }
        public string ADDRESS { get; set; }
        public string ADDRESS_AR { get; set; }
        public string REGION_ID { get; set; }
        public string REGION_DESCRIPTION { get; set; }
        public string REGION_DESCRIPTION_AR { get; set; }
        public string DISTRICT_ID { get; set; }
        public string DISTRICT_DESCRIPTION { get; set; }
        public string DISTRICT_DESCRIPTION_AR { get; set; }
        public string EMAIL_ADDRESS1 { get; set; }
        public string PHONE_NUMBER1 { get; set; }
        public string PHONE_NUMBER2 { get; set; }
        public string FAX { get; set; }
        public string WEB_SITE { get; set; }
        public string NETWORK_ID { get; set; }
        public string NETWORK_DESCRIPTION { get; set; }
        public string NETWORK_DESCRIPTION_AR { get; set; }
        public decimal LONGITUDE { get; set; }
        public decimal LATITUDE { get; set; }
        public string AFFLIATED_PROVIDER_ID { get; set; }
        public string AFFLIATED_PROVIDER_DESC { get; set; }
        public string AFFLIATED_PROVIDER_DESC_AR { get; set; }
        public string LOCATION_NAME { get; set; }
        public string LOCATION_NAME_AR { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            HOSPTIALID = dr.GetString("HOSPTIALID");
            PROVIDER_ID = dr.GetString("PROVIDER_ID");
            PROVIDER_NAME = dr.GetString("PROVIDER_NAME_EN");
            PROVIDER_NAME_AR = dr.GetString("PROVIDER_NAME_AR");
            //PROVIDER_NAME = dr.GetString("LOCATION_NAME_EN");
            //PROVIDER_NAME_AR = dr.GetString("LOCATION_NAME_AR");
            PROVIDER_TYPE_ID = dr.GetString("PROVIDER_TYPE_ID");
            PROVIDER_TYPE_DESCRIPTION = dr.GetString("PROVIDER_TYPE_DESCIPTION_EN");
            PROVIDER_TYPE_DESCRIPTION_AR = dr.GetString("PROVIDER_TYPE_DESCIPTION_AR");
            SPECIALITY_ID = dr.GetString("SPECIALITY_ID");
            SPECIALITY_DESCRIPTION = dr.GetString("SPECIALITY_DESCRIPTION");
            SPECIALITY_DESCRIPTION_AR = dr.GetString("SPECIALITY_DESCRIPTION_AR");
            SEX_ID = dr.GetString("SEX_ID");
            SEX_DESCRIPTION = dr.GetString("SEX_DESCRIPTION");
            SEX_DESCRIPTION_AR = dr.GetString("SEX_DESCRIPTION_AR");
            LOCATION_TYPE_ID = dr.GetString("LOCATION_TYPE_ID");
            LOCATION_TYPE_DESCRIPTION = dr.GetString("LOCATION_TYPE_DESCRIPTION");
            LOCATION_NAME = dr.GetString("LOCATION_NAME_EN");
            LOCATION_NAME_AR = dr.GetString("LOCATION_NAME_AR");
            ADDRESS = dr.GetString("ADDRESS");
            ADDRESS_AR = dr.GetString("ADDRESS_AR");
            REGION_ID = dr.GetString("REGION_ID");
            REGION_DESCRIPTION = dr.GetString("REGION_DESCRIPTION_EN");
            REGION_DESCRIPTION_AR = dr.GetString("REGION_DESCRIPTION_AR");
            DISTRICT_DESCRIPTION = dr.GetString("DISTRICT_DESCRIPTION_EN");
            DISTRICT_DESCRIPTION_AR = dr.GetString("DISTRICT_DESCRIPTION_AR");
            DISTRICT_ID = dr.GetString("DISTRICT_ID");
            EMAIL_ADDRESS1 = dr.GetString("EMAIL_ADDRESS1");
            PHONE_NUMBER1 = dr.GetString("PHONE_NUMBER1");
            PHONE_NUMBER2 = dr.GetString("PHONE_NUMBER2");
            FAX = dr.GetString("FAX");
            WEB_SITE = dr.GetString("WEB_SITE");
            NETWORK_ID = dr.GetString("NETWORK_ID");
            NETWORK_DESCRIPTION = dr.GetString("NETWORK_DESCRIPTION");
            NETWORK_DESCRIPTION_AR = dr.GetString("NETWORK_DESCRIPTION_AR");
            LONGITUDE = dr.GetDecimal("LONGTITUDE");
            LATITUDE = dr.GetDecimal("LATITUDE");
            AFFLIATED_PROVIDER_ID = dr.GetString("AFFLIATED_PROVIDER_ID");
            AFFLIATED_PROVIDER_DESC = dr.GetString("AFFLIATED_PROVIDER_DESC");
            AFFLIATED_PROVIDER_DESC_AR = dr.GetString("AFFLIATED_PROVIDER_DESC_AR");
            MAIN_PROVIDER_NAME = dr.GetString("MAIN_PROVIDER_NAME_EN");
            MAIN_PROVIDER_NAME_AR = dr.GetString("MAIN_PROVIDER_NAME_AR");
        }

    }

    public class SearchResultModel
    {

        public SearchModel Search { get; set; }

        public IList<SearchResult> Result { get; set; }
    }

    public class BenefitsMappingList : IDataMapper
    {
        public int PlanNumber { get; set; }
        public string PlanDescEN { get; set; }
        public string PlanDescAR { get; set; }

        public void MapProperties(DbDataReader dr)
        {

            PlanNumber = dr.GetInt32("PLAN_NUMBER");
            PlanDescEN = dr.GetString("PLAN_DESC_ENG");
            PlanDescAR = dr.GetString("PLAN_DESC_AR");
        }
    }


    public class GlobalConfigMapping : IDataMapper
    {
        public int ACTIVE_AFYA_POLICY_NO { get; set; }

        public string OFFER_TERMS_CONDITION_AR { get; set; }

        public string OFFER_TERMS_CONDITION_EN { get; set; }

        public void MapProperties(DbDataReader dr)
        {

            ACTIVE_AFYA_POLICY_NO = dr.GetInt32("ACTIVE_AFYA_POLICY_NO");
            OFFER_TERMS_CONDITION_AR = dr.GetString("OFFER_TERMS_CONDITION_AR");
            OFFER_TERMS_CONDITION_EN = dr.GetString("OFFER_TERMS_CONDITION_EN");

        }
    }

    public class AddsMapping : IDataMapper
    {
        public int AD_ID { get; set; }

        public bool isActiveAfyaPolicyFound { get; set; }

        public bool IsAddApplicable { get; set; }
        public string AD_IMG_URL { get; set; }
        public DateTime EXPIRY_DATE {get; set;}

        public string ADD_EXPIRE_DATE { get; set; }
        public string AD_DETAIL_TEXT_AR { get; set; }
        public string AD_DETAIL_TEXT_EN { get; set; }
        public string isDiscountOptionApplicable { get; set; }

        public string isClaimRegisterSmsVisibility {get; set;}
        public string isSmsClaimRegisterApplicable { get; set; }

        public void MapProperties(DbDataReader dr)
        {

            AD_ID = dr.GetInt32("AD_ID");
            AD_IMG_URL = dr.GetString("AD_IMG_URL");
            AD_DETAIL_TEXT_AR = dr.GetString("AD_DETAIL_TEXT_AR");
            AD_DETAIL_TEXT_EN = dr.GetString("AD_DETAIL_TEXT_EN");
            EXPIRY_DATE = dr.GetDateTime("EXPIRY_DATE");

        }

    }

    public class SmsVisibility
    {
        public string isClaimRegisterSmsVisibility { get; set; }

        public string isSmsClaimRegisterApplicable { get; set; }
    }


    public class AppUpdate
    {
        public string EnforceUpdate { get; set; }

        public string MessageAR { get; set; }

        public string MessageENG { get; set; }
    }

    public class DiscountCouponMapping : IDataMapper
    {
        public long  CIVIL_ID { get; set; }
        public string CARD_PRINT_DATE { get; set; }
        public string COUPN_EXPIRY_DATE { get; set; }
        public string COUPN_STATUS { get; set; }
        public string REDEEMED_DATE { get; set; }
        public string REDEEMED_POLICY_TYPE { get; set; }

        public string REDEEMED_POLICY_NO { get; set; }
        public bool IS_DISCOUNT_OFFER_APPLICABLE { get; set; }
        public string CARD_PRINT_MESSAGE { get; set; }
        public string OFFER_TERMS_AND_CONDITION_EN { get; set; }
        public string OFFER_TERMS_AND_CONDITION_AR { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            CIVIL_ID           = dr.GetInt64("CIVIL_ID");
            CARD_PRINT_DATE    = dr.GetString("CARD_PRINT_DATE");
            COUPN_EXPIRY_DATE   = dr.GetString("COUPN_EXPIRY_DATE");
            REDEEMED_DATE       = dr.GetString("REDEEMED_DATE");
            COUPN_STATUS         = dr.GetString("COUPN_STATUS");
            REDEEMED_POLICY_TYPE  = dr.GetString("REDEEMED_POLICY_TYPE");
            REDEEMED_POLICY_NO    = dr.GetString("REDEEMED_POLICY_NO");
        }
    }

    public class AppVersion : IDataMapper
    {
        public string  IOS_CURRENT_VERSION { get; set; }
        public string  ANDROID_CURRENT_VERSION { get; set; }
        public int  ENFORCE_UPDATE { get; set; }
        public string  MESSAGE_ENG { get; set; }
        public string  MESSAGE_AR { get; set; }
        public DateTime  UPDATED_ON  { get; set; }

    public void MapProperties(DbDataReader dr)
        {

            IOS_CURRENT_VERSION = dr.GetString("IOS_CURRENT_VERSION");
            ANDROID_CURRENT_VERSION = dr.GetString("ANDROID_CURRENT_VERSION");
            ENFORCE_UPDATE = dr.GetInt32("ENFORCE_UPDATE");
            MESSAGE_ENG = dr.GetString("MESSAGE_ENG");
            MESSAGE_AR = dr.GetString("MESSAGE_AR");
            UPDATED_ON = dr.GetDateTime("UPDATED_ON");

        }

    }
}
