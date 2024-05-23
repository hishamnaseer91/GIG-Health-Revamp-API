using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.LImitsDAL
{
    public class LimitsDAL : DBGenerics, ILimits
    {
        public IList<GetPolicies> GetPolicybyCivilId(LimitCheckModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = @"WITH MEMBERS AS (SELECT I.* FROM (SELECT J.*,
                                MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER,J.EVENT_NUMBER) AS MAX_DATE,
                                MAX(J.EVENT_NUMBER)                OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER) AS MAX_EVENT
                          
                           FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                          WHERE J.MODIFICATION_EFFECTIVE_DATE <= TO_CHAR (SYSDATE, 'DD-MON-YY')
                         AND (J.POLICY_NUMBER,J.MEMBER_NUMBER) IN (SELECT X.POLICY_NUMBER,X.MEMBER_NUMBER FROM mednext.RPLMEMBER X WHERE X.NATIONAL_IDENTITY = '" + model.CivilId + @"')) I WHERE I.MODIFICATION_EFFECTIVE_DATE       = MAX_DATE                    AND I.EVENT_NUMBER                      = MAX_EVENT
                   -- AND NVL(I.EXPIRY_DATE,I.DELETION_DATE) >= TO_CHAR (SYSDATE, 'DD-MON-YY')
),
     POLICIES AS (SELECT C.*
                    FROM (SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER,B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER (PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                           
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE B.MODIFICATION_EFFECTIVE_DATE <= TO_CHAR (SYSDATE, 'DD-MON-YY')
                             AND B.POLICY_NUMBER IN (SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER                = C.MAX_EVENT)
SELECT POLICIES.POLICY_HOLDER,POLICIES.POLICY_EFFECTIVE_DATE,
       MEMBERS.EXPIRY_DATE,
       MEMBERS.DELETION_DATE,
       MEMBERS.MEMBER_NUMBER,
             MEMBERS.POLICY_NUMBER
  FROM MEMBERS,
       POLICIES
WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER";
            var result = ExecuteList<GetPolicies>(query);
            return result;
        }

        public PatientBasicInfo GetPatientBasicInfo(GetPersonalInformationModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = string.Format(@"WITH MEMBERS AS (SELECT I.*
                   FROM (SELECT J.*,
                                MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER,J.EVENT_NUMBER) AS MAX_DATE,
                                MAX(J.EVENT_NUMBER)                OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER) AS MAX_EVENT
                           FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                          WHERE J.MODIFICATION_EFFECTIVE_DATE <= TO_CHAR (SYSDATE, 'DD-MON-YY')
                      AND J.POLICY_NUMBER      = {0}
                    AND J.MEMBER_NUMBER      = {1} ) I
                  WHERE I.MODIFICATION_EFFECTIVE_DATE       = MAX_DATE
                    AND I.EVENT_NUMBER                      = MAX_EVENT
                   -- AND NVL(I.EXPIRY_DATE,I.DELETION_DATE) >= TO_CHAR (SYSDATE, 'DD-MON-YY')
),
     POLICIES AS (SELECT C.*
                    FROM (SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER,B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER (PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE B.MODIFICATION_EFFECTIVE_DATE <= TO_CHAR (SYSDATE, 'DD-MON-YY')
                             AND B.POLICY_NUMBER IN (SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER                = C.MAX_EVENT)                     
SELECT POLICIES.POLICY_HOLDER ,MEMBERS.MEMBER_NUMBER ,POLICIES.EXPIRY_DATE POLICYEXPIRY,POLICIES.POLICY_EFFECTIVE_DATE ,
       MEMBERS.EXPIRY_DATE MEMBEREXPIRY,MEMBERS.MEMBER_EFFECTIVE_DATE,
       MEMBERS.DELETION_DATE,POLICIES.POLICY_NUMBER,
      MMB.FIRST_NAME || '  '||
       MMB.MIDDLE_NAME|| '  '||
       MMB.LAST_NAME ""NAME"",
      MMB.NATIONAL_IDENTITY,
       ADDRES.PHONE_NUMBER1,
             PKG.DEFAULT_NETWORK_ID,
       PKG.DEFAULT_NETWORK_DESCRIPTION,
        (select Value
    
          from mednext.RPLMEMBERCUSTOMFIELD CUST 
         where CUST.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
           and CUST.MEMBER_NUMBER = MEMBERS.MEMBER_NUMBER
           and CUST.FIELD_ID      = 'VIP'
          and CUST.VALUE         = 'Y') VIP_FLAG,
      ADDRES.ADDRESS,ADDRES.REGION_ID,ADDRES.DISTRICT_ID,ADDRES.PHONE_NUMBER2
  FROM 
       POLICIES, MEMBERS 
  
      LEFT OUTER JOIN mednext.RPLMEMBERADDRESS ADDRES 
          ON (MEMBERS.MEMBER_NUMBER = ADDRES.MEMBER_NUMBER)
      
      INNER JOIN mednext.RPLMEMBER MMB
          ON     (MEMBERS.MEMBER_NUMBER =
                  MMB.MEMBER_NUMBER)
             AND (MEMBERS.POLICY_NUMBER =
                     MMB.POLICY_NUMBER)
          
          INNER JOIN mednext.RPLPACKAGE PKG
          ON     (MEMBERS.PACKAGE_NUMBER =
                   PKG.PACKAGE_NUMBER)
             AND (MEMBERS.INSURANCE_COMPANY_NUMBER =
                     PKG.INSURANCE_COMPANY_NUMBER)          

WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER", model.PolicyNumber, model.MemberNumber);

            var result = db.ExecuteSingle<PatientBasicInfo>(query);
            return result;
        }

        public PolicyMemberNumber GetMemberPolicayNo(string civilId)
        {
            var Query = @"select policy_no , member_id from member_users where civil_id = :CivelID";
           
            return ExecuteSingle<PolicyMemberNumber>(Query, ParamBuilder.Par(":CivelID", civilId));

        }

        public MemberNumber GetMemberNo(string civilId, string polNo)
        {
            var Query = @"select POLICY_NUMBER AS POLICY_NO, MEMBER_NUMBER as MEMBER_ID from MEDNEXT.RPLMEMBER where NATIONAL_IDENTITY = :CivelID AND POLICY_NUMBER = :POL";

            return ExecuteSingle<MemberNumber>(Query, ParamBuilder.Par(":CivelID", civilId), ParamBuilder.Par(":POL", polNo));

        }

        public MemberUserGender GetMemberGender(string memberId) {

            DBGenerics db = new DBGenerics();
            string query = "select MEMBER_NUMBER,SEX_ID from MEDNEXT.RPLMEMBER where MEMBER_NUMBER = :memberId";
            return db.ExecuteSingle<MemberUserGender>(query, ParamBuilder.Par(":memberId", memberId));
        }

    }
}
