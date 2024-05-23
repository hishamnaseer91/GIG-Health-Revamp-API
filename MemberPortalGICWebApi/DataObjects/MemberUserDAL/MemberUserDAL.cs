using Dapper;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace MemberPortalGICWebApi.DataObjects.MemberUserDAL
{
    public class MemberUserDAL : DBGenerics, IMemberUser
    {
        private readonly string _connectionString;
        public MemberUserDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Default"].ToString(); ;
        }
        public UserProfileModel GetMemberProfile(string civilId, string pol)
        {
            DBGenerics db = new DBGenerics();
            var Query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E
                  WHERE 1 = 1


                     --   AND E.POLICY_NUMBER = MEMBER_USERS.POLICY_NO
               -- AND E.MEMBER_NUMBER = MEMBER_USERS.MEMBER_ID
                and E.NATIONAL_IDENTITY =:id and E.POLICY_NUMBER = :pol
                    ),
     MEMBERS_HISTORY AS (SELECT I.*
                           FROM(SELECT J.*,
                                        MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER, J.EVENT_NUMBER) AS MAX_DATE,
                                        MAX(J.EVENT_NUMBER)                OVER(PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER) AS MAX_EVENT
                                   FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                  WHERE 1 = 1
                                --and J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                    AND(J.POLICY_NUMBER, J.MEMBER_NUMBER) IN(SELECT G.POLICY_NUMBER, MEMBER_NUMBER FROM MEMBERS G)
                          ) I
                  WHERE I.MODIFICATION_EFFECTIVE_DATE = MAX_DATE
                    AND I.EVENT_NUMBER = MAX_EVENT
                    --AND NVL(I.EXPIRY_DATE, I.DELETION_DATE) >= sysdate
),
     POLICIES AS (SELECT C.*
                    FROM(SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY B.POLICY_NUMBER, B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER(PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE 1 = 1
                            -- and B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                             AND B.POLICY_NUMBER IN(SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER = C.MAX_EVENT)                     
SELECT  RPLNETWORK.NETWORK_DESCRIPTION,
MEMBERS.MEMBER_NUMBER,
         MEMBERS.FIRST_NAME || '  ' ||
       MEMBERS.MIDDLE_NAME || '  ' ||
       MEMBERS.LAST_NAME NAME,
        MEMBERS.INTERNATIONAL_FIRST_NAME || '  ' ||
       MEMBERS.INTERNATIONAL_MIDDLE_NAME || '  ' ||
       MEMBERS.INTERNATIONAL_LAST_NAME INTERNATIONAL_NAME,
        (select Value
          from mednext.RPLMEMBERCUSTOMFIELD CUST
         where CUST.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
           and CUST.MEMBER_NUMBER = MEMBERS.MEMBER_NUMBER
           and CUST.FIELD_ID = 'VIP'
          and CUST.VALUE = 'Y') VIP_FLAG,
       MEMBERS_HISTORY.MEMBER_EFFECTIVE_DATE,
       MEMBERS_HISTORY.EXPIRY_DATE MEMBEREXPIRY,
       MEMBERS_HISTORY.DELETION_DATE   ,
       MEMBERS.NATIONAL_IDENTITY,
       POLICIES.POLICY_HOLDER,
       POLICIES.POLICY_NUMBER,
       POLICIES.POLICY_EFFECTIVE_DATE,
       POLICIES.EXPIRY_DATE POLICYEXPIRY,
       POLICIES.CANCELLATION_DATE POLICY_CANCELLATION_DATE,
       RPLPACKAGE.INSURANCE_COMPANY_NUMBER,
       RPLPACKAGE.DEFAULT_NETWORK_ID,
       RPLMEMBERADDRESS.PHONE_NUMBER1,
       RPLMEMBERADDRESS.ADDRESS_TYPE_ID,
       MEMBERS.CLASS_ID,
       MEMBERS.RELATION_ID,
       MEMBERS.EFFECTIVE_DATE,
       MEMBERS.SEX_ID GENDER_ID,
       MEMBERS.DATE_OF_BIRTH, 
       MEMBERS.PRINCIPAL_FLAG PRINCIPALINDICATOR,
MEMBERS.RELATION_DESCRIPTION,
   RPLMEMBERADDRESS.ADDRESS,
       RPLMEMBERADDRESS.REGION_ID,
       RPLMEMBERADDRESS.DISTRICT_ID
  FROM MEMBERS,
       MEMBERS_HISTORY,
       POLICIES,
       mednext.RPLPACKAGE,
       mednext.RPLNETWORK,
       mednext.RPLMEMBERADDRESS

WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
  AND MEMBERS.POLICY_NUMBER = MEMBERS_HISTORY.POLICY_NUMBER
  AND MEMBERS.MEMBER_NUMBER = MEMBERS_HISTORY.MEMBER_NUMBER
  AND MEMBERS.MEMBER_NUMBER = RPLMEMBERADDRESS.MEMBER_NUMBER(+)
  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER = RPLPACKAGE.INSURANCE_COMPANY_NUMBER
  AND MEMBERS_HISTORY.PACKAGE_NUMBER = RPLPACKAGE.PACKAGE_NUMBER
  AND RPLNETWORK.NETWORK_ID = RPLPACKAGE.DEFAULT_NETWORK_ID";
            return ExecuteSingle<UserProfileModel>(Query, ParamBuilder.Par(":id", civilId), ParamBuilder.Par(":pol", pol));
        }


        //        public MemberUser GetMemberProfile(string civilId)
        //        {
        //            DBGenerics db = new DBGenerics();

        //            var Query = @"select A.*,B.DATE_OF_BIRTH,B.EFFECTIVE_DATE, B.EXPIRY_DATE , C.POLICY_HOLDER   from  MEMBER_ADDRESS_HISTORY A, mednext.rplmember B, mednext.rplpolicy C 
        //where 
        //A.POL_ID=B.POLICY_NUMBER and A.NATIONAL_IDENTITY=B.NATIONAL_IDENTITY
        //and B.POLICY_NUMBER=C.POLICY_NUMBER 
        //and  A.National_IDENTITY = :CivelID
        //order by  HISTORY_ID desc";

        //            return ExecuteSingle<MemberUser>(Query, ParamBuilder.Par(":CivelID", civilId));

        //        }

        public IList<DDLRegion> DALRegionDDL()
        {
            DBGenerics Db = new DBGenerics();


            string Query = "SELECT REGION_ID as Value, REGION_DESCRIPTION as Name FROM mednext.RPLREGION";


            return Db.ExecuteList<DDLRegion>(Query);
        }

        public IList<DDLRegionArea> DALAreaDDL(int Region)
        {
            DBGenerics Db = new DBGenerics();
            if (Region.Equals(0))
            {
                string Query = "SELECT DISTRICT_ID as Value, LOCAL_DESCRIPTION as Name FROM MEDNEXT.DISTRICT";

                return Db.ExecuteList<DDLRegionArea>(Query);
            }
            else
            {
                string Query = "SELECT DISTRICT_ID as Value, LOCAL_DESCRIPTION as Name FROM MEDNEXT.DISTRICT WHERE PARENT_ID=:parent_id";

                return Db.ExecuteList<DDLRegionArea>(Query, ParamBuilder.Par(":parent_id", Region));

            }


        }

        public IList<MemberUser> GetMemberAssuredName(string civilId)
        {
            DBGenerics db = new DBGenerics();
            var Query = @"WITH MEMBERS AS (SELECT E.*
                           FROM mednext.RPLMEMBER E
                          WHERE 1=1
                          -- AND E.POLICY_NUMBER      = :policynum4
                        --  AND E.MEMBER_NUMBER      = :membernum7
                            AND NATIONAL_IDENTITY= :CivelID
                            ),
             MEMBERS_HISTORY AS (SELECT I.*
                                   FROM (SELECT J.*,
                                                MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER,J.EVENT_NUMBER) AS MAX_DATE,
                                                MAX(J.EVENT_NUMBER)                OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER) AS MAX_EVENT
                                           FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                          WHERE J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                            AND (J.POLICY_NUMBER,J.MEMBER_NUMBER) IN (SELECT G.POLICY_NUMBER,MEMBER_NUMBER FROM MEMBERS G)
                                  ) I
                          WHERE I.MODIFICATION_EFFECTIVE_DATE       = MAX_DATE
                            AND I.EVENT_NUMBER                      = MAX_EVENT
                            AND NVL(I.EXPIRY_DATE,I.DELETION_DATE) >= sysdate),
             POLICIES AS (SELECT C.*
                            FROM (SELECT B.*,
                                         MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER,B.EVENT_NUMBER) AS MAX_DATE,
                                         MAX(B.EVENT_NUMBER)                OVER (PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                                    FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                                   WHERE B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                     AND B.POLICY_NUMBER IN (SELECT POLICY_NUMBER FROM MEMBERS)) C
                           WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                             AND C.EVENT_NUMBER                = C.MAX_EVENT)                     
        SELECT MEMBERS.MEMBER_NUMBER,
                 MEMBERS.FIRST_NAME || '  '||
               MEMBERS.MIDDLE_NAME|| '  '||
               MEMBERS.LAST_NAME NAME,
                (select Value

                  from mednext.RPLMEMBERCUSTOMFIELD CUST 
                 where CUST.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
                   and CUST.MEMBER_NUMBER = MEMBERS.MEMBER_NUMBER
                   and CUST.FIELD_ID      = 'VIP'
                  and CUST.VALUE         = 'Y') VIP_FLAG,
               MEMBERS_HISTORY.MEMBER_EFFECTIVE_DATE,
               MEMBERS_HISTORY.EXPIRY_DATE     MEMBEREXPIRY,
               MEMBERS_HISTORY.DELETION_DATE   ,
               MEMBERS.NATIONAL_IDENTITY,
               POLICIES.POLICY_HOLDER,
               POLICIES.POLICY_NUMBER,
               POLICIES.POLICY_EFFECTIVE_DATE,
               POLICIES.EXPIRY_DATE        POLICYEXPIRY,
               POLICIES.CANCELLATION_DATE  POLICY_CANCELLATION_DATE,
               RPLPACKAGE.INSURANCE_COMPANY_NUMBER,
               RPLPACKAGE.DEFAULT_NETWORK_ID,
              -- RPLPROVIDERNETWORK.NETWORK_ID,
               RPLMEMBERADDRESS.PHONE_NUMBER1,
               RPLMEMBERADDRESS.ADDRESS_TYPE_ID,
               MEMBERS.CLASS_ID,
               MEMBERS.RELATION_ID,
               MEMBERS.EFFECTIVE_DATE,
               MEMBERS.SEX_ID    GENDER_ID,
               MEMBERS.DATE_OF_BIRTH, 
               MEMBERS.PRINCIPAL_FLAG PRINCIPALINDICATOR
          FROM MEMBERS,
               MEMBERS_HISTORY,
               POLICIES,
               mednext.RPLPACKAGE,
             --  mednext.RPLPROVIDERNETWORK,
               mednext.RPLMEMBERADDRESS
        WHERE MEMBERS.POLICY_NUMBER                     = POLICIES.POLICY_NUMBER
          AND MEMBERS.POLICY_NUMBER                     = MEMBERS_HISTORY.POLICY_NUMBER
          AND MEMBERS.MEMBER_NUMBER                     = MEMBERS_HISTORY.MEMBER_NUMBER
          AND MEMBERS.MEMBER_NUMBER                     = RPLMEMBERADDRESS.MEMBER_NUMBER(+)
          AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER  = RPLPACKAGE.INSURANCE_COMPANY_NUMBER
          AND MEMBERS_HISTORY.PACKAGE_NUMBER            = RPLPACKAGE.PACKAGE_NUMBER
         -- AND RPLPROVIDERNETWORK.NETWORK_ID             = RPLPACKAGE.DEFAULT_NETWORK_ID
         -- AND RPLPROVIDERNETWORK.PROVIDER_ID            = :ProviderIdCurrent
         -- AND START_DATE                               <= sysdate";
            // string Query = "select Count(*) from GMONLINE.MEMBER_USERS a where A.CIVIL_ID =:civilid and A.PASSWORD=:password";
            return ExecuteList<MemberUser>(Query, ParamBuilder.Par(":CivelID", civilId));

        }

        public MemberUsers GetmemberUser(string civilID)
        {
            DBGenerics db = new DBGenerics();
            #region Query
            string query = @"select * from member_users where civil_id = :civil_id";
            #endregion
            return db.ExecuteSingle<MemberUsers>(query, ParamBuilder.Par(":civil_id", civilID));
        }
        public int ChangePassword(ChangePasswordModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = "Update member_users SET Password='" + model.Password + "' , ISFIRSTPASWORDCHANGED = '1', UPDATED_DATE = sysdate where civil_id =:civilId RETURNING Id INTO :my_id_param";
            var result = db.ExecuteNonQueryOracle(query, ":my_id_param", ParamBuilder.Par(":civilId", model.CivilId), ParamBuilder.OutPar(":my_id_param", 0));
            return result;
        }
        public long isCorrectOldPassword(ChangePasswordModel Model)
        {

            DBGenerics db = new DBGenerics();
            string query = "select ID from member_users where CIVIL_ID=:civilid and Password = :password";
            return db.ExecuteScalarInt64(query, ParamBuilder.Par(":civilid", Model.CivilId), ParamBuilder.Par(":password", Model.OldPassword));
        }
        public int IsUSerCanUpdateAddress()
        {
            DBGenerics db = new DBGenerics();
            string query = "select A.ISADDRESSMOBILEUPDATE from GMONLINE.MEMBERPORTALSETTING a order by 1 asc";
            return db.ExecuteScalarInt32(query);
        }

        public long GetMemberId(string civilId, string polNo)
        {
            DBGenerics db = new DBGenerics();
            string query = "select A.MEMBER_NUMBER  from MEDNEXT.RPLMEMBER a where A.NATIONAL_IDENTITY = :civilId and A.POLICY_NUMBER = :polNo";
            return db.ExecuteScalarInt64(query, ParamBuilder.Par(":civilid", civilId), ParamBuilder.Par(":polNo", polNo));
        }

        public UserMemberUpdateModel GetUserDetailModel(string civilId, string polNo)
        {
            DBGenerics db = new DBGenerics();
            #region Query
            string query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E
                  WHERE 1 = 1


                     --   AND E.POLICY_NUMBER = MEMBER_USERS.POLICY_NO
               -- AND E.MEMBER_NUMBER = MEMBER_USERS.MEMBER_ID
                and E.NATIONAL_IDENTITY =:id and E.POLICY_NUMBER = :pol
                    ),
     MEMBERS_HISTORY AS (SELECT I.*
                           FROM(SELECT J.*,
                                        MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER, J.EVENT_NUMBER) AS MAX_DATE,
                                        MAX(J.EVENT_NUMBER)                OVER(PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER) AS MAX_EVENT
                                   FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                  WHERE 1 = 1
                                --and J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                    AND(J.POLICY_NUMBER, J.MEMBER_NUMBER) IN(SELECT G.POLICY_NUMBER, MEMBER_NUMBER FROM MEMBERS G)
                          ) I
                  WHERE I.MODIFICATION_EFFECTIVE_DATE = MAX_DATE
                    AND I.EVENT_NUMBER = MAX_EVENT
                    --AND NVL(I.EXPIRY_DATE, I.DELETION_DATE) >= sysdate
),
     POLICIES AS (SELECT C.*
                    FROM(SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY B.POLICY_NUMBER, B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER(PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE 1 = 1
                            -- and B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                             AND B.POLICY_NUMBER IN(SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER = C.MAX_EVENT)                     
SELECT  RPLNETWORK.NETWORK_DESCRIPTION,
MEMBERS.MEMBER_NUMBER,
         MEMBERS.FIRST_NAME || '  ' ||
       MEMBERS.MIDDLE_NAME || '  ' ||
       MEMBERS.LAST_NAME NAME,
        MEMBERS.INTERNATIONAL_FIRST_NAME || '  ' ||
       MEMBERS.INTERNATIONAL_MIDDLE_NAME || '  ' ||
       MEMBERS.INTERNATIONAL_LAST_NAME INTERNATIONAL_NAME,
        (select Value
          from mednext.RPLMEMBERCUSTOMFIELD CUST
         where CUST.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
           and CUST.MEMBER_NUMBER = MEMBERS.MEMBER_NUMBER
           and CUST.FIELD_ID = 'VIP'
          and CUST.VALUE = 'Y') VIP_FLAG,
       MEMBERS_HISTORY.MEMBER_EFFECTIVE_DATE,
       MEMBERS_HISTORY.EXPIRY_DATE MEMBEREXPIRY,
       MEMBERS_HISTORY.DELETION_DATE   ,
       MEMBERS.NATIONAL_IDENTITY,
       POLICIES.POLICY_HOLDER,
       POLICIES.POLICY_NUMBER,
       POLICIES.POLICY_EFFECTIVE_DATE,
       POLICIES.EXPIRY_DATE POLICYEXPIRY,
       POLICIES.CANCELLATION_DATE POLICY_CANCELLATION_DATE,
       RPLPACKAGE.INSURANCE_COMPANY_NUMBER,
       RPLPACKAGE.DEFAULT_NETWORK_ID,
       RPLMEMBERADDRESS.PHONE_NUMBER1,
       RPLMEMBERADDRESS.ADDRESS_TYPE_ID,
       MEMBERS.CLASS_ID,
       MEMBERS.RELATION_ID,
       MEMBERS.EFFECTIVE_DATE,
       MEMBERS.SEX_ID GENDER_ID,
       MEMBERS.DATE_OF_BIRTH, 
       MEMBERS.PRINCIPAL_FLAG PRINCIPALINDICATOR,
   RPLMEMBERADDRESS.ADDRESS,
       RPLMEMBERADDRESS.REGION_ID,
       RPLMEMBERADDRESS.DISTRICT_ID
  FROM MEMBERS,
       MEMBERS_HISTORY,
       POLICIES,
       mednext.RPLPACKAGE,
       mednext.RPLNETWORK,
       mednext.RPLMEMBERADDRESS

WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
  AND MEMBERS.POLICY_NUMBER = MEMBERS_HISTORY.POLICY_NUMBER
  AND MEMBERS.MEMBER_NUMBER = MEMBERS_HISTORY.MEMBER_NUMBER
  AND MEMBERS.MEMBER_NUMBER = RPLMEMBERADDRESS.MEMBER_NUMBER(+)
  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER = RPLPACKAGE.INSURANCE_COMPANY_NUMBER
  AND MEMBERS_HISTORY.PACKAGE_NUMBER = RPLPACKAGE.PACKAGE_NUMBER
  AND RPLNETWORK.NETWORK_ID = RPLPACKAGE.DEFAULT_NETWORK_ID";
            #endregion
            return db.ExecuteSingle<UserMemberUpdateModel>(query, ParamBuilder.Par(":id", civilId), ParamBuilder.Par(":pol", polNo));
        }

        public int UpdateAddressLocalHistory(UserMemberUpdateModel Model, string UserId)
        {

            DBGenerics db = new DBGenerics();
            string query = @"insert into MEMBER_ADDRESS_HISTORY
                                   (NATIONAL_IDENTITY,MEM_ID,POL_ID, MEMBER_NAME , MEMBER_NAME_AR,REGION_ID ,DISTRICT_ID,BLOCK_NO ,STREET_NO,BUILDING_NO,FLOOR_NO,FLAT_NO,
                                    IS_ADDRESS_UPDATED,ADDRESS_UPDATE_DDATE,UPDATED_BY)
                                values
                                    (:CivilId,:MEM_ID,:POL_ID,:MEMBER_NAME,:MEMBER_NAME_AR,:REGION_ID,:DISTRICT_ID,:BLOCK_NO,:STREET_NO,:BUILDING_NO,:FLOOR_NO,:FLAT_NO,
                                      :IS_ADDRESS_UPDATED,sysdate,:UPDATED_BY)
                               ";
            return db.ExecuteNonQuery(query, ParamBuilder.Par(":CivilId", Model.NATIONAL_IDENTITY), ParamBuilder.Par(":MEM_ID", Model.MEMBER_NUMBER),
                ParamBuilder.Par(":POL_ID", Model.POLICY_NUMBER),
                    ParamBuilder.Par(":MEMBER_NAME", Model.Name),
                      ParamBuilder.Par(":MEMBER_NAME_AR", Model.ArabicName),
                        ParamBuilder.Par(":REGION_ID", Model.REGION_ID),
            ParamBuilder.Par(":DISTRICT_ID", Model.DISTRICT_ID),
                            ParamBuilder.Par(":BLOCK_NO", Model.BlockNo),
                                ParamBuilder.Par(":STREET_NO", Model.StreetNo),
                            ParamBuilder.Par(":BUILDING_NO", Model.BuildingNo),
                            ParamBuilder.Par(":FLOOR_NO", Model.FloorNo),
             ParamBuilder.Par(":FLAT_NO", Model.FlatNo),

                       ParamBuilder.Par(":IS_ADDRESS_UPDATED", 1),
                            ParamBuilder.Par(":UPDATED_BY", UserId)

                );

        }

        public MemberAddressExistence CheckMemberExistence(string MemberNumber)
        {

            DBGenerics db = new DBGenerics();
            string query = "select FAX, ADDRESS_NUMBER,ADDRESS_TYPE_ID,MEMBER_NUMBER from mednext.RPLMEMBERADDRESS where member_number=:member";
            return db.ExecuteSingle<MemberAddressExistence>(query, ParamBuilder.Par(":member", MemberNumber));
        }

        public IList<Notifications> GetAllNotifications(string civilID)
        {

            DBGenerics db = new DBGenerics();
            //  var Query = @"select NOTIFICATION_ID , NOTIFICATION_DATE , NOTIFICATION_SUBJECT,  NOTIFICATION_ATTACHMENT from Gic_notification where notification_type = '4'  ORDER BY NOTIFICATION_DATE DESC";
            string query = "SELECT * FROM (SELECT * FROM GIC_NOTIFICATION WHERE NOTIFICATION_ID > 0 ";

            query += "AND NOTIFICATION_TYPE IN(4) ";

            query += ") WHERE NOTIFICATION_ID NOT IN(SELECT GIC_NOTIFICATION_ID FROM GIC_NOTIFICATION_USER WHERE USER_ID = " + civilID + " ) ORDER BY NOTIFICATION_DATE DESC";


            return db.ExecuteList<Notifications>(query);
        }

        public bool IsFirstPasswordChange(string civilId)
        {
            bool isUpdate;
            DBGenerics db = new DBGenerics();
            var query = "select ID from member_users where CIVIL_ID = :civilId and ISFIRSTPASWORDCHANGED = 1";
            string result = db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":civilId", civilId));

            if (result == "-1")
            { isUpdate = false; }
            else
            { isUpdate = true; }

            return isUpdate;
        }


        public int DeleteMemberProfile(DeleteMemberModel model)
        {
            DBGenerics db = new DBGenerics();
            string query = "delete from member_users where CIVIL_ID = :CIVIL_ID and Member_Id = :Member_Id";
            return db.ExecuteNonQuery(query,
                ParamBuilder.Par(":CIVIL_ID", model.CivilId),
                ParamBuilder.Par(":Member_Id", model.MemberNumber));
        }

        public int InsertDeleteMemberLog(MemberUsers Model, string DELETION_REMARKS)
        {

            DBGenerics db = new DBGenerics();
            string query = @"insert into MEMBER_USERS_DELETED (
   CIVIL_ID                
  ,POLICY_NO                 
  ,MEMBER_ID                 
  ,MEDICAL_INSURANCE_CARD   
  ,MOBILE_NO                
  ,PRIMARY_EMAIL 
  ,CREATION_DATE
  ,DELETED_DATE          
  ,DELETED_BY              
  ,DELETION_REMARKS
  ,DEVICE_ID
  --,AFYA
  ,PRINCIPALMEMBER           
  ,RELATIONSHIP_DESCRIPTION)
   values(
  :CIVIL_ID,                
  :POLICY_NO,                
  :MEMBER_ID,                 
  :MEDICAL_INSURANCE_CARD,  
  :MOBILE_NO,                
  :PRIMARY_EMAIL,       
  :CREATION_DATE,
   SYSDATE,          
  :DELETED_BY,             
  :DELETION_REMARKS, 
  :DEVICE_ID,
  --:AFYA
  :PRINCIPALMEMBER,         
  :RELATIONSHIP_DESCRIPTION)";
            return db.ExecuteNonQuery(query,
            ParamBuilder.Par(":CIVIL_ID", Model.CIVIL_ID),
            ParamBuilder.Par(":POLICY_NO", Model.POLICY_NO),
            ParamBuilder.Par(":MEMBER_ID", Model.MEMBER_ID),
            ParamBuilder.Par(":MEDICAL_INSURANCE_CARD", Model.MEMBER_ID),
            ParamBuilder.Par(":MOBILE_NO", Model.MOBILE_NO),
            ParamBuilder.Par(":PRIMARY_EMAIL", "N/A"),
            ParamBuilder.Par(":CREATION_DATE", Model.CREATION_DATE.ToString()),
            ParamBuilder.Par(":DELETED_BY", Model.CIVIL_ID),
            ParamBuilder.Par(":DELETION_REMARKS", "MOBILE Device Request: " + DELETION_REMARKS),
            //ParamBuilder.Par(":NETWORK_ID", Model.net),
            ParamBuilder.Par(":DEVICE_ID", Model.DEVICE_ID),

            // ParamBuilder.Par(":AFYA", Model.AFYA == true ? "1" : "0"),
            ParamBuilder.Par(":PRINCIPALMEMBER", Model.PRINCIPALMEMBER),
            ParamBuilder.Par(":RELATIONSHIP_DESCRIPTION", Model.RELATIONSHIP_DESCRIPTION));
        }
        public MemberPolicyandMemberNoModel GetMemberPolicyMemberNumber(string civilId)
        {

            DBGenerics db = new DBGenerics();
            string query = "select POLICY_NO , MEMBER_ID from member_users where CIVIL_ID = :civilId";
            return db.ExecuteSingle<MemberPolicyandMemberNoModel>(query, ParamBuilder.Par(":civilId", civilId));
        }
        public ECardValue GetECardDetail(string civilId, string pol, string memNo)
        {
            DBGenerics db = new DBGenerics();
            ECardValue eCardValue = new ECardValue();
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    #region Query
                    var query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E
                  WHERE 1 = 1
                and E.NATIONAL_IDENTITY =:civilId and E.POLICY_NUMBER = :policyNo and E.MEMBER_NUMBER = :memberNo
                    ),
     POLICIES AS (SELECT C.*
                    FROM(SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY B.POLICY_NUMBER, B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER(PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE 1 = 1
                             AND B.POLICY_NUMBER IN(SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER = C.MAX_EVENT)                     
SELECT
       MEMBERS.FIRST_NAME || ' ' ||
       MEMBERS.MIDDLE_NAME || ' ' ||
       MEMBERS.LAST_NAME Mem_Name,
       API_POLICY_ECARD.MEMBER_NUMBER Mem_No,
       TO_CHAR(API_POLICY_ECARD.T_DATE, 'DD/MM/YYYY') ExpiryDate,
       TO_CHAR(API_POLICY_ECARD.F_DATE, 'DD/MM/YYYY') Enrollment,
       TO_CHAR(MEMBERS.DATE_OF_BIRTH, 'DD/MM/YYYY') Dob,
       MEMBERS.INTERNATIONAL_FIRST_NAME || ' ' ||
       MEMBERS.INTERNATIONAL_MIDDLE_NAME || ' ' ||
       MEMBERS.INTERNATIONAL_LAST_NAME Mem_Name_International,
       MEMBERS.NATIONAL_IDENTITY CidNo,
       MEDNEXT.RPLPOLICY.PRODUCT_DESCRIPTION ProductDescription,
       '' CardToShow,
       API_POLICY_ECARD.POLICY_NUMBER PolicyNo,
       API_POLICY_ECARD.POL_HOLDER PolicyHolder,   
       CASE WHEN API_POLICY_ECARD.DEDUCTABLE IS NULL THEN '' ELSE API_POLICY_ECARD.DEDUCTABLE END AS Deductable,
       CASE WHEN API_POLICY_ECARD.EXCLUSION IS NULL THEN '' ELSE API_POLICY_ECARD.EXCLUSION END AS Exclusion,
       API_POLICY_ECARD.BRODUCT_CODE ProductCode,
       API_POLICY_ECARD.PLAN_DESC Plan,
       CASE WHEN API_POLICY_ECARD.COPAY IS NULL THEN '' ELSE API_POLICY_ECARD.COPAY END Co_Pay,
       API_POLICY_ECARD.COPAY_IN CoPayIn,
       API_POLICY_ECARD.COPAY_OUT CoPayOut,
       --API_POLICY_ECARD.COPAY_DT CoPayDT,
       API_POLICY_ECARD.DENTAL CoPayDT,
       CASE WHEN API_POLICY_ECARD.MATERNITY_LIMIT IS NULL OR API_POLICY_ECARD.MATERNITY_LIMIT = '' OR API_POLICY_ECARD.MATERNITY_LIMIT = '0' THEN 'No' ELSE 'Yes' END Maternity,
       API_POLICY_ECARD.MATERNITY_LIMIT MaternityLimit,
       API_POLICY_ECARD.CHRONIC_MED_WAITING_P Chronic_Waiting_P,
       API_POLICY_ECARD.WELLNESS_WAITING_P Wellness_Waiting_P,
       API_POLICY_ECARD.MATERNITY_IN_WAITING_P MaternityIn_Waiting_P,
       API_POLICY_ECARD.MATERNITY_OUT_WAITING_P MaternityOut_Waiting_P,
       API_POLICY_ECARD.DENTAL_WAITING_P Dental_Waiting_P,
       API_POLICY_ECARD.WAITING_P Waiting_P,
       CASE WHEN API_POLICY_ECARD.EXT_REF_NUM IS NULL THEN '' ELSE  API_POLICY_ECARD.EXT_REF_NUM END CardNo,
       CASE WHEN API_POLICY_ECARD.PKGNUM IS NULL THEN 0 ELSE  API_POLICY_ECARD.PKGNUM END PackageNumber
  FROM MEMBERS,
       --MEMBERS_HISTORY,
       POLICIES,
       --mednext.RPLPACKAGE,
       --mednext.RPLNETWORK,
       --mednext.RPLMEMBERADDRESS,
       MEDNEXT.RPLPOLICY,
       API_POLICY_ECARD
       
WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER AND 
  API_POLICY_ECARD.CIVIL_ID = MEMBERS.NATIONAL_IDENTITY AND API_POLICY_ECARD.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
  AND MEDNEXT.RPLPOLICY.POLICY_NUMBER = POLICIES.POLICY_NUMBER
  --AND MEMBERS.POLICY_NUMBER = MEMBERS_HISTORY.POLICY_NUMBER
  --AND MEMBERS.MEMBER_NUMBER = MEMBERS_HISTORY.MEMBER_NUMBER
  --AND MEMBERS.MEMBER_NUMBER = RPLMEMBERADDRESS.MEMBER_NUMBER
  --AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER = RPLPACKAGE.INSURANCE_COMPANY_NUMBER
  --AND MEMBERS_HISTORY.PACKAGE_NUMBER = RPLPACKAGE.PACKAGE_NUMBER
  --AND RPLNETWORK.NETWORK_ID = RPLPACKAGE.DEFAULT_NETWORK_ID";
                    #endregion
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.Add(":civilId", civilId);
                    dbParams.Add(":policyNo", pol);
                    dbParams.Add(":memberNo", memNo);
                    eCardValue = connection.Query<ECardValue>(query, commandType: CommandType.Text, param: dbParams).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return eCardValue;
        }
    }
}