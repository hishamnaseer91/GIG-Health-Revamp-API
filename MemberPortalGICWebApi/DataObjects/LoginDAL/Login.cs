using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.LoginDAL
{
    public class LoginUser : DBGenerics, ILoginUser
    {
        public int CheckUserExistence(UserModel Model)
        {
            DBGenerics db = new DBGenerics();
            string Query = "select id from GMONLINE.MEMBER_USERS a where A.CIVIL_ID =:civilid and A.PASSWORD=:password";
            return ExecuteScalarInt32(Query, ParamBuilder.Par(":civilid", Model.UserName), ParamBuilder.Par(":password", Common.Encrypt(Model.Password)));

        }
        public int UserIsActiveOrNot(long id)
        {
            DBGenerics db = new DBGenerics();
            string query = "select IS_ACTIVE from member_users where ID=:id";
            return db.ExecuteScalarInt32(query, ParamBuilder.Par(":id", id));

        }

        public MappingUserModel BIZUserModel(string id)
        {
            DBGenerics db = new DBGenerics();
            #region Query
            string query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E, member_users 
                  WHERE 1=1
                  
                   --AND E.POLICY_NUMBER      = MEMBER_USERS.POLICY_NO
                --AND E.MEMBER_NUMBER      = MEMBER_USERS.MEMBER_ID
                 --and member_users.id=:id
                   AND E.POLICY_NUMBER      = MEMBER_USERS.POLICY_NO
                   and E.national_identity      = trim(MEMBER_USERS.CIVIL_ID)
                    and member_users.CIVIL_ID=:id
                    ),
     MEMBERS_HISTORY AS (SELECT I.*
                           FROM (SELECT J.*,
                                        MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER,J.EVENT_NUMBER) AS MAX_DATE,
                                        MAX(J.EVENT_NUMBER)                OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER) AS MAX_EVENT
                                   FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                  WHERE 1=1 
                                --and J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                    AND (J.POLICY_NUMBER,J.MEMBER_NUMBER) IN (SELECT G.POLICY_NUMBER,MEMBER_NUMBER FROM MEMBERS G)
                          ) I
                  WHERE I.MODIFICATION_EFFECTIVE_DATE       = MAX_DATE
                    AND I.EVENT_NUMBER                      = MAX_EVENT
                    --AND NVL(I.EXPIRY_DATE,I.DELETION_DATE) >= sysdate
),
     POLICIES AS (SELECT C.*
                    FROM (SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER,B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER (PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE 1=1
                            -- and B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                             AND B.POLICY_NUMBER IN (SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER                = C.MAX_EVENT)                     
SELECT  MEMBERS_HISTORY.PACKAGE_NUMBER, RPLNETWORK.NETWORK_DESCRIPTION,
MEMBERS.MEMBER_NUMBER,
         MEMBERS.FIRST_NAME || '  '||
       MEMBERS.MIDDLE_NAME|| '  '||
       MEMBERS.LAST_NAME NAME,
        MEMBERS.INTERNATIONAL_FIRST_NAME || '  '||
       MEMBERS.INTERNATIONAL_MIDDLE_NAME|| '  '||
       MEMBERS.INTERNATIONAL_LAST_NAME INTERNATIONAL_NAME,
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
       RPLMEMBERADDRESS.PHONE_NUMBER1,
       RPLMEMBERADDRESS.ADDRESS_TYPE_ID,
       MEMBERS.CLASS_ID,
       MEMBERS.RELATION_ID,
       MEMBERS.EFFECTIVE_DATE,
       MEMBERS.SEX_ID    GENDER_ID,
       MEMBERS.DATE_OF_BIRTH, 
       MEMBERS.PRINCIPAL_FLAG PRINCIPALINDICATOR,
       MEMBERS.RELATION_DESCRIPTION,
        MEMBERS.PRINCIPAL_NAME ,
       RPLMEMBERADDRESS.ADDRESS,
       RPLMEMBERADDRESS.REGION_ID,
       RPLMEMBERADDRESS.DISTRICT_ID
  FROM MEMBERS,
       MEMBERS_HISTORY,
       POLICIES,
       mednext.RPLPACKAGE,
       mednext.RPLNETWORK,
       mednext.RPLMEMBERADDRESS
      
WHERE MEMBERS.POLICY_NUMBER                     = POLICIES.POLICY_NUMBER
  AND MEMBERS.POLICY_NUMBER                     = MEMBERS_HISTORY.POLICY_NUMBER
  AND MEMBERS.MEMBER_NUMBER                     = MEMBERS_HISTORY.MEMBER_NUMBER
  AND MEMBERS.MEMBER_NUMBER                     = RPLMEMBERADDRESS.MEMBER_NUMBER(+)
  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER  = RPLPACKAGE.INSURANCE_COMPANY_NUMBER
  AND MEMBERS_HISTORY.PACKAGE_NUMBER            = RPLPACKAGE.PACKAGE_NUMBER
  AND RPLNETWORK.NETWORK_ID= RPLPACKAGE.DEFAULT_NETWORK_ID";
            #endregion
            return db.ExecuteSingle<MappingUserModel>(query, ParamBuilder.Par(":id", id));
        }

        public ActivePolicyModels GetActivePolicyCivilId(string civilID)
        {
            DBGenerics db = new DBGenerics();
            var query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E
                  WHERE 1=1
               AND (E.POLICY_NUMBER,E.MEMBER_NUMBER) IN (SELECT X.POLICY_NUMBER,X.MEMBER_NUMBER FROM  mednext.RPLMEMBER X WHERE X.NATIONAL_IDENTITY = :NantionalCivilId)
             
                    ),
                    MEMBERS_HISTORY AS (SELECT I.*
                           FROM (SELECT J.*,
                                        MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER,J.EVENT_NUMBER) AS MAX_DATE,
                                        MAX(J.EVENT_NUMBER)                OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER) AS MAX_EVENT
                                   FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                  WHERE 1=1 
                                 --and J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                    AND (J.POLICY_NUMBER,J.MEMBER_NUMBER) IN (SELECT G.POLICY_NUMBER,MEMBER_NUMBER FROM MEMBERS G)
                          ) I
                  WHERE I.MODIFICATION_EFFECTIVE_DATE       = MAX_DATE
                    AND I.EVENT_NUMBER                      = MAX_EVENT
                    --AND NVL(I.EXPIRY_DATE,I.DELETION_DATE) >= sysdate
),
     POLICIES AS (SELECT C.*
                    FROM (SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER,B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER (PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE 1=1 
                                --and B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                             AND B.POLICY_NUMBER IN (SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER                = C.MAX_EVENT)  
SELECT POLICIES.POLICY_HOLDER,POLICIES.POLICY_EFFECTIVE_DATE,
members.NATIONAL_IDENTITY,
       MEMBERS.EXPIRY_DATE,
       MEMBERS.DELETION_DATE,
       MEMBERS.MEMBER_NUMBER,
             MEMBERS.POLICY_NUMBER,
            MEMBERS.First_NAme || ' ' || MEMBERS.Middle_NAME || ' ' || MEMBERS.last_name as Name ,
             pkg.DEFAULT_NETWORK_ID as NETWORK_ID
       
  FROM MEMBERS,MEMBERS_HISTORY,  mednext.RPLPACKAGE pkg,
       POLICIES
WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
 AND MEMBERS.POLICY_NUMBER                     = MEMBERS_HISTORY.POLICY_NUMBER
  AND MEMBERS.MEMBER_NUMBER                     = MEMBERS_HISTORY.MEMBER_NUMBER
  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER  = pkg.INSURANCE_COMPANY_NUMBER
  AND MEMBERS_HISTORY.PACKAGE_NUMBER            = pkg.PACKAGE_NUMBER
   and trunc(sysdate) between POLICIES.POLICY_EFFECTIVE_DATE and MEMBERS.EXPIRY_DATE
 and POLICIES.CANCELLATION_DATE is NULL   and MEMBERS.DELETION_DATE is NULL
  order by POLICIES.POLICY_NUMBER asc";
            return db.ExecuteSingle<ActivePolicyModels>(query, ParamBuilder.Par(":NantionalCivilId", civilID));
        }

        public int UpdatePolicyNumber(ActivePolicyModels model)
        {

            DBGenerics db = new DBGenerics();

            //  var query = "select Civil_ID from member_users where civil_id =:civilId and Medical_Insurance_Card=:medicalCardNo  and Passwprd =:password";
            //var query = "Update member_users set POLICY_NO = :polNo, UPDATED_DATE = sysdate where civil_id =:civilId   RETURNING Id INTO :my_id_param";
            // var query = "Update member_users set POLICY_NO = :polNo, UPDATED_DATE = sysdate where civil_id =:civilId and Medical_Insurance_Card=:medicalCardNo   RETURNING Id INTO :my_id_param";
            //var result = db.ExecuteNonQueryOracle(query, ":my_id_param", ParamBuilder.Par(":civilId", model.CivilId), ParamBuilder.Par(":polNo", model.POLICY_NUMBERs), ParamBuilder.OutPar(":my_id_param", 0));

            var query = "Update member_users SET POLICY_NO='" + model.POLICY_NUMBERs + "' ,MEMBER_ID='" + model.MEMBER_NUMBERs + "',Medical_Insurance_Card='" + model.MEMBER_NUMBERs + "', UPDATED_DATE = sysdate where civil_id =:civilId RETURNING Id INTO :my_id_param";
            var result = db.ExecuteNonQueryOracle(query, ":my_id_param", ParamBuilder.Par(":civilId", model.CivilId), ParamBuilder.OutPar(":my_id_param", 0));

            return result;
        }

    }
}