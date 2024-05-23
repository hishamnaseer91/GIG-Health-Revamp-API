using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class RequestModels
    {
    }

    public class GetProfileModel
    {
        public string CivilId { get; set; }

        public string policyNumber { get; set; }

    }

    public class ProfileAddressModel
    {
        public string CivilId { get; set; } // use to get mmeber information
        public string REGION_ID { get; set; }
        public string DISTRICT_ID { get; set; }
        public string BlockNo { get; set; }
        public string StreetNo { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNo { get; set; }
        public string FlatNo { get; set; }
        public string MobileNumber { get; set; }

        public string policyNumber { get; set; }
    }

    public class GetAreaModel
    {

        public string RegionId { get; set; }
    }
    public class GetClaimRequestModel
    {
        public string CivilId { get; set; }
        public string AuthorizeStatus { get; set; }
        public string Providerid { get; set; }
        public string IncidentType { get; set; }
        public string FromDate2 { get; set; }
        public string ToDate2 { get; set; }

        public string Culture { get; set; }
        public string PolicyNo { get; set; }

        public string MemNo { get; set; }
        //public string Agentname { get; set; }
        //public string ICD_10_Code { get; set; }
    }

    public class AuthorizeServicesModel
    {
        public string RequestId { get; set; }
    }

    public class ProcedureloadModel
    {
        public string RequestId { get; set; }
    }

    public class ClaimStausModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class IncidentType
    {
        public int Id { get; set; }
        public string Description { get; set; }

    }

    public class AddComplaintModel
    {
        public int ComplaintId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        // public string IncidentNo { get; set; }
        public string CategoryName { get; set; }
        //  public string UserId { get; set; }
        public string CivilId { get; set; }
        public DateTime CreationDate { get; set; }

        public string policyNumber { get; set; }

    }

    public class RegisterMemberModel
    {
        public string CivilId { get; set; }
        public string MedicalInsuranceCardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }


    public class MemberInfo : IDataMapper
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            Name = dr.GetString("Name");
            Email = dr.GetString("EMAIL_ADDRESS1");
        }
    }
    public class RegisterMemberCompleteModel
    {
        public string UID { get; set; }
        //  public string MemberId { get; set; }
        public string MedicalInsuranceCard { get; set; }
        public string CivilId { get; set; }
        public string Password { get; set; }

        public string policyNumber { get; set; }
    }
    public class AddMemberUserModel : IDataMapper
    {
        public string CivilId { get; set; }
        public string MedicalInsuranceCardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }

        //
        public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        //public string LastName { get; set; }
        //public string FullName { get { return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName); } 

        public string FullName { get; set; }

        public string PolicyNo { get; set; }
        public string Network_Id { get; set; }
        public string Member_Id { get; set; }
        public string Mobile { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Field is required.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ReqPassword { get; set; }

        public bool IsFirstLogin { get; set; }
        public bool IsActive { get; set; }
        public bool IsRegistrationComplete { get; set; }
        //  [Required(ErrorMessage = "Field is required.")]
        public string DisableRemarks { get; set; }
        public DateTime DisableDate { get; set; }
        public string DefaultLanguage { get; set; }
        public string PrimaryEmail { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public string PrincipalMember { get; set; }
        public string RelationshipDescription { get; set; }


        public void MapProperties(DbDataReader dr)
        {
            FullName = dr.GetString("NAME");
            PolicyNo = dr.GetInt32("POLICY_NUMBER").ToString();
            Network_Id = dr.GetString("NETWORK_ID");
            PrincipalMember = dr.GetString("Principal_Flag");
            RelationshipDescription= dr.GetString("RELATION_DESCRIPTION");
        }
    }

    public class LocationFinderAttributes
    {
        public string Culture { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string RegionID { get; set; }
        public string AreaID { get; set; }
        public string LocationTypeID { get; set; }

        public IList<LocationInfo> list { get; set; }

        public LocationFinderAttributes()
        {
            list = new List<LocationInfo>();
        }

    }

    public class ChangePasswordModel
    {
        public string CivilId { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public bool IsOldPasswordRequired { get; set; }
    }
    public class DeleteMemberModel
    {
        public string CivilId { get; set; }
        public string PolicyNumber { get; set; }
        public string MemberNumber { get; set; }
        public string DeletionComments { get; set; }
    }
    public class ResetPasswordModel
    {
        public string CivilId { get; set; }
        public string MedicalInsuranceCardNo { get; set; }
    }
    public class LimitCheckModel
    {
        public string CivilId { get; set; }

    }

    public class NotificationModel {
        public string CivilId { get; set; }
    }

    public class GetPersonalInformationModel
    {

        public string PolicyNumber { get; set; }
        public string MemberNumber { get; set; }
    }
    public class DDLRegionArea : IDataMapper
    {
        public string RegionAreaName { get; set; }
        public string RegionAreaID { get; set; }
        public void MapProperties(DbDataReader dr)
        {

            RegionAreaID = dr.GetString("Value");
            RegionAreaName = dr.GetString("Name");
        }
    }
    public class DDLRegion : IDataMapper
    {
        public string RegionName { get; set; }
        public string RegionID { get; set; }
        public void MapProperties(DbDataReader dr)
        {

            RegionID = dr.GetString("Value");
            RegionName = dr.GetString("Name");

        }


    }

    public class GetlimitsModel {

       public string CivilId { get; set; }

        public string PolicyNumber { get; set; }
    }

    public class CategoryListModel {
        public string Culture { get; set; }
    }


    public class CardModel
    {
        public string CivilID { get; set; }
        public string PhoneNumber { get; set; }          
        public string PolicyNumber { get; set; }
        public string MemberNumber { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Block { get; set; }
        public string StreetNo { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNo { get; set; }
        public string PreferredTime { get; set; }
        public string DeviceID { get; set; }
        public int isCardDileveryRequest { get; set; }
        public int isCardRePrintingRequest { get; set; }

        public string Subject { get; set; }

        public string EmailTo { get; set; }

        public string RequestType { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }

    public class BindCardModel : IDataMapper
    {
        public string Region { get; set; }
        public string Area { get; set; }
        public string Block { get; set; }
        public string StreetNo { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNo { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            Region = dr.GetString("REGION");
            Area = dr.GetString("AREA");
            Block = dr.GetString("BLOCK");
            StreetNo = dr.GetString("STREET_NO");
            BuildingNo = dr.GetString("BUILDING_NO");
            FloorNo = dr.GetString("FLOOR_NO");
        }
    }

    public class GetCardModel
    {
        public string CivilId { get; set; }
        public string MemberNo { get; set; }
        public string PolicyNo { get; set; }
    }

}